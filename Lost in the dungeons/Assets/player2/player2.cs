using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player2 : MonoBehaviour
{
    public static Player2 instance { get; private set; }

    [Header("Progression")]
    [SerializeField] public bool isDashUnlocked = false;

    [Header("Movement")]
    [SerializeField] private float movingSpeed = 10f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 35f;
    [SerializeField] private float dashDuration = 0.15f;
    public float dashCooldown = 1f; // Сделал public для UI
    public float dashTimer = 0f;    // Текущий таймер отката для UI

    [Header("Dash Combat")]
    [SerializeField] private float dashDamage = 30f;
    [SerializeField] private float dashRange = 1.5f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Ghost Effect")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float ghostDelay = 0.03f;

    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 direction;
    private Vector2 lastMoveDirection;
    private bool isInitialized = false;

    private bool canDash = true;
    private bool isDashing = false;

    private List<Collider2D> hitEnemiesDuringDash = new List<Collider2D>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        rb = GetComponent<Rigidbody2D>();
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
        GameInput2.instance.OnPlayerDash += GameInput_OnPlayerDash;
        GameInput2.instance.OnWeaponChange += GameInput_OnWeaponChange;
        ActiveGun.Instance.transform.GetChild(1).gameObject.SetActive(false);
        ActiveGun.Instance.transform.GetChild(2).gameObject.SetActive(false);
        isInitialized = true;
    }

    private void Update()
    {
        if (isDashing) return;

        // Таймер для UI
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }

        Vector2 input = GameInput2.instance.GetMovementVector();

        if (input != Vector2.zero)
        {
            direction = input;
            lastMoveDirection = input;
        }

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
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    // ВОТ ЭТОТ МЕТОД БЫЛ ПОТЕРЯН
    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (ActiveGun.Instance != null)
        {
            ActiveGun.Instance.GetActiveGun()?.Attack();
        }
    }

    private void GameInput_OnPlayerDash(object sender, System.EventArgs e)
    {
        if (isDashUnlocked && canDash && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }
        else if (!isDashUnlocked)
        {
            Debug.Log("Попытка рывка: способность еще не открыта!");
        }
    }
    private void GameInput_OnWeaponChange(object sender, System.EventArgs e)
    {
        ActiveGun.Instance.transform.GetChild(ActiveGun.CurrentWeaponIndex).gameObject.SetActive(false);
        ActiveGun.CurrentWeaponIndex = (ActiveGun.CurrentWeaponIndex + 1) % 3;
        ActiveGun.Instance.transform.GetChild(ActiveGun.CurrentWeaponIndex).gameObject.SetActive(true);
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;
        dashTimer = dashCooldown; // Ставим таймер на максимум
        hitEnemiesDuringDash.Clear();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        Vector2 dashDir = inputVector.normalized;

        if (dashDir == Vector2.zero) dashDir = lastMoveDirection.normalized;
        if (dashDir == Vector2.zero) dashDir = Vector2.right;

        rb.linearVelocity = dashDir * dashSpeed;

        float timer = 0;
        while (timer < dashDuration)
        {
            SpawnGhost();
            DealDashDamage();

            timer += ghostDelay;
            yield return new WaitForSeconds(ghostDelay);
        }

        yield return new WaitForSeconds(Mathf.Max(0, dashDuration - timer));

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitUntil(() => dashTimer <= 0);
        canDash = true;
    }

    private void DealDashDamage()
    {
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dashRange);
    }
}