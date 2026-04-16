using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;

    public int maxHP = 3;
    private int currentHP;

    private Renderer rend;
    private Material mat;

    void Start()
    {
        currentHP = maxHP;

        rend = GetComponent<Renderer>();
        mat = rend.material; // IMPORTANT: creates instance (not shared)
        
        UpdateTransparency();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation =
                Quaternion.LookRotation(direction) *
                Quaternion.Euler(90f, 0f, 0f);
        }

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
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

        UpdateTransparency();

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
        Destroy(gameObject);
    }
}