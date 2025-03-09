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
    [SerializeField] private PlayerUI playerUI;

    private bool shootInput;
    private bool canShoot = true;
    [Header("Shoot Components")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootTarget;
    [SerializeField] private Transform shootPoint;


    private bool canPunch = true;
    [Header("Punch Components")]
    [SerializeField] private GameObject punchPrefab;
    [SerializeField] private Transform orientation;
    private GameObject currentPunch;
    private bool punchInput;


    void Start()
    {
        health = 5;
        hateCount = 1;
    }

    void Update()
    {
        shootInput = playerInput.actions["Shoot"].WasPressedThisFrame();
        if (shootInput && canShoot)
        {
            StartCoroutine(Shoot());
        }

        punchInput = playerInput.actions["Punch"].WasPressedThisFrame();
        if (punchInput && canPunch)
        {
            StartCoroutine(Punch());
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

    /*private void Shoot()
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
    }*/

    private IEnumerator Shoot()
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
        canShoot = false;
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }

    private IEnumerator Punch()
    {
        currentPunch = Instantiate(punchPrefab, orientation.transform);
        canPunch = false;
        yield return new WaitForSeconds(1f);
        Destroy(currentPunch);
        canPunch = true;
    }
}
