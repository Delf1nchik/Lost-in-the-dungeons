using UnityEngine;

public class Player2 : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 10f;

    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject); // Персонаж тоже сохраняется
    }

    private void Start()
    {
        // Подписка на событие атаки с проверкой, чтобы не было ошибки, если GameInput2 ещё не создан
        if (GameInput2.instance != null)
        {
            GameInput2.instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        }
        else
        {
            Debug.LogWarning("GameInput2.instance is null at Start, attack won't work until it's created.");
        }
    }

    private void Update()
    {
        // Обработка анимаций (оставляем как было)
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("speed", direction.sqrMagnitude);
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        // Проверка на null для ActiveGun, если нужно
        if (ActiveGun.Instance != null && ActiveGun.Instance.GetActiveGun() != null)
        {
            ActiveGun.Instance.GetActiveGun().Shoot();
        }
        else
        {
            Debug.LogWarning("ActiveGun is not ready for shooting.");
        }
    }

    private void FixedUpdate()
    {
        // Защита от null: если GameInput2.instance отсутствует, не пытаемся двигаться
        if (GameInput2.instance == null)
        {
            Debug.LogError("GameInput2.instance is null in FixedUpdate! Movement disabled.");
            return;
        }

        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        // Если inputVector нулевой, движение не будет применено, но ошибки не будет
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
    }
}