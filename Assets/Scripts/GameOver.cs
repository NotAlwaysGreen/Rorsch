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

    [SerializeField] private Image[] ui;

    [Header("Systems")]
    [SerializeField] private PauseMenu pauseMenu;

    [SerializeField] private TextMeshProUGUI ammo;

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

    void Awake()
    {
        // invisible background
        background.color = new Color(1f, 0f, 0f, 0f);

        // invisible text
        Color text = youDiedText.color;
        text.a = 0f;
        youDiedText.color = text;
    }

    void Update()
    {
        if (!started) return;

        timer += Time.deltaTime;

        // STAGE 1 - slow red creep
        if (!darkeningStarted)
        {
            background.color = Color.Lerp(
                background.color,
                lightRed,
                lightRedSpeed * Time.deltaTime
            );
        }

        // STAGE 2 - darkening starts
        if (timer >= darkRedDelay)
        {
            darkeningStarted = true;
        }

        // STAGE 3 - dark blood red
        if (darkeningStarted)
        {
            background.color = Color.Lerp(
                background.color,
                darkRed,
                darkRedSpeed * Time.deltaTime
            );
        }

        // STAGE 4 - YOU DIED text
        if (timer >= darkRedDelay + textAppearDelay)
        {
            Color text = youDiedText.color;

            text.a = Mathf.Lerp(
                text.a,
                1f,
                textFadeSpeed * Time.deltaTime
            );

            youDiedText.color = text;

            // start restart transition once
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

        timer = 0f;

        darkeningStarted = false;

        restarting = false;

        // disable ammo UI
        if (ammo != null)
        {
            ammo.gameObject.SetActive(false);
        }

        // disable gameplay UI
        foreach (Image image in ui)
        {
            if (image != null)
            {
                image.gameObject.SetActive(false);
            }
        }

        // disable pause menu
        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }
    }

    IEnumerator RestartSequence()
    {
        yield return new WaitForSeconds(restartDelay);

        Color startColor = background.color;

        Color endColor = new Color(
            0f,
            0f,
            0f,
            1f
        );

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * finalFadeSpeed;

            background.color = Color.Lerp(
                startColor,
                endColor,
                t
            );

            yield return null;
        }

        RestartGame();
    }

    void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}