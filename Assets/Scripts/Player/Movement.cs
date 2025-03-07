using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
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
        MyInput();
    }

    private void MyInput(){
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
    }
    private void MovePlayer(){
        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        rb.AddForce(moveDirection.normalized * moveSpeed * Time.deltaTime, ForceMode.Force);
    }
}
