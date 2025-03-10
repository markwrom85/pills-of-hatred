using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    private int health;
    public bool isInvincible;

    [Header("Hate System")]
    public float hateCount = 1f;
    private int hateMax = 100;
    public float pickupHateIncrease = 1f;
    public float killHateIncrease = 5f;
    public float parryHateIncrease = 3f;

    private float pickupScore = 1f;
    private float killScore = 5;

    //hate drain
    private bool hateDrain;
    public float drainCooldown = 5f;
    public float drainAmount = 2f;


    [Header("Player Components")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerUI playerUI;

    private bool shootInput;
    private bool canShoot = true;
    [Header("Shoot Components")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootTarget;
    [SerializeField] private Transform shootPoint;
    public Vector3 shootDirection;
    private float bulletSpeed = 30f;


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
        shootDirection = (shootTarget.position - transform.position).normalized;
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

        if(hateDrain)
        {
            hateCount -= drainAmount * Time.deltaTime;
            if(hateCount <= 1){
                hateCount = 1;
                hateDrain = false;
            }
        }
    }

    public void Hurt(int damage)
    {
        if(!isInvincible)
        {
            health -= damage;
        }
        else{
            StartCoroutine(InvincibleCooldown());
        }
        //Debug.Log($"Health: {health}");
    }

    private IEnumerator InvincibleCooldown(){
        isInvincible = true;
        yield return new WaitForSeconds(.5f);
        isInvincible = false;
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

    public void HateScoreCalc(float hateIncrease)
    {
        hateCount += hateIncrease;

        //clamp hateCount
        if (hateCount > hateMax)
        {
            hateCount = hateMax;
        }

        //hateCount goes down
        StartCoroutine(HateDrain());
    }

    private IEnumerator HateDrain()
    {
        hateDrain = false;
        yield return new WaitForSeconds(drainCooldown);
        hateDrain = true;
    }

    private IEnumerator Shoot()
    {
        if (shootTarget != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position + shootDirection, Quaternion.LookRotation(shootDirection));
            bullet.GetComponent<Fireball>().speed = bulletSpeed;
            float currentSpeed = bullet.GetComponent<Fireball>().speed;
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = shootDirection * currentSpeed; // Set the bullet speed
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
        yield return new WaitForSeconds(.2f);
        Destroy(currentPunch);
        yield return new WaitForSeconds(.25f);
        canPunch = true;
    }
}
