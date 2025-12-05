using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Enemy settings")]
    public float moveSpeed = 2f;
    public int health = 3;
    public int damage = 1;

    [Header("Obstacle avoidance")]
    public float obstacleCheckDistance = 1.5f;
    public LayerMask obstacleLayer = -1; 
    public bool drawDebugRays = true;

    [Header("Advanced avoidance")]
    public float sideCheckAngle = 45f;
    public int rayCount = 5;
    public float raySpacing = 15f;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Vector2 currentDirection;
    private float stuckTimer = 0f;
    private Vector2 lastPosition;

    void Start()
    {
        
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found! Add 'Player' tag to player object.");
        }

        
        obstacleLayer = ~(LayerMask.GetMask("Player") | LayerMask.GetMask("Enemy"));

        lastPosition = transform.position;
    }

    void Update()
    {
        if (player != null)
        {
            
            spriteRenderer.flipX = player.position.x < transform.position.x;
        }

        
        CheckIfStuck();
    }

    void FixedUpdate()
    {
        if (player == null || rb == null) return;

        
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        
        Vector2 finalDirection = SmartObstacleAvoidance(directionToPlayer);

        
        currentDirection = finalDirection;

        
        rb.linearVelocity = finalDirection * moveSpeed;
    }

    Vector2 SmartObstacleAvoidance(Vector2 desiredDirection)
    {
        
        if (!CheckObstacleInDirection(desiredDirection, obstacleCheckDistance))
        {
            return desiredDirection;
        }

        
        float bestScore = -Mathf.Infinity;
        Vector2 bestDirection = desiredDirection;

        
        for (int i = 0; i < rayCount; i++)
        {
            
            float angle = -sideCheckAngle + (i * (2 * sideCheckAngle / (rayCount - 1)));
            Vector2 testDirection = Quaternion.Euler(0, 0, angle) * desiredDirection;

            
            bool hasObstacle = CheckObstacleInDirection(testDirection, obstacleCheckDistance);
            float distanceToObstacle = GetObstacleDistance(testDirection);

            
            float score = EvaluateDirection(testDirection, desiredDirection, hasObstacle, distanceToObstacle);

            if (score > bestScore)
            {
                bestScore = score;
                bestDirection = testDirection;
            }
        }

        
        if (bestScore < 0)
        {
            bestDirection = Quaternion.Euler(0, 0, Random.Range(-90f, 90f)) * desiredDirection;
        }

        return bestDirection.normalized;
    }

    bool CheckObstacleInDirection(Vector2 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            distance,
            obstacleLayer
        );

        
        return hit.collider != null && !hit.collider.isTrigger && hit.collider.gameObject != gameObject;
    }

    float GetObstacleDistance(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            obstacleCheckDistance,
            obstacleLayer
        );

        return hit.collider != null ? hit.distance : Mathf.Infinity;
    }

    float EvaluateDirection(Vector2 testDirection, Vector2 desiredDirection, bool hasObstacle, float obstacleDistance)
    {
        float score = 0f;

        
        float dot = Vector2.Dot(testDirection.normalized, desiredDirection.normalized);
        score += dot * 50f;

        
        if (hasObstacle)
        {
            score -= 100f;
            
            if (obstacleDistance > 0)
            {
                score += obstacleDistance * 20f;
            }
        }
        else
        {
            
            score += 200f;
        }

        return score;
    }

    void CheckIfStuck()
    {
        
        float distanceMoved = Vector2.Distance(transform.position, lastPosition);

        if (distanceMoved < 0.01f)
        {
            stuckTimer += Time.deltaTime;

            
            if (stuckTimer > 1f)
            {
                Debug.Log("Enemy is stuck! Trying to escape...");

                
                EscapeStuckSituation();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = transform.position;
    }

    void EscapeStuckSituation()
    {
        
        Vector2 escapeDirection = Random.insideUnitCircle.normalized;

        
        for (int i = 0; i < 8; i++)
        {
            if (!CheckObstacleInDirection(escapeDirection, 0.5f))
            {
                rb.linearVelocity = escapeDirection * moveSpeed * 2f;
                return;
            }
            escapeDirection = Quaternion.Euler(0, 0, 45) * escapeDirection;
        }
    }

    
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        
        StartCoroutine(DamageFlash());

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy hit player!");

            
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = knockbackDirection * 5f;
            }
        }
    }

    
    void OnDrawGizmos()
    {
        if (!drawDebugRays) return;

        
        if (player != null)
        {
            Vector2 directionToPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;

            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, directionToPlayer * obstacleCheckDistance);

            
            Gizmos.color = Color.yellow;
            for (int i = 0; i < rayCount; i++)
            {
                float angle = -sideCheckAngle + (i * (2 * sideCheckAngle / (rayCount - 1)));
                Vector2 testDirection = Quaternion.Euler(0, 0, angle) * directionToPlayer;
                Gizmos.DrawRay(transform.position, testDirection * obstacleCheckDistance);
            }

            
            if (Application.isPlaying)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(transform.position, currentDirection * 1f);
            }
        }

        
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawWireSphere(transform.position, obstacleCheckDistance);
    }

    
    void OnDrawGizmosSelected()
    {
        if (!drawDebugRays) return;

        
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 1.2f);
    }
}