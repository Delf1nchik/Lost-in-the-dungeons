using UnityEngine;

public class Gun : ActiveWeapon.Weapon
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireForce = 20f;
    public override void Attack()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }
}
