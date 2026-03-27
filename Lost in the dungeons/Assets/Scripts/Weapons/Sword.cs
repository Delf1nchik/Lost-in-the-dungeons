using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sword : MonoBehaviour
{
    public event EventHandler OnSwordSwing;

    public void Attack()
    {
        Debug.Log("ATTACK");
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }
}
 