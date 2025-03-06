using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField] private Camera cameraObject;
    private RealPlayerMovement player;
    void Awake()
    {
        player = GetComponent<RealPlayerMovement>();

        cameraObject.gameObject.layer = LayerMask.NameToLayer("Player" + player.playerID);
        if (player.playerID == 1)
        {
            cameraObject.cullingMask &= ~(1 << LayerMask.NameToLayer("Player2"));
        }
        else if (player.playerID == 2)
        {
            cameraObject.cullingMask &= ~(1 << LayerMask.NameToLayer("Player1"));
        }
    }

    void Update()
    {

    }
}
