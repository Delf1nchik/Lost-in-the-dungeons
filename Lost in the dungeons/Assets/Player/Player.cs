using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 10f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Добавляем проверку на существование instance
        if (GameInput.instance == null) return;

        Vector2 inputVector = GameInput.instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
    }

    private void OnDestroy()
    {
        // Не нужно ничего делать с GameInput здесь
        // GameInput сам управляет своим жизненным циклом
    }
}