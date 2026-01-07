using UnityEngine;

public class SlimeBullet : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float speed = 6f;
    [SerializeField] float lifeTime = 6f;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 dir)
    {
        rb.velocity = dir.normalized * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform);
            }

            Destroy(gameObject);
        }
    }
}
