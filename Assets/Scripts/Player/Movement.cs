using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    private bool jumpInput;

    public float playerHeight;
    public LayerMask groundMask;
    bool grounded;

    public Transform orientation;
    
    public PlayerInput playerInput;
    private Vector2 moveInput;
    private Vector3 moveDirection;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        SpeedControl();
        MyInput();

        //handle drag
        if(grounded){
            rb.linearDamping = groundDrag;
        }else{
            rb.linearDamping = 0;
        }
    }

    private void MyInput(){
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        jumpInput = playerInput.actions["Jump"].WasPressedThisFrame();

        if(jumpInput && readyToJump && grounded){
            readyToJump = false;
            
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void MovePlayer(){
        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        //on ground
        if(grounded){
        rb.AddForce(moveDirection.normalized * moveSpeed * Time.deltaTime, ForceMode.Force);
        }
        //in air
        else{
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * Time.deltaTime, ForceMode.Force);
        }
    }

    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //limit velocity if needed
        if(flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump(){
        //resest y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump(){
        readyToJump = true;
    }
}
