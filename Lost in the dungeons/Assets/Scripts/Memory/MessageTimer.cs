using UnityEngine;
using System.Collections;

public class MessageTimer : MonoBehaviour
{
    public void StartTimer(float delay)
    {
        StopAllCoroutines();
        StartCoroutine(HideRoutine(delay));
    }

    private IEnumerator HideRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}