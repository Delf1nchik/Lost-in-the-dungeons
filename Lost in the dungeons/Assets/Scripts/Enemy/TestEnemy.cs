using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public float EnemyHealth = 30f;
    void Update()
    {
        if (EnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
