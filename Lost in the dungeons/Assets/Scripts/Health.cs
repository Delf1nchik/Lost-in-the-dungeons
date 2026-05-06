using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public Image healthBar;
    public float Maxhealth = 100f;
    public float HP;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        HP = Maxhealth;
        FindHealthBar();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindHealthBar();
    }

    void FindHealthBar()
    {
        // Ищет любой Image на сцене, который висит на объекте с именем HealthBar
        Image[] allImages = Resources.FindObjectsOfTypeAll<Image>();
        foreach (Image img in allImages)
        {
            if (img.gameObject.name == "HealthBar" && img.gameObject.activeInHierarchy)
            {
                healthBar = img;
                break;
            }
        }
        UpdateBar();
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP < 0) HP = 0;

        // Если вдруг связь потерялась, пробуем найти полоску еще раз прямо в момент урона
        if (healthBar == null) FindHealthBar();

        UpdateBar();
        Debug.Log("Игрок получил урон! Осталось: " + HP);
    }

    void UpdateBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = HP / Maxhealth;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}