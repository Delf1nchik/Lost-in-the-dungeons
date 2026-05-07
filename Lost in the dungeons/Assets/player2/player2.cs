using UnityEngine;

public class Player2 : MonoBehaviour
{
    public static Player2 instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;

    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 direction;
    private bool isInitialized = false;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject); // Персонаж тоже сохраняется
    }

    private void Start()
    {
        if (GameInput2.instance == null)
        {
            Debug.LogError("GameInput2 íå íàéäåí!");
            enabled = false;
            return;
        }

        GameInput2.instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        GameInput2.instance.OnWeaponChange += GameInput_OnWeaponChange;
        ActiveGun.Instance.transform.GetChild(1).gameObject.SetActive(false);
        ActiveGun.Instance.transform.GetChild(2).gameObject.SetActive(false);
        isInitialized = true;
    }

    private void Update()
    {
        // Обработка анимаций (оставляем как было)
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
            ActiveGun.Instance.GetActiveGun()?.Attack();
        }
    }

    private void GameInput_OnWeaponChange(object sender, System.EventArgs e)
    {
        ActiveGun.Instance.transform.GetChild(ActiveGun.CurrentWeaponIndex).gameObject.SetActive(false);
        ActiveGun.CurrentWeaponIndex = (ActiveGun.CurrentWeaponIndex + 1) % 3;
        ActiveGun.Instance.transform.GetChild(ActiveGun.CurrentWeaponIndex).gameObject.SetActive(true);
    }

    public Vector3 GetPlayerScreenPos()
    {
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPos;
    }

    private void FixedUpdate()
    {
        // Защита от null: если GameInput2.instance отсутствует, не пытаемся двигаться
        if (!isInitialized || GameInput2.instance == null)
        {
            Debug.LogError("GameInput2.instance is null in FixedUpdate! Movement disabled.");
            return;
        }

        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        // Если inputVector нулевой, движение не будет применено, но ошибки не будет
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
    }
    private void OnDestroy()
    {
        if (GameInput2.instance != null)
        {
            GameInput2.instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
        }
    }
}