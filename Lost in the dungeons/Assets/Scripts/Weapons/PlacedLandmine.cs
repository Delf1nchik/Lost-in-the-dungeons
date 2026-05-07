using UnityEngine;

public class PlacedLandmine : MonoBehaviour
{
    public int damage = 15;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Skeleton skeleton))
        {
            // ѕр€мое обращение к статической переменной класса Landmine
            Landmine.currentLandmines--;

            skeleton.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}