using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set;}

    private PlayerInputActions inputActions;
    public event EventHandler OnAttack;
    public event EventHandler OnDash;

    private void Awake() {
        Instance = this;
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += PlayerAttackPerformed;
        inputActions.Player.Dash.performed += PlayerDashPerformed;
    }

    private void PlayerDashPerformed(InputAction.CallbackContext context)
    {
        OnDash?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerAttackPerformed(InputAction.CallbackContext context)
    {
        OnAttack?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector() {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    } 
}
