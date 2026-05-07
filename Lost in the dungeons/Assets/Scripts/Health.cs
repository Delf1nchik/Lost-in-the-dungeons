using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image healthBar;
    public float Maxhealth = 100f;
    public float HP;

    private bool isDead = false;

    void Start()
    {
        HP = Maxhealth;
        UpdateBar();
    }

    public void TakeDamage(float damage)
    {
        // Если уже мертвы, урон не принимаем
        if (isDead) return;

        HP -= damage;

        if (HP <= 0)
        {
            HP = 0;
            TriggerDeath();
        }
        UpdateBar();
    }

    void UpdateBar()
    {
        if (healthBar != null)
            healthBar.fillAmount = HP / Maxhealth;
    }

    private void TriggerDeath()
    {
        isDead = true;

        // Обращаемся к классу Player2 через его instance и вызываем его метод Die
        // Это отключит управление, коллайдер и запустит анимацию
        if (Player2.instance != null)
        {
            Player2.instance.Die();
        }

        // Здесь можно добавить дополнительную логику, характерную только для здоровья
        // Например, выключить скрипт здоровья или запустить таймер перезагрузки
        DeathScreenManager.Instance?.ShowDeathScreen();
        gameObject.SetActive(false); // отключаем игрока
        Debug.Log("Персонаж погиб.");
    }
}