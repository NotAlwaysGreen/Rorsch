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

    // NEW TEXT REFERENCE
    [SerializeField] private TextMeshProUGUI dreamText;

    [SerializeField] private TextMeshProUGUI survivalTimeText;

    [SerializeField] private Image[] ui;

    [Header("Systems")]
    [SerializeField] private PauseMenu pauseMenu;

    [SerializeField] private TextMeshProUGUI ammo;

    [SerializeField] private SurvivalTimer survivalTimer;

    [Header("WIN CONDITION")]
    [SerializeField] private float winSurviveTime = 120f;

    [Header("Stage 1 - Light Red Fade")]
    [SerializeField] private float lightRedSpeed = 0.08f;

    [SerializeField]
    private Color lightRed =
        new Color(1f, 0f, 0f, 0.35f);

    [Header("Stage 2 - Delay Before Dark Red")]
    [SerializeField] private float darkRedDelay = 5f;

    [Header("Stage 3 - Fast Dark Red")]
    [SerializeField] private float darkRedSpeed = 4f;

    [SerializeField]
    private Color darkRed =
        new Color(0.2f, 0f, 0f, 0.9f);

    [Header("Stage 4 - Text")]
    [SerializeField] private float textAppearDelay = 1f;

    [SerializeField] private float textFadeSpeed = 0.5f;

    [Header("Restart Transition")]
    [SerializeField] private float restartDelay = 4f;

    [SerializeField] private float finalFadeSpeed = 1f;

    private bool started = false;

    private float timer = 0f;

    private bool darkeningStarted = false;

    private bool restarting = false;

    private bool playerWon = false;

    private string finalSurvivalTime;

    void Awake()
    {
        background.color = new Color(0f, 0f, 0f, 0f);

        Color text = youDiedText.color;
        text.a = 0f;
        youDiedText.color = text;

        Color dream = dreamText.color;
        dream.a = 0f;
        dreamText.color = dream;

        Color survival = survivalTimeText.color;
        survival.a = 0f;
        survivalTimeText.color = survival;
    }

    void Update()
    {
        if (!started) return;

        timer += Time.deltaTime;

        // =========================
        // WIN SEQUENCE
        // =========================
        if (playerWon)
        {
            background.color = Color.Lerp(
                background.color,
                new Color(0f, 0f, 0f, 1f),
                2f * Time.deltaTime
            );

            // DREAM TEXT FADE
            Color dream = dreamText.color;

            dream.a = Mathf.Lerp(
                dream.a,
                1f,
                textFadeSpeed * Time.deltaTime
            );

            dreamText.color = dream;

            // SURVIVAL TEXT FADE
            Color survival = survivalTimeText.color;

            survival.a = Mathf.Lerp(
                survival.a,
                1f,
                textFadeSpeed * Time.deltaTime
            );

            survivalTimeText.color = survival;

            if (!restarting)
            {
                restarting = true;
                StartCoroutine(RestartSequence());
            }

            return;
        }

        // =========================
        // NORMAL DEATH SEQUENCE
        // =========================

        if (!darkeningStarted)
        {
            background.color = Color.Lerp(
                background.color,
                lightRed,
                lightRedSpeed * Time.deltaTime
            );
        }

        if (timer >= darkRedDelay)
        {
            darkeningStarted = true;
        }

        if (darkeningStarted)
        {
            background.color = Color.Lerp(
                background.color,
                darkRed,
                darkRedSpeed * Time.deltaTime
            );
        }

        if (timer >= darkRedDelay + textAppearDelay)
        {
            // YOU DIED TEXT
            Color text = youDiedText.color;

            text.a = Mathf.Lerp(
                text.a,
                1f,
                textFadeSpeed * Time.deltaTime
            );

            youDiedText.color = text;

            // SURVIVAL TIME
            survivalTimeText.text = finalSurvivalTime;

            Color survival = survivalTimeText.color;

            survival.a = Mathf.Lerp(
                survival.a,
                1f,
                textFadeSpeed * Time.deltaTime
            );

            survivalTimeText.color = survival;

            if (!restarting)
            {
                restarting = true;
                StartCoroutine(RestartSequence());
            }
        }
    }

    public void StartGameOver()
    {
        started = true;

        float survivedSeconds = 0f;

        if (survivalTimer != null)
        {
            survivalTimer.StopTimer();

            survivedSeconds = survivalTimer.GetCurrentTime();

            finalSurvivalTime =
                "You survived for: "
                + survivalTimer.GetFormattedTime();
        }

        // =========================
        // CHECK WIN CONDITION
        // =========================
        playerWon = survivedSeconds >= winSurviveTime;

        if (playerWon)
        {
            background.color = new Color(0f, 0f, 0f, 0f);

            // hide normal death text
            youDiedText.gameObject.SetActive(false);

            // dream text
            dreamText.gameObject.SetActive(true);
            dreamText.text = "It was all a dream";
            dreamText.color = Color.white;

            // survival text
            survivalTimeText.text = finalSurvivalTime;
            survivalTimeText.color = Color.white;
        }
        else
        {
            // normal death text
            youDiedText.gameObject.SetActive(true);
            youDiedText.text = "YOU DIED";

            // hide dream text
            dreamText.gameObject.SetActive(false);
        }

        timer = 0f;

        darkeningStarted = false;

        restarting = false;

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

    IEnumerator RestartSequence()
    {
        yield return new WaitForSeconds(restartDelay);

        RestartGame();
    }

    void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}