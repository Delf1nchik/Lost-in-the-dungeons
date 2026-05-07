using UnityEngine;
using TMPro;

public class MemoryShard : MonoBehaviour
{
    public enum ShardType { Dash, Hellfire }
    [Header("Настройки осколка")]
    [SerializeField] private ShardType shardType;
    [SerializeField] private string message = "Новая способность разблокирована!";

    private GameObject notificationObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player2 player = collision.GetComponent<Player2>();
            if (player != null)
            {
                // Логика разблокировки
                if (shardType == ShardType.Dash)
                {
                    player.UnlockDash(); // Включает DashIcon
                }
                else if (shardType == ShardType.Hellfire)
                {
                    player.UnlockHellfire(); // Включает HellfireIcon
                }

                ShowMessage();
                Destroy(gameObject);
            }
        }
    }

    void ShowMessage()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform t = canvas.transform.Find("PickupNotification");
            if (t != null) notificationObject = t.gameObject;
        }

        if (notificationObject != null)
        {
            notificationObject.SetActive(true);
            var textComponent = notificationObject.GetComponent<TextMeshProUGUI>();
            if (textComponent != null) textComponent.text = message;

            // Запуск таймера (MessageTimer должен быть на объекте PickupNotification)
            var timer = notificationObject.GetComponent<MessageTimer>();
            if (timer == null) timer = notificationObject.AddComponent<MessageTimer>();

            timer.StartTimer(3f); // Сообщение исчезнет через 3 секунды
        }
    }
}