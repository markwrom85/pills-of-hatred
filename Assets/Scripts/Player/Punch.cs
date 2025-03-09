using Unity.VisualScripting;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private int myPlayerID;
    void Start()
    {
        myPlayerID = GetComponentInParent<RealPlayerMovement>().playerID;
    }

    void OnTriggerEnter(Collider other)
    {
        RealPlayerMovement otherPlayer = other.gameObject.GetComponentInParent<RealPlayerMovement>();

//        Debug.Log(otherPlayer.playerID);
        if(otherPlayer != null && otherPlayer.playerID != myPlayerID)
        {
            otherPlayer.gameObject.GetComponent<PlayerCharacter>().Hurt(1);
        }
    }
}
