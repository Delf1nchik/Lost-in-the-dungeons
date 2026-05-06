using UnityEngine;
using TMPro; // Не забудьте добавить этот неймспейс

public class MemoryShard : MonoBehaviour
{
    [SerializeField] private string message = "Вы подобрали осколок и разблокировали рывок!";
    private TextMeshProUGUI notificationText;

    private void Start()
    {
        // Ищем текст в Canvas по тегу или имени
        GameObject textObj = GameObject.Find("PickupNotification");
        if (textObj != null)
        {
            notificationText = textObj.GetComponent<TextMeshProUGUI>();
            notificationText.gameObject.SetActive(false); // Скрываем при старте
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player2 player = collision.GetComponent<Player2>();
            if (player != null)
            {
                player.UnlockDash();
                ShowMessage();
                Destroy(gameObject, 0.1f); // Небольшая задержка, чтобы успел сработать код
            }
        }
    }

    void ShowMessage()
    {
        if (notificationText != null)
        {
            notificationText.text = message;
            notificationText.gameObject.SetActive(true);
            // Скрыть сообщение через 3 секунды
            Invoke("HideMessage", 3f);
        }
    }

    void HideMessage()
    {
        notificationText.gameObject.SetActive(false);
    }
}