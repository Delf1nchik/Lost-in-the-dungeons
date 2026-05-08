using UnityEngine;

public class DashGhost : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color color;
    [SerializeField] private float fadeSpeed = 5f; // Скорость исчезновения

    public void Init(Sprite playerSprite, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = playerSprite;
        transform.position = pos;
        transform.rotation = rot;
        transform.localScale = scale;

        // Устанавливаем начальный цвет (можно сделать полупрозрачным сразу)
        color = sr.color;
        color.a = 0.5f;
        sr.color = color;
    }

    void Update()
    {
        // Постепенно уменьшаем альфа-канал (прозрачность)
        color.a -= fadeSpeed * Time.deltaTime;
        sr.color = color;

        // Когда призрак стал невидимым — удаляем его
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}