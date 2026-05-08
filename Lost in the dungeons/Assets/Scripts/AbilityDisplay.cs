using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum AbilityType { Dash, Nova }

public class AbilityDisplay : MonoBehaviour
{
    public AbilityType type; // Выбери тип в инспекторе
    public Image abilityIcon;
    public Image cooldownImage;
    public TextMeshProUGUI cooldownText;

    void Update()
    {
        if (Player2.instance == null) return;

        bool isUnlocked = false;
        float currentTimer = 0;
        float maxCooldown = 1;

        // Выбираем данные в зависимости от типа способности
        if (type == AbilityType.Dash)
        {
            isUnlocked = Player2.instance.isDashUnlocked;
            currentTimer = Player2.instance.dashTimer;
            maxCooldown = Player2.instance.dashCooldown;
        }
        else
        {
            isUnlocked = Player2.instance.isNovaUnlocked;
            currentTimer = Player2.instance.novaTimer;
            maxCooldown = 4f; // Твой novaCooldown
        }

        // Логика отображения (остается твоя прежняя)
        if (!isUnlocked)
        {
            abilityIcon.enabled = false;
            cooldownImage.enabled = false;
            if (cooldownText != null) cooldownText.gameObject.SetActive(false);
            return;
        }

        abilityIcon.enabled = true;
        if (currentTimer > 0)
        {
            cooldownImage.enabled = true;
            cooldownImage.fillAmount = currentTimer / maxCooldown;
            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(true);
                cooldownText.text = Mathf.CeilToInt(currentTimer).ToString();
            }
        }
        else
        {
            cooldownImage.enabled = false;
            if (cooldownText != null) cooldownText.gameObject.SetActive(false);
        }
    }
}