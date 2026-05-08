using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Объект не удалится при смене сцены
        }
        else
        {
            Destroy(gameObject); // Если в новой сцене уже есть такой объект, удаляем дубликат
        }
    }
}