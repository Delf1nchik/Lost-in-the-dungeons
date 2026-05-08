using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class FinalDoor : MonoBehaviour
{
    [Header("Настройки видео")]
    public VideoPlayer videoPlayer;
    public GameObject videoCanvas; // Сюда тащи объект Canvas или RawImage, где видео

    private bool isPlayerNearby = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void Update()
    {
        // Если игрок рядом и нажал E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            // Проверяем наличие осколка через твой Singleton-экземпляр Player2
            if (Player2.instance != null && Player2.instance.hasLastShard)
            {
                StartEndCutscene();
            }
            else
            {
                Debug.Log("Дверь заперта! Тебе нужен LASTSHARD.");
            }
        }
    }

    void StartEndCutscene()
    {
        // 1. Останавливаем игрока (используем твой флаг)
        Player2.instance.isDead = true;

        // 2. Запускаем видео
        videoPlayer.Play();

        // 3. Подписываемся на финал видео, чтобы выйти в меню
        videoPlayer.loopPointReached += (vp) => {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        };
    }
}