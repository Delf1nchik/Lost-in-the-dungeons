using UnityEngine;

public class NovaShard : MonoBehaviour
{
    [SerializeField] private string message = "Вы нашли Древнее Ядро! Нажмите [F], чтобы вызвать вспышку!";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player2 player = collision.GetComponent<Player2>();
            if (player != null)
            {
                player.UnlockNova();
                // Вызываем твой метод показа уведомления
                ShowNovaMessage();
                Destroy(gameObject);
            }
        }
    }

    // Копируем твою логику поиска Canvas из MemoryShard.cs
    void ShowNovaMessage()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform t = canvas.transform.Find("PickupNotification");
            if (t != null)
            {
                GameObject notification = t.gameObject;
                notification.SetActive(true);
                var text = notification.GetComponent<TMPro.TextMeshProUGUI>();
                if (text != null) text.text = message;

                // Используем твой MessageTimer
                var timer = notification.GetComponent<MessageTimer>() ?? notification.AddComponent<MessageTimer>();
                timer.StartTimer(3f);
            }
        }
    }
}