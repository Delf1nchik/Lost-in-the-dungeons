using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput instance { get; private set; }

    private PlayerInputActions _actions;

    private void Awake()
    {
        // ”ничтожаем старый экземпл€р, если он есть
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }

        instance = this;

        _actions = new PlayerInputActions();
        _actions.Enable();
    }

    private void OnDestroy()
    {
        if (_actions != null)
        {
            // ¬ажно: сначала Disable, потом Dispose
            _actions.Player.Disable();
            _actions.Dispose();
            _actions = null;
        }

        if (instance == this)
        {
            instance = null;
        }
    }

    public Vector2 GetMovementVector()
    {
        if (_actions == null) return Vector2.zero;
        Vector2 InputVector = _actions.Player.Move.ReadValue<Vector2>();
        return InputVector;
    }
}