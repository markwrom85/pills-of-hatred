using UnityEngine;
using UnityEngine.InputSystem;

public class RealPlayerMovement : MonoBehaviour
{
    public int playerID;
    public float moveSpeed = 150f;
    public float jumpHeight = 60f;
    public Rigidbody rb;
    public float gravityForce = 4500f;
    private Vector2 moveInput;
    private PlayerInput playerInput;
    private Vector3 moveDirection;
    private bool jumpInput;
    private bool isGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            return true;
        }
        return false;
    }

    [SerializeField] private GameObject camFollow;
    public Vector2 cameraInput;
    public float camRotationPower = 1.5f;

[SerializeField] private Transform cam;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        playerID = GetComponent<PlayerInput>().user.index + 1;

    }

    void Update()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        jumpInput = playerInput.actions["Jump"].WasPressedThisFrame();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = new Vector3(cam.forward.x, 0, cam.forward.z) * moveInput.y;
        moveDirection += cam.right * moveInput.x;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * moveSpeed);
        }
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (isGrounded())
        {
            rb.AddRelativeForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * (jumpInput ? 1 : 0))/*multiply by result of if jump is pressed*/, ForceMode.VelocityChange);
        }

        //CameraRotation();

        
    }


    void OnCollisionEnter(Collision collision)
    {
        ReactiveTarget otherPlayer = collision.gameObject.GetComponent<ReactiveTarget>();
        if (otherPlayer != null)
        {
            switch (playerID)
            {
                case 1:
                    FindFirstObjectByType<UIController>().AddPlayer1Score(1);
                    break;
                case 2:
                    FindFirstObjectByType<UIController>().AddPlayer2Score(1);
                    break;
            }
        }
    }

    void FixedUpdate(){
        rb.AddRelativeForce(Vector3.down * gravityForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        rb.AddRelativeForce(moveDirection * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        //rb.AddRelativeTorque(moveDirection * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

    }

    private void CameraRotation(){
        cameraInput = playerInput.actions["Look"].ReadValue<Vector2>().normalized;
        camFollow.transform.rotation *= Quaternion.AngleAxis(cameraInput.x * camRotationPower, Vector3.up);

        camFollow.transform.rotation *= Quaternion.AngleAxis(cameraInput.y * camRotationPower, Vector3.right);

        var angles = camFollow.transform.localEulerAngles;
        angles.z = 0;

        var angle = camFollow.transform.localEulerAngles.x;
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        camFollow.transform.localEulerAngles = angles;

        //transform.rotation = Quaternion.Euler(0, camFollow.transform.rotation.eulerAngles.y,0);
        //camFollow.transform.localEulerAngles = new Vector3(angles.x, 0,0);
    }
}