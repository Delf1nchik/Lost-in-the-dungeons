using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleFancyHealthUI : MonoBehaviour
{
    [Header("References")]
    public Slider healthSlider;
    public Image healthFill;
    public Image glowEffect;
    public Image border;

    [Header("Colors")]
    public Color highHealthColor = new Color(0.2f, 0.8f, 0.2f);    // Зеленый
    public Color mediumHealthColor = new Color(0.8f, 0.8f, 0.2f); // Желтый
    public Color lowHealthColor = new Color(0.8f, 0.2f, 0.2f);    // Красный

    [Header("Effects")]
    public float glowDuration = 0.5f;
    public float shakeIntensity = 3f;

    private HealthSystem healthSystem;
    private int lastHealth;
    private Vector3 originalBorderPosition;

    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged += OnHealthChanged;
            lastHealth = healthSystem.GetCurrentHealth();

            // Сохраняем оригинальную позицию рамки
            if (border != null)
                originalBorderPosition = border.rectTransform.localPosition;

            // Скрываем свечение при старте
            if (glowEffect != null)
                glowEffect.color = new Color(1, 1, 1, 0);

            // Устанавливаем начальный цвет
            UpdateHealthColor(healthSystem.GetCurrentHealth());
        }
    }

    void OnHealthChanged(int newHealth)
    {
        // Эффект при лечении
        if (newHealth > lastHealth && glowEffect != null)
        {
            StartCoroutine(GlowAnimation(Color.green));
        }
        // Эффект при получении урона
        else if (newHealth < lastHealth)
        {
            if (border != null)
                StartCoroutine(ShakeAnimation());

            // Свечение при критическом здоровье
            if (newHealth <= healthSystem.GetMaxHealth() * 0.3f)
            {
                StartCoroutine(GlowAnimation(Color.red));
            }
        }

        lastHealth = newHealth;
        UpdateHealthColor(newHealth);
    }

    void UpdateHealthColor(int currentHealth)
    {
        if (healthFill == null || healthSystem == null) return;

        float healthPercent = (float)currentHealth / healthSystem.GetMaxHealth();

        // Меняем цвет заполнения
        if (healthPercent > 0.6f)
            healthFill.color = highHealthColor;
        else if (healthPercent > 0.3f)
            healthFill.color = mediumHealthColor;
        else
            healthFill.color = lowHealthColor;
    }

    IEnumerator GlowAnimation(Color glowColor)
    {
        if (glowEffect == null) yield break;

        float timer = 0f;

        // Появление свечения
        while (timer < glowDuration / 2)
        {
            timer += Time.deltaTime;
            float alpha = timer / (glowDuration / 2);
            glowEffect.color = new Color(glowColor.r, glowColor.g, glowColor.b, alpha);
            yield return null;
        }

        // Исчезновение свечения
        timer = 0f;
        while (timer < glowDuration / 2)
        {
            timer += Time.deltaTime;
            float alpha = 1 - (timer / (glowDuration / 2));
            glowEffect.color = new Color(glowColor.r, glowColor.g, glowColor.b, alpha);
            yield return null;
        }

        glowEffect.color = new Color(1, 1, 1, 0);
    }

    IEnumerator ShakeAnimation()
    {
        if (border == null) yield break;

        float shakeDuration = 0.3f;
        float timer = 0f;

        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;

            // Случайное смещение
            Vector3 shakeOffset = new Vector3(
                Random.Range(-shakeIntensity, shakeIntensity),
                Random.Range(-shakeIntensity, shakeIntensity),
                0
            );

            border.rectTransform.localPosition = originalBorderPosition + shakeOffset;
            yield return null;
        }

        // Возвращаем на место
        border.rectTransform.localPosition = originalBorderPosition;
    }

    void OnDestroy()
    {
        if (healthSystem != null)
            healthSystem.OnHealthChanged -= OnHealthChanged;
    }
}