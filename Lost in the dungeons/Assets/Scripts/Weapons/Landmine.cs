using UnityEngine;

public class Landmine : ActiveGun.Weapon
{
    public GameObject placedLandminePrefab;
    public int maxLandmines = 3;
    public static int currentLandmines = 0;
    private float timer = 0.6f;

    [Header("Audio")]
    public AudioSource audioSource; // Перетащи сюда компонент AudioSource
    public AudioClip placeSound;    // Звук установки мины

    public override void Attack()
    {
        if (timer >= 0.6 && currentLandmines < maxLandmines)
        {
            Instantiate(placedLandminePrefab, transform.position, transform.rotation);

            // Воспроизводим звук установки
            if (audioSource != null && placeSound != null)
            {
                audioSource.PlayOneShot(placeSound);
            }

            currentLandmines++;
            timer = 0;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
}