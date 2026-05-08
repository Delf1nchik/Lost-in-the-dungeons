using UnityEngine;
using System.Collections;

public class BossSkeleton : Skeleton
{
    [Header("Boss Settings")]
    [SerializeField] private GameObject memoryShardPrefab; // Осколок рывка
    [SerializeField] private GameObject minionPrefab;      // Префаб обычного скелета
    [SerializeField] private float abilityCooldown = 5f;

    private bool isEnraged = false;
    private bool hasDroppedShard = false;

    protected override void Start()
    {
        // Вызываем Start из базового Skeleton (настройка HP и таргета)
        base.Start();
        StartCoroutine(BossLogicRoutine());
    }

    private IEnumerator BossLogicRoutine()
    {
        while (EnemyHP > 0)
        {
            yield return new WaitForSeconds(abilityCooldown);

            if (EnemyHP > 0)
            {
                // Способность: Призыв помощников
                SpawnMinions();
            }

            // Фаза 2: Гнев (когда HP < 50%)
            if (EnemyHP < 50 && !isEnraged)
            {
                EnterEnrageMode();
            }
        }
    }

    private void SpawnMinions()
    {
        animator.SetTrigger("Damage"); // Используем анимацию получения урона как "замах" для призыва
        Debug.Log("БОСС ПРИЗЫВАЕТ ПОДМОГУ!");

        for (int i = 0; i < 2; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * 3f;
            Instantiate(minionPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void EnterEnrageMode()
    {
        isEnraged = true;
        damage *= 1.5f; // Увеличиваем урон в полтора раза
        Debug.Log("БОСС В ЯРОСТИ! Урон увеличен.");
        // Можно изменить цвет босса на красный
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    // Переопределяем метод получения урона из Skeleton[cite: 4]
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (EnemyHP <= 0 && !hasDroppedShard)
        {
            DieAndDropShard();
        }
    }

    private void DieAndDropShard()
    {
        hasDroppedShard = true;
        Debug.Log("БОСС ПОВЕРЖЕН!");

        // Спавним осколок памяти, который разблокирует рывок в Player2
        if (memoryShardPrefab != null)
        {
            Instantiate(memoryShardPrefab, transform.position, Quaternion.identity);
        }
    }
}