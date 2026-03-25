using UnityEngine;
using System.Transactions;
using Unity.VisualScripting;

public class Player2 : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 10f;

    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 direction;
    private bool isInitialized = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (GameInput2.instance == null)
        {
            Debug.LogError("GameInput2 не найден!");
            enabled = false;
            return;
        }
        //GameInput2.instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        //GameInput2.instance.OnWeaponChange += GameInput_OnWeaponChange;
        // ActiveWeapon.Instance.transform.GetChild(1).gameObject.SetActive(false);

        GameInput2.instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        isInitialized = true;
    }

    private void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("speed", direction.sqrMagnitude);
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (ActiveGun.Instance != null)
        {
            ActiveGun.Instance.GetActiveGun()?.Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (!isInitialized || GameInput2.instance == null) return;

        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(GameInput2.instance.GetMousePosition());
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    private void OnDestroy()
    {
        // Отписываемся от события
        if (GameInput2.instance != null)
        {
            GameInput2.instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
        }
    }
}