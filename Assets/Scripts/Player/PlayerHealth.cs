using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHeath;

    private int currentHealth;

    public HealthBar healthBar;

    private Knockback knockback;

    private Animator anim;
    private Rigidbody2D rb;

    public bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = maxHeath;

        if (healthBar == null)
            healthBar = UI_Manager.Instance.healthBar;

        healthBar.SetMaxHealth(maxHeath);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log("Player nhận: " + damage + " sát thương, máu còn lại: " + currentHealth);
        knockback.GetKncockBack(EnemyController.instance.transform, 15f);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        anim.SetTrigger("IsDie");

        GetComponent<PlayerController>().enabled = false;
        rb.velocity = Vector2.zero;

        EnemyController.instance.ClearTarget();

        Destroy(gameObject, 1f);
    }
}
