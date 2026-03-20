using UnityEngine;
using System;

public class GameInput2 : MonoBehaviour
{
    public static GameInput2 instance { get; private set; }

    private PlayerActions _actions;

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        // Проверка на дубликат
        if (instance != null && instance != this)
        {
            Debug.Log("Уничтожаем дубликат GameInput2");
            Destroy(gameObject);
            return;
        }

        instance = this;

        _actions = new PlayerActions();

        // Включаем нужные action maps
        _actions.PLayer.Enable();
        _actions.Combat.Enable();

        // Подписываемся на событие атаки
        _actions.Combat.Attack.started += Attack_started;

        Debug.Log($"GameInput2 создан на сцене: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
    }

    private void Attack_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        if (_actions == null) return Vector2.zero;
        Vector2 InputVector = _actions.PLayer.Move.ReadValue<Vector2>();
        return InputVector;
    }

    public Vector2 GetMousePosition()
    {
        if (_actions == null) return Vector2.zero;
        Vector2 MousePosition = _actions.PLayer.MousePosition.ReadValue<Vector2>();
        return MousePosition;
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
        Debug.Log($"GameInput2 уничтожается");

        if (_actions != null)
        {
            // Отписываемся от событий
            _actions.Combat.Attack.started -= Attack_started;

            // Отключаем action maps
            if (_actions.PLayer.enabled) _actions.PLayer.Disable();
            if (_actions.Combat.enabled) _actions.Combat.Disable();

            // Очищаем ресурсы
            _actions.Dispose();
            _actions = null;
        }

        if (instance == this)
        {
            instance = null;
        }
    }
}