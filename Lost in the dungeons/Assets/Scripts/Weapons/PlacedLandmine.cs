using UnityEngine;

public class PlacedLandmine : MonoBehaviour
{
    public int damage = 15;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool hitTarget = false;

        if (collision.transform.TryGetComponent(out Skeleton skeleton))
        {
            skeleton.TakeDamage(damage);
            hitTarget = true;
        }
        // ДОБАВЛЯЕМ: Проверка на босса
        else if (collision.transform.TryGetComponent(out BossController boss))
        {
            boss.TakeDamage(damage);
            hitTarget = true;
        }

        if (hitTarget)
        {
            Destroy(gameObject);
            // Безопасный поиск объекта Landmine для уменьшения счетчика
            GameObject landmineObj = GameObject.Find("Landmine");
            if (landmineObj != null)
            {
                landmineObj.GetComponent<Landmine>().currentLandmines--;
            }
        }
    }
}
