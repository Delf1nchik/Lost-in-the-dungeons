using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public GameObject DoorPrefab;
    public GameObject Player;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) SceneManager.LoadScene(1);
    }
}
