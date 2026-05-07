using UnityEngine;

public class Landmine : ActiveGun.Weapon
{
    public GameObject placedLandminePrefab;
    public int maxLandmines = 3;
    public int currentLandmines = 0;
    private float timer = 0.6f;

    public override void Attack()
    {
        if (timer >= 0.6 && currentLandmines < maxLandmines)
        {
            Instantiate(placedLandminePrefab, transform.position, transform.rotation);
            currentLandmines++;
            timer = 0;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
}
