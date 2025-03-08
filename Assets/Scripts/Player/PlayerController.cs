using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
public class PlayerController : MonoBehaviour
{
    public int playerID;
    public float moveSpeed = 150f;
    public float jumpHeight = 60f;
    public float gravityForce = 4500f;
    private Vector2 moveInput;
    private bool jumpInput;
    public bool canTeleport = true;
    private PlayerInput playerInput;
    private Rigidbody rb;
    public Vector3 moveDirection;
    public float teleportDistance = 4.5f;


    private bool isGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            return true;
        }
        return false;
    }

    void Start() //sample used Awake(), changed it so that two players can use the keyboard
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        playerID = GetComponent<PlayerInput>().user.index + 1;
        /*switch (playerID)
        {
            //not sure where currentActionMap shows in the inspector, but need to make sure
            //current gets changed instead of the default. default won't allow for player2 movement
            case 1:
                playerInput.SwitchCurrentActionMap("Player1");
                break;
            case 2:
                playerInput.SwitchCurrentActionMap("Player2");
                break;
        }*/
    }
    void Update()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        jumpInput = playerInput.actions["Jump"].WasPressedThisFrame();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if(moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * moveSpeed);
        }
        //gameObject.transform.rotation = Quaternion.LookRotation(moveDirection);
        //gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, Quaternion.LookRotation (moveDirection), Time.deltaTime * 40f);


        /*Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        if (jumpInput)
        {
            transform.position += Vector3.up * jumpHeight;
        }*/

        if (isGrounded())
        {
            rb.AddRelativeForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * (jumpInput ? 1 : 0))/*multiply by result of if jump is pressed*/, ForceMode.VelocityChange);
        }

        if (canTeleport && playerInput.actions["Teleport"].WasPressedThisFrame()) //can't have the canTeleport bool tied to the button being pressed because the bool also needs to change in the coroutine
        {
            StartCoroutine(Teleport());
        }
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.down * gravityForce * Time.deltaTime, ForceMode.Acceleration);
        rb.AddRelativeForce(moveDirection * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
        rb.AddRelativeTorque(moveDirection * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);

    }

    void OnCollisionEnter(Collision collision)
    {
        /*ReactiveTarget otherPlayer = collision.gameObject.GetComponent<ReactiveTarget>();
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
        }*/
    }

    private IEnumerator Teleport()
    {
        if (moveDirection != Vector3.zero) //teleport in the direction the player is moving
        {
            gameObject.transform.position += moveDirection * teleportDistance;
        }
        /*else //if player isn't moving, teleport in the direction the player is facing
        //DOESN'T WORK BECAUSE NEED TO CHANGE ROTATION DIRECTION STILL!!
        {
            gameObject.transform.position += Vector3.forward * teleportDistance;
        }*/
        Physics.SyncTransforms();
        canTeleport = false;
        yield return new WaitForSeconds(.5f);
        canTeleport = true;
    }
}