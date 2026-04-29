using UnityEngine;
using System.Collections;

public class UIFadeText : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;
    public float displayTime = 2f;

    private Coroutine currentRoutine;

    void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    public void ShowText()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        // Fade in
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1;

        // Wait
        yield return new WaitForSeconds(displayTime);

        // Fade out
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}