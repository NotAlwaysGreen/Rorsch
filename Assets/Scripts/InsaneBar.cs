using UnityEngine;

using UnityEngine.UI;
public class InsaneBar : MonoBehaviour
{

    public Image insanityBar;

    [Header("Settings")]
    public float BarFillSpeed = 0.01f; 

    private float insanity = 0f;

    void Update()
    {
        BarFill();
    }

    private void BarFill()
    {
        // slowly increase over time
        insanity += BarFillSpeed * Time.deltaTime;

        // clamp between 0 and 1
        insanity = Mathf.Clamp01(insanity);

        // update UI
        insanityBar.fillAmount = insanity;
    }


    //will call this when it Grabs Pills 
    public void ReduceInsanity(float amount)
    {
        insanity -= amount;
        insanity = Mathf.Clamp01(insanity);
    }
}
