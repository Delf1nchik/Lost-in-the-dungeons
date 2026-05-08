using UnityEngine;

public class key2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInventory>().hasKey1 = true;
            Destroy(gameObject);
        }
    }
}
