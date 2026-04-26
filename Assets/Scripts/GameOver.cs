using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image background;

    [SerializeField] private TextMeshProUGUI youDiedText;

    [SerializeField] private TextMeshProUGUI dreamText;

    [SerializeField] private TextMeshProUGUI survivalTimeText;

    [SerializeField] private Image[] ui;

    [Header("Systems")]
    [SerializeField] private PauseMenu pauseMenu;

    [SerializeField] private TextMeshProUGUI ammo;

    [SerializeField] private SurvivalTimer survivalTimer;

    [Header("WIN CONDITION")]
    [SerializeField] private float winSurviveTime = 120f;

    [Header("RED DEATH CONTROLS")]

    [SerializeField] private float lightRedSpeed = 0.08f;

    [SerializeField] private float darkRedDelay = 5f;

    [SerializeField] private float darkRedSpeed = 4f;

    [SerializeField]
    private Color lightRed =
        new Color(1f, 0f, 0f, 0.35f);

    [SerializeField]
    private Color darkRed =
        new Color(0.2f, 0f, 0f, 0.9f);

    [Header("BLACK WIN CONTROLS")]

    [SerializeField]
    private Color dreamFade =
        new Color(0f, 0f, 0f, 0.35f);

    [SerializeField]
    private Color dreamDark =
        new Color(0f, 0f, 0f, 0.95f);

    [Header("Text")]

    [SerializeField] private float textAppearDelay = 1f;

    [SerializeField] private float textFadeSpeed = 0.5f;

    [Header("Restart")]

    [SerializeField] private float restartDelay = 4f;

    private bool started = false;

    private bool playerWon = false;

    private bool restarting = false;

    private bool darkeningStarted = false;

    private float timer = 0f;

    private string finalSurvivalTime;

    void Awake()
    {
        background.color = new Color(0f, 0f, 0f, 0f);

        HideText(youDiedText);
        HideText(dreamText);
        HideText(survivalTimeText);
    }

    void Update()
    {
        // =====================================
        // WIN CHECK
        // =====================================

        if (
            !started &&
            survivalTimer != null &&
            survivalTimer.GetCurrentTime() >= winSurviveTime
        )
        {
            StartDreamEnding();
        }

        if (!started) return;

        float delta = playerWon
            ? Time.unscaledDeltaTime
            : Time.deltaTime;

        timer += delta;

        // =====================================
        // WIN ENDING
        // =====================================

        if (playerWon)
        {
            // stage 1
            if (!darkeningStarted)
            {
                background.color = Color.Lerp(
                    background.color,
                    dreamFade,
                    lightRedSpeed * delta
                );
            }

            // stage 2
            if (timer >= darkRedDelay)
            {
                darkeningStarted = true;
            }

            // stage 3
            if (darkeningStarted)
            {
                background.color = Color.Lerp(
                    background.color,
                    dreamDark,
                    darkRedSpeed * delta
                );
            }

            // stage 4
            if (timer >= darkRedDelay + textAppearDelay)
            {
                FadeTextWhite(dreamText, delta);

                if (!restarting)
                {
                    restarting = true;
                    StartCoroutine(RestartSequence());
                }
            }

            return;
        }

        // =====================================
        // DEATH ENDING
        // =====================================

        // stage 1
        if (!darkeningStarted)
        {
            background.color = Color.Lerp(
                background.color,
                lightRed,
                lightRedSpeed * delta
            );
        }

        // stage 2
        if (timer >= darkRedDelay)
        {
            darkeningStarted = true;
        }

        // stage 3
        if (darkeningStarted)
        {
            background.color = Color.Lerp(
                background.color,
                darkRed,
                darkRedSpeed * delta
            );
        }

        // stage 4
        if (timer >= darkRedDelay + textAppearDelay)
        {
            FadeTextBlack(youDiedText, delta);

            FadeTextBlack(survivalTimeText, delta);

            if (!restarting)
            {
                restarting = true;
                StartCoroutine(RestartSequence());
            }
        }
    }

    // =====================================
    // START DEATH
    // =====================================

    public void StartGameOver()
    {
        started = true;

        playerWon = false;

        restarting = false;

        darkeningStarted = false;

        timer = 0f;

        if (survivalTimer != null)
        {
            survivalTimer.StopTimer();

            finalSurvivalTime =
                "You survived for: "
                + survivalTimer.GetFormattedTime();
        }

        // SET TEXT
        youDiedText.text = "YOU DIED!";

        survivalTimeText.text = finalSurvivalTime;

        // SHOW UI
        youDiedText.gameObject.SetActive(true);

        survivalTimeText.gameObject.SetActive(true);

        // HIDE WIN UI
        dreamText.gameObject.SetActive(false);

        DisableGameplayUI();
    }

    // =====================================
    // START WIN
    // =====================================

    void StartDreamEnding()
    {
        started = true;

        playerWon = true;

        restarting = false;

        darkeningStarted = false;

        timer = 0f;

        if (survivalTimer != null)
        {
            survivalTimer.StopTimer();
        }

        // SET TEXT
        dreamText.text = "It was all a dream..";

        // SHOW WIN UI
        dreamText.gameObject.SetActive(true);

        // HIDE DEATH UI
        youDiedText.gameObject.SetActive(false);

        survivalTimeText.gameObject.SetActive(false);

        DisableGameplayUI();

        // freeze gameplay
        Time.timeScale = 0f;
    }

    // =====================================
    // HELPERS
    // =====================================

    void DisableGameplayUI()
    {
        if (ammo != null)
        {
            ammo.gameObject.SetActive(false);
        }

        foreach (Image image in ui)
        {
            if (image != null)
            {
                image.gameObject.SetActive(false);
            }
        }

        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }
    }

    void HideText(TextMeshProUGUI text)
    {
        Color c = text.color;
        c.a = 0f;
        text.color = c;
    }

    void FadeTextBlack(TextMeshProUGUI text, float delta)
    {
        Color c = Color.black;

        c.a = Mathf.Lerp(
            text.color.a,
            1f,
            textFadeSpeed * delta
        );

        text.color = c;
    }

    void FadeTextWhite(TextMeshProUGUI text, float delta)
    {
        Color c = Color.white;

        c.a = Mathf.Lerp(
            text.color.a,
            1f,
            textFadeSpeed * delta
        );

        text.color = c;
    }

    IEnumerator RestartSequence()
    {
        yield return new WaitForSecondsRealtime(restartDelay);

        RestartGame();
    }

    void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}