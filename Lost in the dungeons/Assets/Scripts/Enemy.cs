using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy settings")]
    public float moveSpeed = 2f;
    public int health = 3;
    public int damage = 1;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    void Start()
    {
      
    }

    void Update()
    {
        
        if (player != null)
        {
            if (player.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    void FixedUpdate()
    {
       
        if (player != null)
        {
            
            Vector2 direction = (player.position - transform.position).normalized;


            rb.linearVelocity = direction * moveSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy hit player!");
            
        }
    }
}
