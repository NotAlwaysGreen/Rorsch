using UnityEngine;
using UnityEngine.UI;

public class InsaneBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image insanityBar;

    [Header("Settings")]
    [SerializeField] private float barFillSpeed = 0.01f;

    [Header("Audio System")]
    [SerializeField] private MusicLayerController audioController;

    private float insanity;

    void Update()
    {
        IncreaseOverTime();
    }

    private void IncreaseOverTime()
    {
        insanity += barFillSpeed * Time.deltaTime;
        insanity = Mathf.Clamp01(insanity);

        UpdateSystems();
    }

    public void ReduceInsanity(float amount)
    {
        insanity -= amount;
        insanity = Mathf.Clamp01(insanity);

        UpdateSystems();
    }

    public void AddInsanity(float amount)
    {
        insanity += amount;
        insanity = Mathf.Clamp01(insanity);

        UpdateSystems();
    }

    public float GetInsanity()
    {
        return insanity;
    }

    private void UpdateSystems()
    {
        // UI update
        if (insanityBar != null)
            insanityBar.fillAmount = insanity;

        // Audio update
        if (audioController != null)
            audioController.SetInsanity(insanity);
    }
}