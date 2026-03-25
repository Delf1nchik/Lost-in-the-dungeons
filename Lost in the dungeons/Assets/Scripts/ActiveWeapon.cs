using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }
    public static int CurrentWeaponIndex = 0;

    [SerializeField] private Gun gun;
    [SerializeField] private Sword sword;

    private void Awake()
    {
        Instance = this;
    }

    public abstract class Weapon : MonoBehaviour
    {
        public abstract void Attack();
    }

    public Weapon GetActiveWeapon()
    {
        switch (CurrentWeaponIndex)
        {
            case 0: return gun;
            case 1: return sword;
        }
        return null;
    }
}
