using UnityEngine;

public class Sword : ActiveWeapon.Weapon
{
    public override void Attack()
    {
        Debug.Log("удар");
    }
}
