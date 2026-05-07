using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 0.5f;


    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Skeleton skeleton))
        {
            Destroy(gameObject);
            skeleton.TakeDamage(damage);
        }
        else if (collision.transform.TryGetComponent(out BossController boss))
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}