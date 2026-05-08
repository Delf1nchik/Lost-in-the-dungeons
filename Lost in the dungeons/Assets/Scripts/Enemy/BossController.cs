using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
<<<<<<< HEAD
    [Header("Настройки")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float chaseRange = 7f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Дроп после смерти")]
    [SerializeField] private GameObject shardPrefab; // Перетащи сюда префаб осколка в инспекторе
=======
    [Header("Настройки Босса")]
    [SerializeField] private int maxHealth = 250;
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float abilityCooldown = 5f;
    [SerializeField] private GameObject memoryShardPrefab;

    [Header("Настройки Адского Пламени")]
    [SerializeField] private GameObject fireProjectilePrefab; // Префаб огненного шара
    [SerializeField] private int projectileCount = 10;        // Сколько шаров вылетит по кругу
    [SerializeField] private float projectileSpeed = 7f;      // Скорость шаров
>>>>>>> Boss

    private int currentHealth;
    private bool isEnraged = false;
    private bool isDead = false;
    private bool hasDroppedShard = false;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

<<<<<<< HEAD
        if (Player2.instance != null)
=======
        // Поиск игрока 
        if (Player2.instance != null) player = Player2.instance.transform;
        else
>>>>>>> Boss
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        StartCoroutine(BossLogicRoutine());
    }

    void Update()
    {
        if (isDead || player == null) return;

        // Поворот (Flip) - если смотрит не туда, поменяй 1 и -1 местами
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

<<<<<<< HEAD
        if (distanceToPlayer <= attackRange)
        {
            StopMoving();
            if (Time.time >= nextAttackTime)
=======
        // Управление движением и параметром IsChasing
        float distance = Vector2.Distance(transform.position, player.position);

        // Босс бежит, только если не атакует прямо сейчас
        if (!animator.GetBool("IsAttacking"))
        {
            if (distance > 1.5f)
>>>>>>> Boss
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;
                animator.SetBool("IsChasing", true); // Включаем твой bool бега
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                animator.SetBool("IsChasing", false); // Выключаем твой bool бега
            }
        }
    }

    private IEnumerator BossLogicRoutine()
    {
        while (currentHealth > 0)
        {
<<<<<<< HEAD
            Chase();
=======
            yield return new WaitForSeconds(abilityCooldown);

            if (currentHealth > 0 && !isDead)
            {
                // Запускаем способность
                StartCoroutine(ExecuteHellfire());
            }

            // Фаза 2: Гнев
            if (currentHealth < maxHealth / 2 && !isEnraged)
            {
                EnterEnrageMode();
            }
>>>>>>> Boss
        }
    }

    private IEnumerator ExecuteHellfire()
    {
        // 1. Включаем твой bool атаки (IsAttacking), чтобы босс перешел в анимацию
        animator.SetBool("IsAttacking", true);
        rb.linearVelocity = Vector2.zero; // Босс останавливается для каста

        // Ждем 0.2 секунды, чтобы анимация замаха успела начаться
        yield return new WaitForSeconds(0.2f);

        // 2. Выпускаем снаряды по кругу
        float angleStep = 360f / projectileCount;
        float angle = 0f;

        for (int i = 0; i < projectileCount; i++)
        {
<<<<<<< HEAD
            StopMoving();
        }

        anim.SetFloat("Speed", rb.linearVelocity.magnitude);
=======
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 direction = new Vector2(x, y);

            GameObject fire = Instantiate(fireProjectilePrefab, transform.position, Quaternion.identity);

            if (fire.TryGetComponent(out Rigidbody2D projRb))
            {
                projRb.linearVelocity = direction * projectileSpeed;
            }

            float rotZ = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            fire.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            angle += angleStep;
        }

        // 3. Ждем еще 0.8 секунды, чтобы босс "постоял" в анимации атаки
        yield return new WaitForSeconds(0.8f);

        // 4. Выключаем bool атаки, чтобы босс вернулся к преследованию
        animator.SetBool("IsAttacking", false);
>>>>>>> Boss
    }

    private void EnterEnrageMode()
    {
<<<<<<< HEAD
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void Attack()
    {
        nextAttackTime = Time.time + attackCooldown;
        anim.SetTrigger("Attack");
=======
        isEnraged = true;
        speed *= 1.3f;
        abilityCooldown *= 0.7f;
        GetComponent<SpriteRenderer>().color = Color.red;
>>>>>>> Boss
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("Damage"); // Твой триггер урона

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return; // Защита от двойного вызова
        isDead = true;
<<<<<<< HEAD

        anim.SetTrigger("Death");

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // --- НОВАЯ ЛОГИКА: СПАВН ОСКОЛКА ---
        SpawnShard();

        Destroy(gameObject, 2f);
    }

    private void SpawnShard()
    {
        if (shardPrefab != null)
        {
            // Создаем осколок в позиции босса
            // Quaternion.identity означает "без вращения"
            Instantiate(shardPrefab, transform.position, Quaternion.identity);
            Debug.Log("Босс побежден! Осколок выпал.");
        }
        else
        {
            Debug.LogWarning("Префаб осколка не назначен в BossController!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
=======
        animator.SetTrigger("Death"); // Твой триггер смерти
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        if (!hasDroppedShard && memoryShardPrefab != null)
        {
            Instantiate(memoryShardPrefab, transform.position, Quaternion.identity);
            hasDroppedShard = true;
        }

        Destroy(gameObject, 2f);
    }
>>>>>>> Boss
}