using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public static DeathScreenManager Instance { get; private set; }

    [SerializeField] private GameObject deathScreenPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Awake()
    {
        // Превращаем в синглтон и не даём уничтожиться при загрузке новых сцен
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Убедимся, что панель выключена на старте
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(false);
    }

    void Start()
    {
        // Восстанавливаем время (на случай, если при перезапуске сцены timeScale остался 0)
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowDeathScreen()
    {
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(true);
            Time.timeScale = 0f;          // ставим игру на паузу
            Cursor.visible = true;        // показываем курсор для кнопок
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Методы для кнопок (остаются без изменений)
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}