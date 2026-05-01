using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Base Movement")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Dash Settings")]
    [SerializeField] private float dashVelocity = 25f; // �������� �����
    [SerializeField] private float dashTime = 0.2f;     // ������������ (����� ��������)
    [SerializeField] private float dashCooldown = 1f;   // �������

    private bool canDash = true;
    private bool isDashing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ���� �� � �����, ���������� ������� ����
        if (isDashing) return;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(PerformDash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator PerformDash()
    {
        Debug.Log("Dash Started!");

        canDash = false;
        isDashing = true;

        // ���������� ����������� ����� (���� ����, ���� � �����)
        // ���� ������ �� ������, ����� � �������, ���� ������� ������
        Vector2 dashDir = moveInput.normalized;
        if (dashDir == Vector2.zero)
        {
            dashDir = new Vector2(transform.localScale.x, 0);
        }

        // ��������� �������� �����
        rb.linearVelocity = dashDir * dashVelocity;

        // ������ ����: �� ����� ����� �������� ����� �������� ��� �������� ������ ������
        // Physics2D.IgnoreLayerCollision(LayerPlayer, LayerEnemy, true);

        yield return new WaitForSeconds(dashTime);

        rb.linearVelocity = Vector2.zero; // ������ ��������� � �����
        isDashing = false;

        // ���� �������
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}