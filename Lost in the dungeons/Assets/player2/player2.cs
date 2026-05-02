using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player2 : MonoBehaviour
{
    public static Player2 instance { get; private set; }

    [Header("Progression")]
    [SerializeField] private bool isDashUnlocked = false; 

    [Header("Movement")]
    [SerializeField] private float movingSpeed = 10f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 35f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Dash Combat")]
    [SerializeField] private float dashDamage = 30f;
    [SerializeField] private float dashRange = 1.5f; // Радиус поражения
    [SerializeField] private LayerMask enemyLayer;   // Слой врагов

    [Header("Ghost Effect")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float ghostDelay = 0.03f;

    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 direction;
    private Vector2 lastMoveDirection; // Для рывка, если стоим на месте
    private bool isInitialized = false;

    private bool canDash = true;
    private bool isDashing = false;

    // Список, чтобы не бить одного и того же врага дважды за один рывок
    private List<Collider2D> hitEnemiesDuringDash = new List<Collider2D>();

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject); // Сохранение персонажа между сценами[cite: 3]
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
        GameInput2.instance.OnPlayerDash += GameInput_OnPlayerDash; // Подписка на событие рывка[cite: 3]
        isInitialized = true;
    }

    private void Update()
    {
        if (isDashing) return;

        // Получаем ввод из GameInput2[cite: 2]
        Vector2 input = GameInput2.instance.GetMovementVector();

        if (input != Vector2.zero)
        {
            direction = input;
            lastMoveDirection = input; // Запоминаем для рывка
        }

        // Обновление анимаций[cite: 3]
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
        animator.SetFloat("speed", input.sqrMagnitude);
    }
    public void UnlockDash()
    {
        isDashUnlocked = true;
        Debug.Log("Воспоминание восстановлено: Рывок разблокирован!");
    }

    public Vector3 GetPlayerScreenPos()
    {
        return Camera.main.WorldToScreenPoint(transform.position); // Для работы ActiveGun[cite: 3]
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (ActiveGun.Instance != null)
        {
            ActiveGun.Instance.GetActiveGun()?.Attack(); // Логика атаки из оригинала[cite: 3]
        }
    }

    private void GameInput_OnPlayerDash(object sender, System.EventArgs e)
    {
        if (isDashUnlocked && canDash && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }

        if (canDash && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;
        hitEnemiesDuringDash.Clear();

        // Отключаем столкновения со слоем Enemy на время рывка
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        Vector2 dashDir = inputVector.normalized;

        // Если кнопки не нажаты, используем последнее направление движения
        if (dashDir == Vector2.zero) dashDir = lastMoveDirection.normalized;
        if (dashDir == Vector2.zero) dashDir = Vector2.right; // Запасной вариант

        rb.linearVelocity = dashDir * dashSpeed;

        float timer = 0;
        while (timer < dashDuration)
        {
            SpawnGhost(); // Эффект шлейфа[cite: 3]
            DealDashDamage(); // Нанесение урона

            timer += ghostDelay;
            yield return new WaitForSeconds(ghostDelay);
        }

        yield return new WaitForSeconds(Mathf.Max(0, dashDuration - timer));

        // Включаем столкновения обратно
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void DealDashDamage()
    {
        // Поиск врагов в радиусе рывка
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, dashRange, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            if (!hitEnemiesDuringDash.Contains(enemy))
            {
                Skeleton skeleton = enemy.GetComponent<Skeleton>();
                if (skeleton != null)
                {
                    skeleton.TakeDamage((int)dashDamage);
                    Debug.Log("Рывок нанес урон: " + enemy.name);
                }

                hitEnemiesDuringDash.Add(enemy);
            }
        }
    }

    private void SpawnGhost()
    {
        if (ghostPrefab != null)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            DashGhost ghostScript = ghost.GetComponent<DashGhost>();
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
        // Обычное перемещение через MovePosition[cite: 3]
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

    // Отрисовка радиуса поражения в редакторе (Gizmos)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dashRange);
    }
}