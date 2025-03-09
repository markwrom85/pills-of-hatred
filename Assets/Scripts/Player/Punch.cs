using Unity.VisualScripting;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private int myPlayerID;
    private Vector3 myShootDirection;
    public float bulletSpeedMult = 1.5f;
    void Start()
    {
        myPlayerID = GetComponentInParent<RealPlayerMovement>().playerID;
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
            myShootDirection = GetComponentInParent<PlayerCharacter>().shootDirection;
            Rigidbody bulletRb = other.gameObject.GetComponent<Rigidbody>();
            float currentSpeed = other.gameObject.GetComponent<Fireball>().speed;
            Debug.Log(currentSpeed);
            bulletRb.linearVelocity = myShootDirection * (currentSpeed * bulletSpeedMult);

            /*GameObject bullet = Instantiate(bulletPrefab, shootPoint.position + shootDirection, Quaternion.LookRotation(shootDirection));
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = shootDirection * 20f; // Set the bullet speed
            }*/
        }
    }
}
