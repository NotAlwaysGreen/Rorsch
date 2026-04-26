using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI youDiedText;
    [SerializeField] private Image[] ui;

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

    private bool started = false;

    private float timer = 0f;

    private bool darkeningStarted = false;

    void Awake()
    {
        background.color = new Color(1f, 0f, 0f, 0f);

        Color text = youDiedText.color;
        text.a = 0f;
        youDiedText.color = text;
    }

    void Update()
    {
        if (!started) return;

        timer += Time.deltaTime;

        // STAGE 1
        if (!darkeningStarted)
        {
            background.color = Color.Lerp(
                background.color,
                lightRed,
                lightRedSpeed * Time.deltaTime
            );
        }

        // STAGE 2 -> start darkening
        if (timer >= darkRedDelay)
        {
            darkeningStarted = true;
        }

        // STAGE 3
        if (darkeningStarted)
        {
            background.color = Color.Lerp(
                background.color,
                darkRed,
                darkRedSpeed * Time.deltaTime
            );
        }

        // STAGE 4
        if (timer >= darkRedDelay + textAppearDelay)
        {
            Color text = youDiedText.color;

            text.a = Mathf.Lerp(
                text.a,
                1f,
                textFadeSpeed * Time.deltaTime
            );

            youDiedText.color = text;
        }
    }

    public void StartGameOver()
    {
        started = true;

        timer = 0f;

        darkeningStarted = false;

        // disable UI immediately
        foreach (Image image in ui)
        {
            if (image != null)
            {
                image.gameObject.SetActive(false);
            }
        }
    }
}