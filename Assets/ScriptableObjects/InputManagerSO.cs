using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;


[CreateAssetMenu(menuName = "InputManager")]
public class InputManagerSO : ScriptableObject
{
    Controls myControls;
    public event Action OnJumping;
    public event Action<Vector2> OnMoving;
    public event Action OnInteracting;
    void OnEnable()
    {
        myControls = new Controls();
        myControls.Player.Enable();
        myControls.Player.Jump.started += JumpAction;
        myControls.Player.Move.performed += MoveAction;
        myControls.Player.Move.canceled += MoveAction;
        myControls.Player.Interact.started += InteractAction;
    }

    private void InteractAction(InputAction.CallbackContext ctx)
    {
        OnInteracting?.Invoke();
    }

    private void JumpAction(InputAction.CallbackContext ctx) 
    {
        OnJumping?.Invoke();
    }

    private void MoveAction(InputAction.CallbackContext ctx) 
    {
        OnMoving?.Invoke(ctx.ReadValue<Vector2>());
    }


}
