using UnityEngine;

public class NovaVisualEffect : MonoBehaviour
{
    [Header("Настройки анимации")]
    [SerializeField] private float expandSpeed = 15f; // Скорость расширения
    [SerializeField] private float fadeSpeed = 4f;    // Скорость исчезновения
    [SerializeField] private Color novaColor = new Color(0.4f, 0.8f, 1f, 0.8f); // Голубоватый цвет артефакта

    private SpriteRenderer sr;
    private float currentAlpha = 1f;

    void Start()
    {
        // Создаем SpriteRenderer программно, если его нет
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();

        // Создаем программно белый круг (чтобы не искать спрайт)
        sr.sprite = CreateCircleSprite();
        sr.color = novaColor;

        // Устанавливаем начальный размер
        transform.localScale = Vector3.zero;

        // Удаляем объект через 1 секунду (на всякий случай)
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        // 1. Расширяем круг
        transform.localScale += Vector3.one * expandSpeed * Time.deltaTime;

        // 2. Делаем его прозрачным
        currentAlpha -= fadeSpeed * Time.deltaTime;
        Color c = sr.color;
        c.a = currentAlpha;
        sr.color = c;

        // 3. Удаляем, когда совсем исчезнет
        if (currentAlpha <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Вспомогательный метод для создания текстуры круга
    private Sprite CreateCircleSprite()
    {
        int size = 128;
        Texture2D texture = new Texture2D(size, size);
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f - 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                if (dist < radius)
                    texture.SetPixel(x, y, Color.white);
                else
                    texture.SetPixel(x, y, Color.clear);
            }
        }
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }
}