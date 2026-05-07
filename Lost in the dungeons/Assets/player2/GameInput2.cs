using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput2 : MonoBehaviour
{
    public static GameInput2 instance { get; private set; }

    private @PlayerActions _actions;

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerDash; // Событие для рывка
    public event EventHandler OnWeaponChange;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        _actions = new @PlayerActions();

        _actions.PLayer.Enable();
        _actions.Combat.Enable();

        // Подписки на события
        _actions.Combat.Attack.started += Attack_started;
        _actions.PLayer.Dash.performed += Dash_performed; // Dash должен быть создан в Input Actions
        _actions.Combat.ChangeWeapon.started += ChangeWeapon_started;

        Debug.Log("GameInput2 initialized with Dash support");
    }
    private void ChangeWeapon_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (OnWeaponChange != null)
        {
            OnWeaponChange.Invoke(this, EventArgs.Empty);
        }
    }
    private void Attack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        return _actions.PLayer.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMousePosition()
    {
        return _actions.PLayer.MousePosition.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        if (_actions != null)
        {
            _actions.PLayer.Disable();
            _actions.Combat.Disable();
        }
    }

    private void OnDestroy()
    {
        if (_actions != null)
        {
            _actions.Combat.Attack.started -= Attack_started;
            _actions.PLayer.Dash.performed -= Dash_performed;

            _actions.Dispose();
            _actions = null;
        }

        if (instance == this) instance = null;
    }
}