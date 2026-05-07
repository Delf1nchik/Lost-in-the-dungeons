using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityDisplay : MonoBehaviour
{
    public Image abilityIcon;    // Ñþäà êèíü ñàìó èêîíêó (AbilityIcon)
    public Image cooldownImage;  // Ñþäà êèíü CooldownOverlay
    public TextMeshProUGUI cooldownText;

    void Update()
    {
        if (Player2.instance == null) return;

        // 1. ÏÐÎÂÅÐÊÀ ÐÀÇÁËÎÊÈÐÎÂÊÈ
        // Åñëè ñïîñîáíîñòü åùå íå îòêðûòà, âûêëþ÷àåì âñå ýëåìåíòû èíòåðôåéñà
        if (!Player2.instance.isDashUnlocked)
        {
            if (abilityIcon.enabled) abilityIcon.enabled = false;
            if (cooldownImage.enabled) cooldownImage.enabled = false;
            if (cooldownText != null) cooldownText.gameObject.SetActive(false);
            return; // Âûõîäèì èç ìåòîäà, äàëüøå êîä íå ïîéäåò
        }

        // 2. ÅÑËÈ ÎÒÊÐÛÒÀ — ÂÊËÞ×ÀÅÌ ÈÊÎÍÊÓ
        if (!abilityIcon.enabled) abilityIcon.enabled = true;

        // 3. ËÎÃÈÊÀ ÊÓËÄÀÓÍÀ (êàê â Dota 2)
        if (Player2.instance.dashTimer > 0)
        {
            if (!cooldownImage.enabled) cooldownImage.enabled = true;

            cooldownImage.fillAmount = Player2.instance.dashTimer / Player2.instance.dashCooldown;

            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(true);
                cooldownText.text = Mathf.CeilToInt(Player2.instance.dashTimer).ToString();
            }
        }
        else
        {
            if (cooldownImage.enabled) cooldownImage.enabled = false;
            if (cooldownText != null) cooldownText.gameObject.SetActive(false);
        }
    }
}