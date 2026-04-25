using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image staminaUi;

    [Header("Settings")]

    // 1 = full
    [SerializeField] private float stamina = 1f;

    [SerializeField] private float drainSpeed = 0.5f;

    [SerializeField] private float regenSpeed = 0.25f;

    public void Drain()
    {
        stamina -= drainSpeed * Time.deltaTime;

        stamina = Mathf.Clamp01(stamina);

        UpdateUI();
    }

    public void Regen()
    {
        stamina += regenSpeed * Time.deltaTime;

        stamina = Mathf.Clamp01(stamina);

        UpdateUI();
    }

    public float GetStamina()
    {
        return stamina;
    }

    private void UpdateUI()
    {
        if (staminaUi != null)
        {
            staminaUi.fillAmount = stamina;
        }
    }
}