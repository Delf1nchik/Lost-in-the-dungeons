using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Smooth Settings")]
    public float healthChangeSpeed = 3f; // Скорость плавного изменения (0 = мгновенно)

    [Header("Status")]
    public bool isDead = false;

    // События для связи с другими системами
    public System.Action<int> OnHealthChanged;
    public System.Action OnDeath;

    private float displayHealth; // Плавно изменяемое здоровье для отображения
    private Coroutine healthCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        displayHealth = currentHealth;
        isDead = false;

        Debug.Log($"Health system started: {currentHealth}/{maxHealth}");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int newHealth = currentHealth - damage;
        if (newHealth < 0) newHealth = 0;

        currentHealth = newHealth;

        // Плавное изменение если скорость > 0
        if (healthChangeSpeed > 0)
        {
            if (healthCoroutine != null)
                StopCoroutine(healthCoroutine);
            healthCoroutine = StartCoroutine(SmoothHealthChange(newHealth));
        }
        else
        {
            // Мгновенное изменение
            displayHealth = newHealth;
            OnHealthChanged?.Invoke(newHealth);
        }

        Debug.Log($"Damage taken: {damage}. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int healAmount)
    {
        if (isDead) return;

        int newHealth = currentHealth + healAmount;
        if (newHealth > maxHealth) newHealth = maxHealth;

        currentHealth = newHealth;

        // Плавное изменение если скорость > 0
        if (healthChangeSpeed > 0)
        {
            if (healthCoroutine != null)
                StopCoroutine(healthCoroutine);
            healthCoroutine = StartCoroutine(SmoothHealthChange(newHealth));
        }
        else
        {
            // Мгновенное изменение
            displayHealth = newHealth;
            OnHealthChanged?.Invoke(newHealth);
        }

        Debug.Log($"Healed: {healAmount}. Health: {currentHealth}/{maxHealth}");
    }

    IEnumerator SmoothHealthChange(int targetHealth)
    {
        float startHealth = displayHealth;
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * healthChangeSpeed;
            displayHealth = Mathf.Lerp(startHealth, targetHealth, timer);

            // Вызываем событие с плавным значением для UI
            OnHealthChanged?.Invoke(Mathf.RoundToInt(displayHealth));

            yield return null;
        }

        displayHealth = targetHealth;
        OnHealthChanged?.Invoke(targetHealth);
    }

    void Die()
    {
        isDead = true;
        Debug.Log("💀 Entity died!");
        OnDeath?.Invoke();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        displayHealth = currentHealth;
        isDead = false;

        if (healthCoroutine != null)
            StopCoroutine(healthCoroutine);

        Debug.Log($"Health reset: {currentHealth}/{maxHealth}");
        OnHealthChanged?.Invoke(currentHealth);
    }

    // Геттеры
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public float GetHealthPercentage() => (float)currentHealth / maxHealth;
    public bool IsAlive() => !isDead;

    // Новый метод для плавного отображения
    public float GetDisplayHealth() => displayHealth;
}