using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    private Camera cameraObject;
    
    [SerializeField] private RealPlayerMovement id;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Rigidbody rb;
    public float rotationSpeed;
    [SerializeField] private PlayerInput playerInput;
    private Vector2 moveInput;

    public bool exploreCam;
    public Transform combatLookAt;


    void Awake()
    {
        cameraObject = GetComponent<Camera>();

        cameraObject.gameObject.layer = LayerMask.NameToLayer("Player" + id.playerID);
        if (id.playerID == 1)
        {
            cameraObject.cullingMask &= ~(1 << LayerMask.NameToLayer("Player2"));
        }
        else if (id.playerID == 2)
        {
            cameraObject.cullingMask &= ~(1 << LayerMask.NameToLayer("Player1"));
        }

        if (cinemachineCamera != null)
        {
            cinemachineCamera.OutputChannel = id.playerID == 1 ? OutputChannels.Channel01 : OutputChannels.Channel02;
            cameraObject.GetComponent<CinemachineBrain>().ChannelMask = id.playerID == 1 ? OutputChannels.Channel01 : OutputChannels.Channel02;
        }

    }

    void Update()
    {
        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //rotate player object
        if (exploreCam)
        {
            moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 inputDir = orientation.forward * moveInput.y + orientation.right * moveInput.x;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }
}
