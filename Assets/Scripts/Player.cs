using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform cam; 
    private Vector3 movementDirection;
    private Vector3 inputDirection; 


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
        RotateToDestination();
    }

    private void JumpAction()
    {
        Debug.Log("Player Jumps");
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
    }

    private void MovementLogic()
    {
        //reads camera to move and blocks movement
        movementDirection = cam.forward * inputDirection.z + cam.right * inputDirection.x;
        movementDirection.y = 0;

        //translates movement to place and animation
        controller.Move(movementDirection * movementSpeed * Time.deltaTime);
        anim.SetFloat("speed", controller.velocity.magnitude);
    }

    private void RotateToDestination() 
    {
        //gets rotation angle for player
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = targetRotation;
    }
}
