using UnityEngine;
using UnityEngine.UI;

public class InsaneBar : MonoBehaviour
{
    [SerializeField] private Image insanityBar;

    [Header("Settings")]
    [SerializeField] private float barFillSpeed = 0.01f;

    private float insanity;

    void Update()
    {
        insanity += barFillSpeed * Time.deltaTime;

        insanity = Mathf.Clamp01(insanity);

        insanityBar.fillAmount = insanity;
    }

    public void ReduceInsanity(float amount)
    {
        insanity -= amount;

        insanity = Mathf.Clamp01(insanity);
    }

    public float GetInsanity()
    {
        return insanity;
    }
    public void AddInsanity(float amount)
    {
        insanity += amount;

        insanity = Mathf.Clamp01(insanity);
    }
}