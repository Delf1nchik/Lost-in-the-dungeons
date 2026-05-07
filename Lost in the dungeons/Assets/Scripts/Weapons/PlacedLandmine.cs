using UnityEngine;

public class PlacedLandmine : MonoBehaviour
{
    public int damage = 15;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Skeleton skeleton))
        {
            Destroy(gameObject);
            skeleton.TakeDamage(damage);
            GameObject landmine = GameObject.Find("Landmine");
            landmine.GetComponent<Landmine>().currentLandmines--;
        }
    }
}
