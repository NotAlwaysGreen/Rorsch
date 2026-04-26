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

    [Header("Game Over")]
    [SerializeField] private GameOver gameOverUI;
    [SerializeField] private FPSController playerController;
    [SerializeField] private EnemySpawner enemySpawner;

    private bool gameOverTriggered = false;

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
        // UI
        if (insanityBar != null)
            insanityBar.fillAmount = insanity;

        // Audio
        if (audioController != null)
            audioController.SetInsanity(insanity);

        // Game Over Trigger
        if (insanity >= 1f && !gameOverTriggered)
        {
            gameOverTriggered = true;

            if (playerController != null)
            {
                playerController.StartMovementShutdown();
            }

            if (enemySpawner != null)
            {
                enemySpawner.EnableGameOverMode();
            }

            if (gameOverUI != null)
            {
                gameOverUI.StartGameOver();
            }
        }
    }
}
