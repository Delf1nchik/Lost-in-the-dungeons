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
        _actions.Enable();

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
}