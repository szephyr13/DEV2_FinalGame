using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private UIGameManager uiGameManager;
    [SerializeField] private GameObject interactionKeys;
    private CharacterController controller;
    private Animator anim;
    private bool isPaused;
    private bool hasKey;


    //colliding variables
    private bool nearLockedDoor;
    private bool nearUnlockableDoor;
    private bool nearUnlockedDoor;
    private Door nearDoor;
    private bool showingDoorInfo;
    private bool nearKey;
    private Collider key;
    private bool takingKey;



    public bool IsPaused { get => isPaused; set => isPaused = value; }
    public bool HasKey { get => hasKey; set => hasKey = value; }
    public Door NearDoor { get => nearDoor; set => nearDoor = value; }
    public bool NearLockedDoor { get => nearLockedDoor; set => nearLockedDoor = value; }
    public bool NearUnlockableDoor { get => nearUnlockableDoor; set => nearUnlockableDoor = value; }
    public bool NearUnlockedDoor { get => nearUnlockedDoor; set => nearUnlockedDoor = value; }

    void OnEnable()
    {
        inputManager.OnJumping += JumpAction;
        inputManager.OnMoving += MoveAction;
        inputManager.OnInteracting += InteractAction;
        inputManager.OnPausing += PauseAction;
    }

    private void PauseAction()
    {
        if (!isPaused) 
        {
            uiGameManager.PauseMenu();
        } else 
        {
            uiGameManager.Unpause();
        }
        
    }

    private void InteractAction()
    {
        uiGameManager.Interaction();
        if (!showingDoorInfo)
        {
            if(NearUnlockedDoor)
            {
                NearDoor.OpenDoor();
                nearUnlockedDoor = false;
                nearDoor = null;
                interactionKeys.SetActive(false);
            }
            else if (NearLockedDoor)
            {
                NearDoor.DoesntOpen();
                showingDoorInfo = true;
                interactionKeys.SetActive(false);
            }
            else if (NearUnlockableDoor && !hasKey)
            {
                NearDoor.NeedsKey();
                showingDoorInfo = true;
                interactionKeys.SetActive(false);
            }
            else if (NearUnlockableDoor && HasKey)
            {
                NearDoor.OpenDoor();
                nearUnlockableDoor = false;
                nearDoor = null;
                interactionKeys.SetActive(false);
            }
        } else 
        {
            if (NearLockedDoor)
            {
                NearDoor.gameObject.GetComponent<TextManager>().DisplayNextSentence();
                showingDoorInfo = false;
                interactionKeys.SetActive(false);
            }
            else if (NearUnlockableDoor)
            {
                NearDoor.gameObject.GetComponent<TextManager>().DisplayNextSentence();
                showingDoorInfo = false;
                interactionKeys.SetActive(false);
            }
        }
        if(nearKey && !takingKey)
        {
            key.gameObject.GetComponent<TextManager>().StartDialogue();
            takingKey = true;
            interactionKeys.SetActive(false);
        }
        else if (nearKey && takingKey)
        {
            key.gameObject.GetComponent<TextManager>().DisplayNextSentence();
            interactionKeys.SetActive(false);
        }
        
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
        HasKey = false;
        showingDoorInfo = false;
    }


    void Update()
    {
        if (isPaused)
        {
            anim.SetFloat("speed", 0);
        } 
        else 
        {
        MovementLogic();

        if (CheckOnGround() && verticalSpeed.y < 0)
        {
            verticalSpeed.y = 0;
        }
        CheckGravity();
        }

        
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("UnlockedDoor"))
        {
            interactionKeys.SetActive(true);
            AudioManager.instance.PlaySFX("Hover");
            NearUnlockedDoor = true;
            NearDoor = other.gameObject.GetComponent<Door>();
        }
        else if (other.CompareTag("UnlockableDoor"))
        {
            interactionKeys.SetActive(true);
            AudioManager.instance.PlaySFX("Hover");
            NearUnlockableDoor = true;
            NearDoor = other.gameObject.GetComponent<Door>();
        }
        else if (other.CompareTag("LockedDoor"))
        {
            interactionKeys.SetActive(true);
            AudioManager.instance.PlaySFX("Hover");
            NearLockedDoor = true;
            NearDoor = other.gameObject.GetComponent<Door>();
        }
        else if(other.CompareTag("Key"))
        {
            interactionKeys.SetActive(true);
            AudioManager.instance.PlaySFX("Hover");
            nearKey = true;
            key = other;
        }

        if (other.CompareTag("Victory"))
        {
            uiGameManager.YouWon();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("UnlockedDoor"))
        {
            interactionKeys.SetActive(false);
            NearUnlockedDoor = false;
            NearDoor = null;
        }
        else if(other.CompareTag("UnlockableDoor"))
        {
            interactionKeys.SetActive(false);
            NearUnlockableDoor = false;
            NearDoor = null;
        }
        else if(other.CompareTag("LockedDoor"))
        {
            interactionKeys.SetActive(false);
            NearLockedDoor = false;
            NearDoor = null;
        }
        else if(other.CompareTag("Key"))
        {
            interactionKeys.SetActive(false);
            nearKey = false;
            key = null;
        }
    }
}
