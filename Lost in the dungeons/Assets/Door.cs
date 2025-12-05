using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public void UseDoor()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(1);
        }
    }
}

