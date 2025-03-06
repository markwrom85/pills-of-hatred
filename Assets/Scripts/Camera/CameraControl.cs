using Unity.Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    private Camera cameraObject;
    [SerializeField] private RealPlayerMovement player;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    void Awake()
    {
        cameraObject = GetComponent<Camera>();

        cameraObject.gameObject.layer = LayerMask.NameToLayer("Player" + player.playerID);
        if (player.playerID == 1)
        {
            cameraObject.cullingMask &= ~(1 << LayerMask.NameToLayer("Player2"));
        }
        else if (player.playerID == 2)
        {
            cameraObject.cullingMask &= ~(1 << LayerMask.NameToLayer("Player1"));
        }

        if (cinemachineCamera != null)
        {
            cinemachineCamera.OutputChannel = player.playerID == 1 ? OutputChannels.Channel01 : OutputChannels.Channel02;
            cameraObject.GetComponent<CinemachineBrain>().ChannelMask = player.playerID == 1 ? OutputChannels.Channel01 : OutputChannels.Channel02;
        }
    }

    void Update()
    {

    }
}
