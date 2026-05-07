using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("��������������")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float chaseRange = 7f;   // ������ ����������� ������
    [SerializeField] private float attackRange = 1.5f; // ������ �����
    [SerializeField] private float attackCooldown = 2f; // ����������� �����

    private int currentHealth;
    private float nextAttackTime;
    private bool isDead = false;

    // ������ �� ����������
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        // ����� ������. ������������, ��� � ������ ���� ��� "Player" 
        // ��� ���������� ���� Singleton Player2.instance
        if (Player2.instance != null)
        {
            player = Player2.instance.transform;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 1. ������ ����������� � ������ ���������
        if (distanceToPlayer <= attackRange)
        {
            // ��������� � ���� � �������
            StopMoving();
            if (Time.time >= nextAttackTime)
            {
                Attack();
            }
        }
        else if (distanceToPlayer <= chaseRange)
        {
            // ����� ������, �� ������ � ����������
            Chase();
        }
        else
        {
            // ����� ������� ������ � �����
            StopMoving();
        }

        // 2. �������� �������� � �������� ��� �������� Idle <-> Run
        // ���������� magnitude (����� ������� ��������)
        anim.SetFloat("Speed", rb.linearVelocity.magnitude);
    }

    private void Chase()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        // ������� ������� �����/������ � ����������� �� �����������
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

        // ������ �������� ����� (��� � ����� ���������)
        anim.SetTrigger("Attack");

        // ����� ����� ������� ����� ��������� ����� ������
        // ��������: player.GetComponent<PlayerHealth>().TakeDamage(10);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        anim.SetTrigger("Damage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Death");

        rb.linearVelocity = Vector2.zero; // Останавливаем движение
        rb.simulated = false;       // Выключаем физику (он перестанет толкаться и получать урон)

        // Если нужно, чтобы он исчез через 2 секунды:
        Destroy(gameObject, 2f);
    }

    // ��������� �������� � ��������� (��� ��������)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}