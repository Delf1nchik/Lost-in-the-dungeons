using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput instance { get; private set; }

    private PlayerInputActions _actions;

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        instance = this;

        _actions = new PlayerInputActions();
        _actions.Enable();

        _actions.Combat.Attack.started += Attack_started;
    }

    private void Attack_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (OnPlayerAttack != null)
        {
            OnPlayerAttack.Invoke(this, EventArgs.Empty);
        }
    }

    public Vector2 GetMovementVector()
    {
        Vector2 InputVector = _actions.Player.Move.ReadValue<Vector2>();
        return InputVector;
    }

    public Vector2 GetMousePosition()
    {
        Vector2 MousePosition = _actions.Player.MousePosition.ReadValue<Vector2>();
        return MousePosition;
    }
}
