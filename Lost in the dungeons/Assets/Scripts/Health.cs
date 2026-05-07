using UnityEngine;

using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class Health : MonoBehaviour

{

    public Image healthBar;

    public float Maxhealth = 100f;

    public float HP;

    private bool isDead = false;

<<<<<<< Updated upstream
 
=======
   
>>>>>>> Stashed changes


    void Start()

    {

        HP = Maxhealth;
        UpdateBar();
        FindHealthBar();

    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)

    {
        UpdateBar();
        FindHealthBar();

    }


    void FindHealthBar()

    {

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


        if (healthBar == null) FindHealthBar();


        if (HP <= 0)

        {

            HP = 0;

            TriggerDeath();

        }

        UpdateBar();

        Debug.Log("Игрок получил урон! Осталось: " + HP);

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

    void UpdateBar()

    {

        if (healthBar != null)

        {

            healthBar.fillAmount = HP / Maxhealth;

        }

    }


<<<<<<< Updated upstream
 
=======
  
>>>>>>> Stashed changes
}