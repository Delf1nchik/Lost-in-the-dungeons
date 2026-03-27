using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public Transform bossRoomSpawnPoint;

    private bool playerInside = false;
    private GameObject player;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory inv = player.GetComponent<PlayerInventory>();

            if (inv.hasKey)
            {
                TeleportPlayer();
            }
            else
            {
                Debug.Log("Нужен ключ!");
            }
        }
    }

    void TeleportPlayer()
    {
        player.transform.position = bossRoomSpawnPoint.position;
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
        }
    }
}