using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform camera; 
    private float inputH;
    private float inputV;

    private CharacterController controller;
    private Animator anim;



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
        //reads input
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        //reads camera to move and blocks movement
        Vector3 movementDirection = (camera.forward * inputV + camera.right * inputH).normalized;
        movementDirection.y = 0;

        //translates movement to place and animation
        controller.Move(movementDirection * movementSpeed * Time.deltaTime);
        anim.SetFloat("speed", controller.velocity.magnitude);

        if (inputH != 0 || inputV != 0) 
        {
            RotateToDestination(movementDirection);
        }
        
    }

    private void RotateToDestination(Vector3 destination) 
    {
        //gets rotation angle for player
        Quaternion targetRotation = Quaternion.LookRotation(destination);
        transform.rotation = targetRotation;
    }
}
