using UnityEngine;
using UnityEngine.SceneManagement;
public class Afterboss : MonoBehaviour
{
    public string level;

    private bool playerInside = false;
    private GameObject player;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            PlayerInventory inv = player.GetComponent<PlayerInventory>();

            if (inv.hasShard)
            {
                SceneManager.LoadScene(level);
            }
            else
            {
                Debug.Log("Нужен осколок!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }
}
