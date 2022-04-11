using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace Lotfy
{
    public class CharacterControl : MonoBehaviour
    {
        #region >>>>>>(Componentes)<<<<<<
        public CharacterController _controller
        {
            get
            {
                if (Controller == null)
                {
                    Controller = GetComponent<CharacterController>();
                }

                return Controller;
            }
        }
        private CurvesAndRotaion CAndR;

        public CurvesAndRotaion _curveAndRot
        {
            get
            {
                if (CAndR == null)
                {
                    CAndR = GetComponent<CurvesAndRotaion>();
                }

                return CAndR;
            }
        }
        #endregion

        private bool groundedPlayer;
        public Vector3 InputVector { get; private set; }

        [SerializeField] private float playerSpeed = 5.0f;
        [SerializeField] private float maxPlayerSpeed = 5.0f;

        private Vector3 movementVector = Vector3.zero;

        private CharacterController Controller;

        private float maxJumpHeight = 3;
        private float maxJumpTime = 1;
        private float gravityJump;
        private float InitalJumpVelocity;
        private bool isJumping = false;
        private float gravityMultiplier = 1;
        
        private Direction CurrentDirction;
        public float LerpCurveTime = 0.1f;


        private void SetJumpEqaution()
        {
            float timeToApex = maxJumpTime / 2f;
            InitalJumpVelocity = (2 * maxJumpHeight) / timeToApex;
            gravityJump = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        }
        private void Start()
        {
            SetJumpEqaution();
        }

        private void Update()
        {
            //Debug
            SpeedMonitor();
            CurveDistance();
            
            HandleJump();
            ApplyGravity();
            MoveForwared();
        }
        private void FixedUpdate()
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                _curveAndRot._PathHandler.CalculateDirFromClosestPoint(CurrentDirction, transform.position,
                    out _curveAndRot.Line);
                transform.position = Vector3.Slerp(transform.position, new Vector3(_curveAndRot.Line.x, transform.position.y, _curveAndRot.Line.z), LerpCurveTime*curveDistance);
            }
        }

        public void MoveForwared()
        {
            switch (Input.GetAxisRaw("Horizontal"))
            {
                case 1:
                    UpdateMovementVector(Direction.Forward);
                    break;
                case -1:
                    UpdateMovementVector(Direction.Backword);
                    break;
            }

        }
        private void UpdateMovementVector(Direction Dir)
        {
            InputVector = _curveAndRot._PathHandler.CalculateDirFromClosestPoint(Dir, _curveAndRot.Line, out _curveAndRot.Line);
            
            if (Input.GetAxisRaw("Horizontal")== 0)
            {
                movementVector.z = movementVector.x = 0;
            }

            movementVector.x += playerSpeed * Time.deltaTime * InputVector.x;
            movementVector.z += playerSpeed * Time.deltaTime * InputVector.z;

            movementVector.x = Mathf.Clamp(movementVector.x, -maxPlayerSpeed, maxPlayerSpeed);
            movementVector.z = Mathf.Clamp(movementVector.z, -maxPlayerSpeed, maxPlayerSpeed);

            _controller.Move(movementVector * Time.deltaTime * maxPlayerSpeed);


            transform.rotation = Quaternion.LookRotation(InputVector);
        }
        
        private float speedPerSec;
        Vector3 oldPosition;
        void SpeedMonitor()
        {
            speedPerSec = Vector3.Distance(oldPosition, transform.position) / Time.deltaTime;
            Grapher.Log(speedPerSec,"Current Speed",Color.gray);
            oldPosition = transform.position;
        }
        private float curveDistance;
        void CurveDistance()
        {
            curveDistance = Vector3.Distance(transform.position, new Vector3(_curveAndRot.Line.x, transform.position.y, _curveAndRot.Line.z));
            Grapher.Log(curveDistance,"Curve Distance", Color.blue);
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
            if (_controller.isGrounded && isJumping && movementVector.y <= 0)
            {
                isJumping = false;
                gravityMultiplier = 1;
            }
        }

        private void IsJumping()
        {
            if (isJumping)
            {
                if (Input.GetKeyUp(KeyCode.Space) || movementVector.y <= 0)
                    gravityMultiplier = 1.5f;
            }
        }

        private void IsStartingToJump()
        {
            if (_controller.isGrounded && !isJumping && Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                movementVector.y = InitalJumpVelocity;
            }
        }

        private void ApplyGravity()
        {
            if (!_controller.isGrounded)
            {
                movementVector.y += gravityJump * Time.deltaTime * gravityMultiplier;
            }
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
