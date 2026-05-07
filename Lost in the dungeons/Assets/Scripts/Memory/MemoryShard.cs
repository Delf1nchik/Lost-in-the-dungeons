using UnityEngine;
using TMPro;

public class MemoryShard : MonoBehaviour
{
    // Убираем [SerializeField] у notificationObject, так как будем искать его кодом
    private GameObject notificationObject;
    [SerializeField] private string message = "Вы подобрали осколок и разблокировали рывок!";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player2 player = collision.GetComponent<Player2>();
            if (player != null)
            {
                player.UnlockDash();
                ShowMessage();
                Destroy(gameObject);
            }
        }
    }

    void ShowMessage()
    {
        // 1. Ищем Canvas
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            // 2. Ищем PickupNotification внутри Canvas (даже если он выключен)
            Transform t = canvas.transform.Find("PickupNotification");
            if (t != null)
            {
                notificationObject = t.gameObject;
            }
        }

        if (notificationObject != null)
        {
            notificationObject.SetActive(true);

            var textComponent = notificationObject.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = message;
            }

            // Запуск таймера
            var timer = notificationObject.GetComponent<MessageTimer>();
            if (timer == null) timer = notificationObject.AddComponent<MessageTimer>();
            timer.StartTimer(3f);
        }
        else
        {
            Debug.LogError("ОШИБКА: Не нашел объект PickupNotification внутри Canvas! Проверь имя в Hierarchy.");
        }
    }
}