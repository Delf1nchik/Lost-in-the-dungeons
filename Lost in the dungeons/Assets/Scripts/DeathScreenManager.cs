using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;                 // <-- ДОБАВЬ ЭТУ СТРОКУ

public class DeathScreenManager : MonoBehaviour
{
    public static DeathScreenManager Instance { get; private set; }

    [SerializeField] private GameObject deathPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Создаём EventSystem, если его нет (исправлен устаревший метод)
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            var eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
            eventSystemObj.transform.SetParent(transform);
        }

        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    // Вызывается каждый раз после загрузки новой сцены
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowDeathScreen()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            var eventSystem = EventSystem.current;
            if (eventSystem != null)
                eventSystem.enabled = true;

            // Фокусируем кнопку (опционально)
            var restartButton = deathPanel.transform.Find("ButtonRestart")?.GetComponent<Button>();
            if (restartButton != null)
                EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
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