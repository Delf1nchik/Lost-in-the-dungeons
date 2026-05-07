using UnityEngine;

public class PlacedLandmine : MonoBehaviour
{
    public int damage = 15;
    public AudioClip explosionSound; // Звук взрыва мины

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Skeleton skeleton))
        {
            // Воспроизводим звук взрыва в позиции мины
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            }

            Landmine.currentLandmines--;
            skeleton.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}