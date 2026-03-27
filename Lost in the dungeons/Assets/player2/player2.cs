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
    }

    private void Start()
    {
        if (GameInput2.instance == null)
        {
            Debug.LogError("GameInput2 не найден!");
            enabled = false;
            return;
        }

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
            ActiveGun.Instance.GetActiveGun()?.Attack();
        }
    }

    private void FixedUpdate()
    {
        if (!isInitialized || GameInput2.instance == null) return;

        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
    }

    public Vector3 GetPlayerScreenPos()
    {
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPos;
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