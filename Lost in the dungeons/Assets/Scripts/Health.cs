using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image healthBar; // вручную задаём
    public float Maxhealth = 100f;
    public float HP;

    void Start()
    {
        HP = Maxhealth;
        UpdateBar();
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;

        if (HP < 0) HP = 0;

        UpdateBar();
    }

    void UpdateBar()
    {
        if (healthBar != null)
            healthBar.fillAmount = HP / Maxhealth;
    }
}