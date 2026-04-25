using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Insanity")]
    public float insanityIncrease;
    [Header("Vignette Effect")]
    private VignetteController vignetteController;

    [Header("Randomization")]
    public float minSpeed = 6f;
    public float maxSpeed = 18f;

    public float minScale = 200f;
    public float maxScale = 1000f;

    [Header("References")]
    public Transform player;
    public EnemySpawner spawner;
    public InsaneBar insaneBar;

    [Header("Health")]
    public int maxHP = 1;
    private int currentHP;

    [Header("Visual")]
    public float fadeDuration = 1f;
    public float fadeOutDuration = 0.2f;

    private float moveSpeed;

    private Renderer rend;
    private Material mat;

    private bool isDying = false;
    private Coroutine fadeCoroutine;

    void Start()
    {
        GameObject vignetteObj = GameObject.FindGameObjectWithTag("Vignette");

        if (vignetteObj != null)
        {
            vignetteController = vignetteObj.GetComponent<VignetteController>();
        }

        rend = GetComponent<Renderer>();
        mat = rend.material;

        // Random scale
        float randomScale = Random.Range(minScale, maxScale);
        transform.localScale = Vector3.one * randomScale;

        // Random speed
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        // HP (simple, fixed)
        currentHP = maxHP;

        // Start invisible
        SetAlpha(0f);

        // Fade in
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (player == null || isDying) return;

        Vector3 direction = player.position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-90f, 0f, 0f);
        }

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        killWhenPlayerClose();
    }

        private void killWhenPlayerClose()
    {
        if (Vector3.Distance(transform.position, player.position) < 10f)
        {
            if (vignetteController != null)
            {
                vignetteController.PlayEffect();
                insaneBar.AddInsanity(insanityIncrease);
            }

            Die();
        }
    }

    // ---------------- FADE IN ----------------
    IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            if (isDying) yield break;

            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            SetAlpha(alpha);

            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(1f);
        fadeCoroutine = null;
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if (isDying) return;

        if (collision.gameObject.CompareTag("Bullet"))
        {
            Die();
        }
    }
  
    void Die()
    {
        if (isDying) return;
        isDying = true;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        if (spawner != null)
        {
            spawner.OnEnemyKilled();
        }

        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        float time = 0f;
        float startAlpha = mat.color.a;

        while (time < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, 0f, time / fadeOutDuration);
            SetAlpha(alpha);

            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);
        Destroy(gameObject);
    }

    // ---------------- HELPERS ----------------
    void SetAlpha(float alpha)
    {
        Color color = mat.color;
        color.a = alpha;
        mat.color = color;
    }
}