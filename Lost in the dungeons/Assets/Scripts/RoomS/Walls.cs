
using UnityEngine;
public class Walls : MonoBehaviour
{
    public GameObject Wall;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            
            Instantiate(Wall, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}