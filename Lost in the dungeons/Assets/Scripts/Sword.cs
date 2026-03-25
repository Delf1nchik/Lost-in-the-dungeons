using UnityEngine;

public class Sword : ActiveWeapon.Weapon
{
    private bool attackStarted;
    private float timer = 1;
    private int direction = -1;

    public override void Attack()
    {
        if (timer > 0.25)
        {
            GetComponent<Collider2D>().enabled = true;
            attackStarted = true;
            timer = 0;
        }
    }

    private void Start()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.25 && attackStarted)
        {
            GetComponent<Collider2D>().enabled = false;
            attackStarted = false;
            direction *= -1;
        }

        if (attackStarted)
        {
            transform.RotateAround(transform.parent.position, Vector3.forward,  720 * direction * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<TestEnemy>().EnemyHealth -= 10f;
        }
    }
}
