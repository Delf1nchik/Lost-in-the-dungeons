using UnityEngine;

public class ActiveGun : MonoBehaviour
{
    public static ActiveGun Instance { get; private set; }

    [SerializeField] private Gun gun;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {

    }
    public Gun GetActiveGun()
    {
        return gun; 
    }
}
