using UnityEngine;

public class Player2 : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 10f;

    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 direction;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameInput2.instance.OnPlayerAttack += GameInput_OnPlayerAttack;
    }
    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("speed", direction.sqrMagnitude);
    }
    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveGun.Instance.GetActiveGun().Shoot();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = GameInput2.instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(GameInput2.instance.GetMousePosition());
        //Vector2 aimDirection = mousePosition - rb.position;
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = aimAngle;
    }
}
