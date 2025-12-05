using UnityEngine;

public class GameInput2 : MonoBehaviour
{
    public static GameInput2 instance { get; private set; }

    private PlayerActions _actions;

    private void Awake()
    {
        instance = this;

        _actions = new PlayerActions();
        _actions.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 InputVector = _actions.PLayer.Move.ReadValue<Vector2>();
        return InputVector;
    }
}
