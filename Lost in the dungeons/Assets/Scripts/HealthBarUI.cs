using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider healthSlider;          // Ползунок здоровья
    public Image healthFill;             // Заполняемая часть
    public Text healthText;              // Текст здоровья (опционально)

    [Header("Colors")]
    public Color highHealthColor = Color.green;
    public Color mediumHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    private HealthSystem healthSystem;

    void Start()
    {
        // СНАЧАЛА принудительно ставим полное здоровье в UI
        if (healthFill != null)
            healthFill.color = highHealthColor;
        if (healthSlider != null)
            healthSlider.value = 1f;

        // ПОТОМ находим систему здоровья
        healthSystem = GetComponent<HealthSystem>();
        if (healthSystem == null)
            healthSystem = GetComponentInParent<HealthSystem>();

        if (healthSystem != null)
        {
            // Подписываемся на изменение здоровья
            healthSystem.OnHealthChanged += UpdateHealthBar;
            // Обновляем полоску
            UpdateHealthBar(healthSystem.GetCurrentHealth());
        }
        else
        {
            Debug.LogError("HealthSystem не найден!");
        }
    }

    void UpdateHealthBar(int currentHealth)
    {
        if (healthSlider != null && healthSystem != null)
        {
            // ВАЖНО: используем maxHealth из HealthSystem
            float healthPercent = (float)currentHealth / healthSystem.GetMaxHealth();
            healthSlider.value = healthPercent;

            Debug.Log($"UI: {currentHealth}/{healthSystem.GetMaxHealth()} = {healthPercent}");

            // Меняем цвет в зависимости от здоровья
            if (healthFill != null)
            {
                if (healthPercent > 0.6f)
                    healthFill.color = highHealthColor;
                else if (healthPercent > 0.3f)
                    healthFill.color = mediumHealthColor;
                else
                    healthFill.color = lowHealthColor;
            }
        }

        // Обновляем текст если есть
        if (healthText != null && healthSystem != null)
        {
            healthText.text = $"{currentHealth}/{healthSystem.GetMaxHealth()}";
        }

        // Дополнительная проверка: при полном здоровье всегда зеленый
        if (healthSystem != null && currentHealth == healthSystem.GetMaxHealth() && healthFill != null)
        {
            healthFill.color = highHealthColor;
        }
    }

    void OnDestroy()
    {
        // Отписываемся от события при уничтожении
        if (healthSystem != null)
            healthSystem.OnHealthChanged -= UpdateHealthBar;
    }
}