using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 10f;

    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameInput2.instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        GameInput2.instance.OnWeaponChange += GameInput_OnWeaponChange;
        ActiveWeapon.Instance.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void GameInput_OnWeaponChange(object sender, System.EventArgs e)
    {
        ActiveWeapon.CurrentWeaponIndex = (ActiveWeapon.CurrentWeaponIndex + 1) % 2;
        foreach (Transform child in ActiveWeapon.Instance.transform)
        {
            child.transform.gameObject.SetActive(false);
        }
        ActiveWeapon.Instance.transform.GetChild(ActiveWeapon.CurrentWeaponIndex).gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(GameInput2.instance.GetMousePosition());
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
}
