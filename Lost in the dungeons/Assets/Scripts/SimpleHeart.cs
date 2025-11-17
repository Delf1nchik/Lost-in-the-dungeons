using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleHeart : MonoBehaviour
{
    [Header("Heart Reference")]
    public Image heartImage;

    [Header("Heart Settings")]
    public Color fullHealthColor = new Color(1f, 0.2f, 0.2f); // Ярко-красный
    public Color lowHealthColor = new Color(0.1f, 0f, 0f);     // Почти черный
    public float maxPulseSpeed = 3f;                           // Макс скорость при низком здоровье
    public float minPulseSpeed = 0.5f;                         // Мин скорость при полном здоровье
    public float maxPulseIntensity = 0.3f;                     // Макс интенсивность пульса
    public float minPulseIntensity = 0.1f;                     // Мин интенсивность пульса

    private HealthSystem healthSystem;
    private Vector3 originalScale;
    private float currentHealthPercent = 1f;

    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged += OnHealthChanged;

            // Сохраняем оригинальный размер
            originalScale = heartImage.rectTransform.localScale;

            // Устанавливаем НАЧАЛЬНЫЙ цвет как при полном здоровье
            currentHealthPercent = 1f; // 100% здоровья
            heartImage.color = fullHealthColor;
            heartImage.rectTransform.localScale = originalScale;

            Debug.Log("❤️ Сердце инициализировано с полным здоровьем");
        }
    }

    void OnHealthChanged(int currentHealth)
    {
        UpdateHeartColor(currentHealth);
    }

    void UpdateHeartColor(int currentHealth)
    {
        if (healthSystem == null) return;

        // Используем плавное здоровье
        float smoothHealth = healthSystem.GetDisplayHealth();
        currentHealthPercent = smoothHealth / healthSystem.GetMaxHealth();

        // Плавное изменение цвета от ярко-красного к почти черному
        Color targetColor = Color.Lerp(lowHealthColor, fullHealthColor, currentHealthPercent);
        heartImage.color = targetColor;

        Debug.Log($"❤️ Здоровье: {currentHealthPercent:P0} - Цвет: {targetColor}");
    }

    void Update()
    {
        if (healthSystem == null) return;

        // Пульсация в зависимости от здоровья
        float pulseSpeed = Mathf.Lerp(maxPulseSpeed, minPulseSpeed, currentHealthPercent);
        float pulseIntensity = Mathf.Lerp(maxPulseIntensity, minPulseIntensity, currentHealthPercent);

        float pulse = Mathf.PingPong(Time.time * pulseSpeed, pulseIntensity) + (1f - pulseIntensity / 2f);
        heartImage.rectTransform.localScale = originalScale * pulse;
    }

    void OnDestroy()
    {
        if (healthSystem != null)
            healthSystem.OnHealthChanged -= OnHealthChanged;
    }
}