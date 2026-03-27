using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDoor : MonoBehaviour
{
    public string bossSceneName = "BossRoom";

    private bool playerInside = false;
    private GameObject player; 

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            PlayerInventory inv = player.GetComponent<PlayerInventory>();

            if (inv.hasKey)
            {
                SceneManager.LoadScene(bossSceneName);
            }
            else
            {
                Debug.Log("Нужен ключ!");
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