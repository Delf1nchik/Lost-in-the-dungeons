using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour
{
    public static Player2 instance { get; private set; }

    [Header("Movement")]
    [SerializeField] private float movingSpeed = 10f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 35f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Ghost Effect")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float ghostDelay = 0.03f;

    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 direction;
    private bool isInitialized = false;

    private bool canDash = true;
    private bool isDashing = false;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject); // Персонаж сохраняется между сценами
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
        GameInput2.instance.OnPlayerDash += GameInput_OnPlayerDash; // Подписка на рывок
        isInitialized = true;
    }

    private void Update()
    {
        if (isDashing) return;

        // Получаем ввод из GameInput2[cite: 2]
        Vector2 input = GameInput2.instance.GetMovementVector();

        // Запоминаем направление для рывка, если игрок движется
        if (input != Vector2.zero) direction = input;

        // Обновляем аниматор[cite: 3]
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
        animator.SetFloat("speed", input.sqrMagnitude);
    }

    // Тот самый метод, который потерялся:
    public Vector3 GetPlayerScreenPos()
    {
        return Camera.main.WorldToScreenPoint(transform.position); //[cite: 3]
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        // Вызов атаки активного оружия[cite: 3]
        if (ActiveGun.Instance != null)
        {
            ActiveGun.Instance.GetActiveGun()?.Attack();
        }
    }

    private void GameInput_OnPlayerDash(object sender, System.EventArgs e)
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;

        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        Vector2 dashDir = inputVector.normalized;

        // Если ввода нет, используем последнее направление
        if (dashDir == Vector2.zero) dashDir = direction.normalized;
        if (dashDir == Vector2.zero) dashDir = Vector2.right; // Дефолт

        rb.linearVelocity = dashDir * dashSpeed;

        // Эффект призраков
        float timer = 0;
        while (timer < dashDuration)
        {
            SpawnGhost();
            timer += ghostDelay;
            yield return new WaitForSeconds(ghostDelay);
        }

        yield return new WaitForSeconds(Mathf.Max(0, dashDuration - timer));

        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void SpawnGhost()
    {
        if (ghostPrefab != null)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            var ghostScript = ghost.GetComponent<DashGhost>();
            if (ghostScript != null)
            {
                ghostScript.Init(GetComponent<SpriteRenderer>().sprite, transform.position, transform.rotation, transform.localScale);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isInitialized || GameInput2.instance == null || isDashing) return;

        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        // Используем MovePosition для обычного движения[cite: 3]
        rb.MovePosition(rb.position + inputVector.normalized * (movingSpeed * Time.fixedDeltaTime));
    }

    private void OnDestroy()
    {
        if (GameInput2.instance != null)
        {
            GameInput2.instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
            GameInput2.instance.OnPlayerDash -= GameInput_OnPlayerDash;
        }
    }
}