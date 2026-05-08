using UnityEngine;

public class Key3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInventory>().hasKey2 = true;
            Destroy(gameObject);
        }
    }
}
