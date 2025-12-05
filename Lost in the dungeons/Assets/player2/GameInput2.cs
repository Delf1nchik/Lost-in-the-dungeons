using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GameInput2 : MonoBehaviour
{
    public static GameInput2 instance { get; private set; }

    private PlayerActions _actions;

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        instance = this;

        _actions = new PlayerActions();
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
        Vector2 InputVector = _actions.PLayer.Move.ReadValue<Vector2>();
        return InputVector;
    }

    public Vector2 GetMousePosition()
    {
        Vector2 MousePosition = _actions.PLayer.MousePosition.ReadValue<Vector2>();
        return MousePosition;
    }
}
