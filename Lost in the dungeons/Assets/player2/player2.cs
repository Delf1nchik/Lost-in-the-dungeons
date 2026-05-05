using UnityEngine;

public class Player2 : MonoBehaviour
{
    public static Player2 instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;

    private Rigidbody2D rb;
    public Animator animator;
    private Collider2D playerCollider; // Добавляем ссылку на коллайдер
    private Vector2 direction;
    private bool isInitialized = false;

    public bool isDead = false; // Флаг состояния смерти

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>(); // Получаем коллайдер
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (GameInput2.instance == null)
        {
            Debug.LogError("GameInput2 не найден!");
            enabled = false;
            return;
        }

        GameInput2.instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        isInitialized = true;
    }

    private void Update()
    {
        // Если персонаж мертв, прерываем выполнение Update (не даем сменить анимацию на бег)
        if (isDead) return;

        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("speed", direction.sqrMagnitude);
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        // Запрещаем атаковать мертвым
        if (isDead) return;

        if (ActiveGun.Instance != null)
        {
            ActiveGun.Instance.GetActiveGun()?.Attack();
        }
    }

    public Vector3 GetPlayerScreenPos()
    {
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPos;
    }

    private void FixedUpdate()
    {
        // Если мертв — не двигаемся
        if (isDead) return;

        if (!isInitialized || GameInput2.instance == null)
        {
            Debug.LogError("GameInput2.instance is null in FixedUpdate! Movement disabled.");
            return;
        }

        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
    }

    // Вызывайте этот метод из вашего скрипта здоровья, когда HP падает до 0
    public void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        // Меняем слой, чтобы системы поиска или триггеры врагов его не видели
        gameObject.layer = LayerMask.NameToLayer("DeadBody");

        if (playerCollider != null) playerCollider.enabled = false;
        animator.SetTrigger("Die");
    }

    private void OnDestroy()
    {
        if (GameInput2.instance != null)
        {
            GameInput2.instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
        }
    }
}