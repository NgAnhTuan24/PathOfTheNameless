using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject,lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerHealth>();
            if(player != null)
            {
                player.TakeDamage(damage, transform);
            }
            Destroy(gameObject);
        }
    }
}
