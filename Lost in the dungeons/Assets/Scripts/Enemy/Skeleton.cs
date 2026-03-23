using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float scaleX = 1.3f;
    [SerializeField] private float scaleY = 1.3f;
    public int EnemyHP = 100;
    public Animator animator;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    void Update()
    {
        if (target == null) return;

        // Поворот спрайта
        HandleFlip();
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

    public void TakeDamage(int damage)
    {
        EnemyHP -= damage;
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
}