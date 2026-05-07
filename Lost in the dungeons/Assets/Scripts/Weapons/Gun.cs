using UnityEngine;

public class Gun : ActiveGun.Weapon
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireForce = 50f;
    private float timer = 0.6f;

    public override void Attack()
    {
        if (timer >= 0.6)
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

    private void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(GameInput2.instance.GetMousePosition());
        Vector2 aimDirection = mousePosition - (Vector2)transform.position;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg));
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90));
    }

}
