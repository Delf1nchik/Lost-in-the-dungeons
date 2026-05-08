using UnityEngine;

public class PlacedLandmine : MonoBehaviour
{
    public int damage = 15;
    public AudioClip explosionSound;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool hitTarget = false;
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        
        if (collision.transform.TryGetComponent(out Skeleton skeleton))
        {
            skeleton.TakeDamage(damage);
            hitTarget = true;
        }
        // ƒќЅј¬Ћя≈ћ: ѕроверка на босса
        else if (collision.transform.TryGetComponent(out BossController boss))
        {
            boss.TakeDamage(damage);
            hitTarget = true;
        }

        if (hitTarget)
        {
            Landmine.currentLandmines--;

            Destroy(gameObject);
        }
    }
}
