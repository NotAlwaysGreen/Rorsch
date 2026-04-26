using UnityEngine;

public class SurvivalTimer : MonoBehaviour
{
    private float survivalTime = 0f;

    private bool stopped = false;

    void Update()
    {
        if (stopped) return;

        survivalTime += Time.deltaTime;
    }

    // returns raw seconds
    public float GetCurrentTime()
    {
        return survivalTime;
    }

    // formatted MM:SS
    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(survivalTime / 60f);

        int seconds = Mathf.FloorToInt(survivalTime % 60f);

        return minutes.ToString("00")
            + ":"
            + seconds.ToString("00");
    }

    // stop timer when game over starts
    public void StopTimer()
    {
        stopped = true;
    }
}