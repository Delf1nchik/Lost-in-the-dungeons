using UnityEngine;

public class WallR : MonoBehaviour
{
    public GameObject LWall; 
    public GameObject RWall;
    private bool replaced = false; // чтобы замена произошла только один раз

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что столкнулись с объектом с тегом "Wall" и дверь ещё не была заменена
        if (other.CompareTag("Wall") && !replaced)
        {
            replaced = true;

            // Получаем угол поворота двери по оси Y
            float yRotation = transform.eulerAngles.y;

            // Если дверь повёрнута на 180° (с допустимым отклонением 0.5°), спавним LWall
            if (Mathf.Abs(yRotation - 180f) < 0.5f)
            {
                Instantiate(RWall, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(LWall, transform.position, Quaternion.identity);
            }

            // Удаляем дверь
            Destroy(gameObject);
        }
    }
}