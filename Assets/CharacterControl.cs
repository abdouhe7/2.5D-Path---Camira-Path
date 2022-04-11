using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public Vector3 InputVector { get; private set; }

    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float maxPlayerSpeed = 5.0f;

    //[SerializeField] private float jumpHeight = 1.0f;
    //[SerializeField] private float gravityValue = -9.81f;
    private Vector3 movementVector = Vector3.zero;

    private CharacterController controller;
    private bool groundedPlayer;
    private float maxJumpHeight = 3;
    private float maxJumpTime = 1;
    private float gravityJump;
    private float InitalJumpVelocity;
    private bool isJumping = false;
    private float gravityMultiplier = 1;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        SetJumpEqaution();
    }
    private void SetJumpEqaution()
    {
        float timeToApex = maxJumpTime / 2f;
        InitalJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        gravityJump = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
    }
    
    private void GetInput()
    {
        InputVector  = new Vector3(0, 0, Input.GetAxisRaw("Horizontal"));
    }
    private void UpdateMovementVector()
    {
        if (InputVector.z == 0)
        {
            movementVector.z = 0;
        }
        movementVector.z += playerSpeed * Time.deltaTime * InputVector.z;
        movementVector.z = Mathf.Clamp(movementVector.z, -maxPlayerSpeed, maxPlayerSpeed);
    }
    private float speedPerSec;
    Vector3 oldPosition;
    void SpeedMonitor()
            {
                speedPerSec = Vector3.Distance(oldPosition, transform.position) / Time.deltaTime;
                //speed = Vector3.Distance (oldPosition, transform.position);
                oldPosition = transform.position;
            }
    private void Update()
    {
    SpeedMonitor();
        HandleJump();
        ApplyGravity();
        GetInput();
        UpdateMovementVector();
        controller.Move(movementVector * Time.deltaTime);
    }
    
    private void HandleJump()
    {
        IsStartingToJump();
        IsJumping();
        IsReturnedFromJumping();
    }

    private void IsReturnedFromJumping()
    {
        //we returning from jumping stat
        if (controller.isGrounded && isJumping && movementVector.y <= 0)
        {
            isJumping = false;
            gravityMultiplier = 1;
        }
    }

    private void IsJumping()
    {
        if (isJumping)
        {
            if (Input.GetKeyUp(KeyCode.Space) ||  movementVector.y <= 0)
                gravityMultiplier = 1.5f;
        }
    }

    private void IsStartingToJump()
    {
        if (controller.isGrounded && !isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            movementVector.y = InitalJumpVelocity;
        }
    }

    private void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            movementVector.y += gravityJump * Time.deltaTime * gravityMultiplier;
        }
    }

    //private void Update()
    //{
    //    ApplyGravity();
    //    InputVector  = new Vector3(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
    //    if (InputVector.z == 0)
    //    {
    //        movementVector.z = 0;
    //    }
    //    movementVector.z += playerSpeed * Time.deltaTime * InputVector.z;
    //    movementVector.z = Mathf.Clamp(movementVector.z, -maxPlayerSpeed, maxPlayerSpeed);
    //    controller.Move(movementVector * Time.deltaTime);
    //}

    ////No Jump Logic
    //private void Update()
    //{
    //    InputVector  = new Vector3(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
    //    if (InputVector.z == 0)
    //    {
    //        movementVector.z = 0;
    //    }
    //    movementVector.z += playerSpeed * Time.deltaTime * InputVector.z;
    //    movementVector.z = Mathf.Clamp(movementVector.z, -maxPlayerSpeed, maxPlayerSpeed);
    //    controller.Move(movementVector * Time.deltaTime);
    //}

    //No velocity , character controller
    //private void Update()
    //{
    //    InputVector  = new Vector3(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
    //    controller.Move(InputVector * Time.deltaTime * playerSpeed);
    //}

    //The default unity logic
    //void Update()
    //{
    //    groundedPlayer = controller.isGrounded;
    //    if (groundedPlayer && playerVelocity.y < 0)
    //    {
    //        playerVelocity.y = 0f;
    //    }

    // Vector3 move = new Vector3(Input.GetAxis("Vertical") , 0, Input.GetAxis("Horizontal"));
    // controller.Move(move * Time.deltaTime * playerSpeed);

    // if (move != Vector3.zero) { gameObject.transform.forward = move; }

    // // Changes the height position of the player.. if (Input.GetButtonDown("Jump") &&
    // groundedPlayer) { playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue); }

    //    playerVelocity.y += gravityValue * Time.deltaTime;
    //    controller.Move(playerVelocity * Time.deltaTime);
    //}
}