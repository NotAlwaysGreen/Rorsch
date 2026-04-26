using UnityEngine;

public class InsanityVignette : MonoBehaviour
{
    [Header("Reference")]
    public CanvasGroup canvasGroup;

    [Header("Fade")]
    public float fadeSpeed = 5f;

    private float targetAlpha;

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup missing on vignette object!");
            enabled = false;
            return;
        }

        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        canvasGroup.alpha = Mathf.Lerp(
            canvasGroup.alpha,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );
    }

    public void SetInsanity(bool active)
    {
        targetAlpha = active ? 1f : 0f;
    }
}