using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI messageText;
    public CanvasGroup canvasGroup;

    public float fadeDuration = 0.5f;
    public float displayTime = 2f;

    private Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0f;
    }

    public void ShowMessage(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeMessage(message));
    }

    private IEnumerator FadeMessage(string message)
    {
        messageText.text = message;

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