using UnityEngine;

public class Gun : ActiveWeapon.Weapon
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireForce = 20f;
    private float timer = 0.5f;
    public override void Attack()
    {
        if (timer >= 0.5)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            timer = 0;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
}
