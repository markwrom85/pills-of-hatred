using Unity.VisualScripting;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private int myPlayerID;
    private Vector3 myShootDirection;
    public float bulletSpeedMult = 1.5f;
    private float hateIncrease;
    private PlayerCharacter myPlayerCharacter;

    void Start()
    {
        myPlayerID = GetComponentInParent<RealPlayerMovement>().playerID;
        myPlayerCharacter = GetComponentInParent<PlayerCharacter>();
        hateIncrease = myPlayerCharacter.parryHateIncrease;
    }
    void OnTriggerEnter(Collider other)
    {
        RealPlayerMovement otherPlayer = other.gameObject.GetComponentInParent<RealPlayerMovement>();

        //        Debug.Log(otherPlayer.playerID);
        if (otherPlayer != null && otherPlayer.playerID != myPlayerID)
        {
            otherPlayer.gameObject.GetComponent<PlayerCharacter>().Hurt(1);
        }
        if (other.tag == "bullet")
        {
            Parry(other.gameObject);
        }
    }

    private void Parry(GameObject other)
    {
        //make sure bullet is parried in the right direction
        myShootDirection = myPlayerCharacter.shootDirection;

        //alter speed of bullet
        other.gameObject.GetComponent<Fireball>().speed *= bulletSpeedMult;
        float currentSpeed = other.gameObject.GetComponent<Fireball>().speed;

        //bullet is parried in look direction and sped up
        Rigidbody bulletRb = other.gameObject.GetComponent<Rigidbody>();
        bulletRb.linearVelocity = myShootDirection * currentSpeed;

        //add to hate meter
        myPlayerCharacter.HateScoreCalc(hateIncrease);
    }
}
