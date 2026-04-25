using UnityEngine;
using System.Collections;

public class InsanityVignette : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public float fadeInTime = 0.15f;
    public float fadeOutTime = 0.35f;

    public bool InsaneSprint;

    private bool lastState;
    private Coroutine transition;

    void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (InsaneSprint != lastState)
        {
            lastState = InsaneSprint;

            if (transition != null)
                StopCoroutine(transition);

            transition = StartCoroutine(FadeState(InsaneSprint));
        }
    }

    IEnumerator FadeState(bool state)
    {
        float time = 0f;

        float start = canvasGroup.alpha;
        float target = state ? 1f : 0f;
        float duration = state ? fadeInTime : fadeOutTime;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = target;
    }
}