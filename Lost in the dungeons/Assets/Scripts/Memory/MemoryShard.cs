using UnityEngine;

public class MemoryShard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что предмет подобрал именно игрок
        if (collision.CompareTag("Player"))
        {
            Player2 player = collision.GetComponent<Player2>();
            if (player != null)
            {
                player.UnlockDash(); // Разблокируем способность[cite: 3]

                // Здесь можно добавить эффект вспышки или звука
                Destroy(gameObject); // Удаляем осколок с карты
            }
        }
    }
}