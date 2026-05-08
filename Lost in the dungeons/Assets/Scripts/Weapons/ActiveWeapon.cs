using UnityEngine;

public class ActiveGun : MonoBehaviour
{
    public static ActiveGun Instance { get; private set; }
    public static int CurrentWeaponIndex = 0;

    [SerializeField] private Gun gun;
    [SerializeField] private Sword sword;
    [SerializeField] private Landmine landmine;

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
            gun.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            gun.GetComponent<SpriteRenderer>().flipY = false;
        }
    }

    public abstract class Weapon : MonoBehaviour
    {
        public abstract void Attack();
    }

    public Weapon GetActiveGun()
    {
        switch (CurrentWeaponIndex)
        {
            case 0: return sword;
            case 1: return gun;
            case 2: return landmine;
        }
        return null;
    }
}
