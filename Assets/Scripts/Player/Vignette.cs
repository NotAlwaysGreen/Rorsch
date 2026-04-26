using UnityEngine;
using System.Collections;

public class VignetteController : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public float fadeInTime = 0.15f;
    public float fadeOutTime = 0.35f;

    private Coroutine currentEffect;

    void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }

    public void PlayEffect()
    {
        if (currentEffect != null)
        {
            StopCoroutine(currentEffect);
        }

        currentEffect = StartCoroutine(FadeEffect());
    }

    IEnumerator FadeEffect()
    {
        canvasGroup.alpha = 0f;

        float time = 0f;

        // Fade In
        while (time < fadeInTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeInTime);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Fade Out
        time = 0f;

        while (time < fadeOutTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeOutTime);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}