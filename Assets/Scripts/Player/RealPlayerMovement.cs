using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RealPlayerMovement : MonoBehaviour
{
    public int playerID;
    public float moveSpeed = 150f;
    public float jumpHeight = 60f;
    public Rigidbody rb;
    public float gravityForce = 4500f;
    private Vector2 moveInput;
    private bool jumpInput;
    private PlayerInput playerInput;
    private Vector3 moveDirection;
    public Transform orientation;

    public int jumpCount = 2;

    /*private bool isGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            return true;
        }
        return false;
    }*/

    private bool dashInput;
    public float dashDistance = 50f;
    public float dashCount = 3f;
    private float maxDash = 3f;
    private float dashCooldown = .75f;

    //[SerializeField] private PlayerUI playerUI;

    public bool canMove = true;

    public Slider dashSlider;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        playerID = GetComponent<PlayerInput>().user.index + 1;
        rb.freezeRotation = true;
        jumpCount = 2;
    }

    void Start()
    {
        dashCount = maxDash;
        dashSlider.maxValue = maxDash;
        dashSlider.value = dashCount;
        canMove = true;
    }

    void Update()
    {
        //SpeedControl();
        if(canMove)
        {
            MyInputs();
        }
        
        //charge dash
        if (dashCount < 3)
        {
            dashCount += dashCooldown * Time.deltaTime ;
            if(dashCount > 3)
            {
                dashCount = 3;
            }
            dashSlider.value = dashCount;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void OnCollisionEnter(Collision collision)
    {
        ReactiveTarget otherPlayer = collision.gameObject.GetComponent<ReactiveTarget>();
        if (otherPlayer != null)
        {
            /*switch (playerID)
            {
                case 1:
                    FindFirstObjectByType<UIController>().AddPlayer1Score(1);
                    break;
                case 2:
                    FindFirstObjectByType<UIController>().AddPlayer2Score(1);
                    break;
            }*/

            //playerUI.AddPlayerScore(1);
        }

        //ground check
        if (collision.gameObject.layer == LayerMask.NameToLayer("groundMask"))
        {
            jumpCount = 2;
        }
    }

    private void MyInputs()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        jumpInput = playerInput.actions["Jump"].WasPressedThisFrame();
        if (jumpInput && jumpCount > 0)
        {
            Jump();
        }

        dashInput = playerInput.actions["Dash"].WasPressedThisFrame();
        if (dashInput && dashCount >= 1)
        {
            Dash();
        }
    }

    private void MovePlayer()
    {
        //player moves in direction they face
        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        rb.AddRelativeForce(moveDirection.normalized * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        //constant downward force on player
        rb.AddRelativeForce(Vector3.down * gravityForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //resest y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddRelativeForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        jumpCount--;
    }

    private void Dash()
    {
        GetComponent<PlayerCharacter>().isInvincible = true;
        Vector3 dashDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        if (dashDirection != Vector3.zero)
        {
            rb.AddRelativeForce(dashDirection.normalized * dashDistance, ForceMode.VelocityChange);
        }else{
            rb.AddRelativeForce(orientation.forward * dashDistance, ForceMode.VelocityChange);
        }
        dashCount--;
    }
}