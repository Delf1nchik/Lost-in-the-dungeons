using UnityEngine;
using UnityEngine.UI;

public class Vampire : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float scaleX = 1.3f;
    [SerializeField] private float scaleY = 1.3f;
    public int EnemyHP = 100;
    public Animator animator;
    public Slider enemyHealthBar;

    public float damage = 10f;

    protected virtual void Start()
    {
        enemyHealthBar.value = EnemyHP;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        // Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    protected virtual void Update()
    {
        // Если цели нет, пробуем найти её снова
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            return; // Пропускаем кадр, если всё еще не нашли
        }

        HandleFlip();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerDamage();
        }
    }

    private void HandleFlip()
    {
        if (target.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(scaleX, scaleY);
        }
        else
        {
            transform.localScale = new Vector2(-scaleX, scaleY);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        EnemyHP -= damage;
        enemyHealthBar.value = EnemyHP;
        if (EnemyHP > 0)
        {
            animator.SetTrigger("Damage");
        }
        else
        {
            animator.SetTrigger("Death");
            GetComponent<CapsuleCollider2D>().enabled = false;
            this.enabled = false;

        }
    }
    public void PlayerDamage()
    {
        // Если по какой-то причине цель потеряна, ищем её по тегу заново
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }

        if (target != null)
        {
            // Ищем скрипт Health именно на том объекте, который сейчас в таргете
            Health playerHealth = target.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Урон нанесен игроку: " + damage);
            }
            else
            {
                Debug.LogError("Скрипт Health не найден на объекте Player!");
            }
        }
    }
}