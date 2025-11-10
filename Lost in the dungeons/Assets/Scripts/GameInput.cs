using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput instance {  get; private set; }

    private PlayerInputActions _actions;

    private void Awake()
    {
        instance = this;

        _actions = new PlayerInputActions();
        _actions.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 InputVector = _actions.Player.Move.ReadValue<Vector2>();
        return InputVector;
    }
}
