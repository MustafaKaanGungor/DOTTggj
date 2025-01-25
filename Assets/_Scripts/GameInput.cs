using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set;}

    private PlayerInputActions inputActions;
    public event EventHandler OnDash;
    private bool isAttacking = false;

    private void Awake() {
        Instance = this;
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += context => isAttacking = true;
        inputActions.Player.Attack.canceled += context => isAttacking = false;
        inputActions.Player.Dash.performed += PlayerDashPerformed;
    }

    private void PlayerDashPerformed(InputAction.CallbackContext context)
    {
        OnDash?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector() {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    } 

    public bool IsAttacking() {
        return isAttacking;
    }
}
