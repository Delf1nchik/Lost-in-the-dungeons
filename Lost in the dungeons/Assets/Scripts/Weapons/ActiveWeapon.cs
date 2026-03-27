using UnityEngine;

public class ActiveGun : MonoBehaviour
{
    public static ActiveGun Instance { get; private set; }

    //[SerializeField] private Gun gun;
    [SerializeField] private Sword sword;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        FollowMousePosition();
    }

    private void FollowMousePosition()
    {
        Vector3 mousePos = GameInput2.instance.GetMousePosition();
        Vector3 playerPos = Player2.instance.GetPlayerScreenPos();

        if (mousePos.x < playerPos.x)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public Sword GetActiveGun()
    {
        return sword; 
    }
}
