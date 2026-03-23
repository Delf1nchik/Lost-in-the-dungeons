
using UnityEngine;
public class Walls : MonoBehaviour
{
    public GameObject Wall;
    public GameObject LWall;
    public GameObject RWall;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Instantiate(Wall, transform.GetChild(0).position, Quaternion.identity);
            Instantiate(LWall, transform.GetChild(0).position, Quaternion.identity);
            Instantiate(RWall, transform.GetChild(0).position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}