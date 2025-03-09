using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    private int health;

    [Header("Hate System")]
    public int hateCount = 1;
    private int hateMax = 100;
    public int pickupHateIncrease = 1;
    public int killHateIncrease = 5;
    public int parryHateIncrease = 3;

    private int pickupScore = 1;
    private int killScore = 5;

    [Header("Player Components")]
    [SerializeField] private PlayerInput playerInput;
    private bool shootInput;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootTarget;
    [SerializeField] private Transform shootPoint;
    private RealPlayerMovement parentObj;

    [SerializeField] private PlayerUI playerUI;

    void Start()
    {
        health = 5;
        hateCount = 1;

        parentObj = GetComponentInParent<RealPlayerMovement>();
        Debug.Log(parentObj);
    }

    void Update()
    {
        shootInput = playerInput.actions["Shoot"].WasPressedThisFrame();
        if (shootInput)
        {
            Die();
            //Shoot();
        }
    }

    public void Hurt(int damage)
    {
        health -= damage;
        Debug.Log($"Health: {health}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pickup")
        {
            playerUI.AddPlayerScore(pickupScore * hateCount);
            HateScoreCalc(pickupHateIncrease);
            Destroy(other.gameObject);
        }
    }

    private void HateScoreCalc(int hateIncrease)
    {
        hateCount += hateIncrease;

        //clamp hateCount
        if (hateCount > hateMax)
        {
            hateCount = hateMax;
        }
    }

    private void Shoot()
    {
        if (shootTarget != null)
        {
            Vector3 direction = (shootTarget.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position + direction, Quaternion.LookRotation(direction));
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = direction * 20f; // Set the bullet speed
            }
        }
    }

    private void Die(){
        Destroy(parentObj.gameObject);
    }
}
