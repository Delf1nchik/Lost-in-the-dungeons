using UnityEngine;
using UnityEngine.InputSystem;

public class HealthTester : MonoBehaviour
{
    private HealthSystem health;
    private Keyboard keyboard;

    void Start()
    {
        health = GetComponent<HealthSystem>();
        keyboard = Keyboard.current;
        Debug.Log("HealthTester готов! Нажми 1 или 2 для теста");
    }

    void Update()
    {
        if (keyboard == null) return;

        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            Debug.Log("Нажата 1 - наносим урон");
            health.TakeDamage(10);
        }

        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            Debug.Log("Нажата 2 - лечим");
            health.Heal(15);
        }
    }
}