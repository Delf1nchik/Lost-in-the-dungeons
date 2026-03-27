using UnityEngine;
using System;

public class GameInput2 : MonoBehaviour
{
    public static GameInput2 instance { get; private set; }

    private PlayerActions _actions;

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        // Реализация синглтона с сохранением между сценами
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // Теперь объект не уничтожается при загрузке новой сцены

        _actions = new PlayerActions();

        _actions.PLayer.Enable();
        _actions.Combat.Enable();

        _actions.Combat.Attack.started += Attack_started;

        Debug.Log("GameInput2 initialized"); // Для проверки, что объект создан
    }

    private void Attack_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        // Читаем вектор движения из Input Actions
        Vector2 inputVector = _actions.PLayer.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector2 GetMousePosition()
    {
        Vector2 mousePosition = _actions.PLayer.MousePosition.ReadValue<Vector2>();
        return mousePosition;
    }

    private void OnDisable()
    {
        if (_actions != null)
        {
            if (_actions.PLayer.enabled) _actions.PLayer.Disable();
            if (_actions.Combat.enabled) _actions.Combat.Disable();
        }
    }

    private void OnDestroy()
    {
        if (_actions != null)
        {
            _actions.Combat.Attack.started -= Attack_started;

            if (_actions.PLayer.enabled) _actions.PLayer.Disable();
            if (_actions.Combat.enabled) _actions.Combat.Disable();

            _actions.Dispose();
            _actions = null;
        }

        if (instance == this)
        {
            instance = null;
        }
    }
}