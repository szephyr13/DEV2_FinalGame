using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravityFactor;
    [SerializeField] private Transform cam; 
    private Vector3 movementDirection;
    private Vector3 inputDirection; 
    private Vector3 verticalSpeed;


    [Header("Ground Detection")]
    [SerializeField] private Transform feetPosition;
    [SerializeField] private float groundDetectionDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float jumpHeight;



    //input, controller, animations
    [Header("Input")]
    [SerializeField] private InputManagerSO inputManager;
    private CharacterController controller;
    private Animator anim;

    void OnEnable()
    {
        inputManager.OnJumping += JumpAction;
        inputManager.OnMoving += MoveAction;
    }

    private void MoveAction(Vector2 ctx)
    {
        inputDirection = new Vector3(ctx.x, 0, ctx.y);
    }

    private void JumpAction()
    {
        if (CheckOnGround()) 
        {
            //verticalSpeed.y = Mathf.Sqrt(-2 * gravityFactor * jumpHeight);
            anim.SetTrigger("jump");
        }
        
    }




    void Start()
    {
        //hides cursor
        Cursor.lockState = CursorLockMode.Locked;
        //gets character controller
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        MovementLogic();

        if (CheckOnGround() && verticalSpeed.y < 0)
        {
            verticalSpeed.y = 0;
        }
        CheckGravity();
    }

    private void MovementLogic()
    {
        //reads camera to move and blocks movement
        movementDirection = cam.forward * inputDirection.z + cam.right * inputDirection.x;
        movementDirection.y = 0;

        //translates movement to place and animation
        controller.Move(movementDirection * movementSpeed * Time.deltaTime);
        anim.SetFloat("speed", controller.velocity.magnitude);

        //rotation logic
        if (movementDirection.sqrMagnitude > 0) 
        {
            RotateToDestination();
        }
    }

    private void RotateToDestination() 
    {
        //gets rotation angle for player
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = targetRotation;
    }

    private void CheckGravity() 
    {
        verticalSpeed.y += gravityFactor * Time.deltaTime;
        controller.Move(verticalSpeed * Time.deltaTime);
    }

    private bool CheckOnGround() 
    {  
        return Physics.CheckSphere(feetPosition.position, groundDetectionDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(feetPosition.position, groundDetectionDistance);
    }
}
