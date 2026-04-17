using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public EnemySpawner spawner;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Health")]
    public int maxHP = 3;
    private int currentHP;

    [Header("Visual")]
    public float fadeDuration = 2f;

    private Renderer rend;
    private Material mat;

    private bool isFadingIn = true;

    void Start()
    {
        currentHP = maxHP;

        rend = GetComponent<Renderer>();
        mat = rend.material;

        // Start fully transparent
        Color color = mat.color;
        color.a = 0f;
        mat.color = color;

        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);

            Color color = mat.color;
            color.a = alpha;
            mat.color = color;

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure fully visible
        Color finalColor = mat.color;
        finalColor.a = 1f;
        mat.color = finalColor;

        isFadingIn = false;

        // Sync with HP system after fade
        UpdateTransparency();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (!isFadingIn) // 👈 prevent fighting with fade
        {
            UpdateTransparency();
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void UpdateTransparency()
    {
        float alpha = (float)currentHP / maxHP;

        Color color = mat.color;
        color.a = alpha;
        mat.color = color;
    }

    void Die()
    {
        if (spawner != null)
        {
            spawner.OnEnemyKilled();
        }

        Destroy(gameObject);
    }
}