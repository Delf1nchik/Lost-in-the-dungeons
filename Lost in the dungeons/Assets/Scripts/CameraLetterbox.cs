using UnityEngine;

public class CameraLetterbox : MonoBehaviour
{
    // Эталонное соотношение сторон — 16:10
    private const float TargetAspect = 16f / 10f; // = 1.6f

    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        UpdateViewport();
    }

    void Update()
    {
        // Если разрешение экрана изменилось в реальном времени (например, в редакторе)
        if (Screen.width != _prevWidth || Screen.height != _prevHeight)
        {
            UpdateViewport();
        }
    }

    private int _prevWidth;
    private int _prevHeight;

    void UpdateViewport()
    {
        // Текущее соотношение экрана
        float currentAspect = (float)Screen.width / Screen.height;

        // Множитель масштаба viewport
        float scaleHeight = currentAspect / TargetAspect;
        float scaleWidth = TargetAspect / currentAspect;

        Rect rect = _camera.rect;

        if (scaleHeight < 1f) // Экран уже, чем 16:10 (например, 4:3)
        {
            // Уменьшаем высоту viewport, добавляем полосы сверху/снизу
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else if (scaleWidth < 1f) // Экран шире, чем 16:10 (например, 16:9)
        {
            // Уменьшаем ширину viewport, добавляем полосы слева/справа
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0;
        }
        else // Соотношение точно 16:10
        {
            rect.width = 1f;
            rect.height = 1f;
            rect.x = 0;
            rect.y = 0;
        }

        _camera.rect = rect;

        // Сохраняем текущие размеры для проверки изменений
        _prevWidth = Screen.width;
        _prevHeight = Screen.height;
    }
}