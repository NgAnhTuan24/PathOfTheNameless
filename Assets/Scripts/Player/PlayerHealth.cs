using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region
    [SerializeField] private int maxHeath;

    private int currentHealth;

    public HealthBar healthBar;

    private Knockback knockback;
    private Flash flash;

    private Animator anim;
    private Rigidbody2D rb;
    private InvincibilitySkill skill;

    private bool isDead = false;

    public bool IsDead => isDead;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
        skill = GetComponent<InvincibilitySkill>();
    }

    private void Start()
    {
        currentHealth = maxHeath;

        if (healthBar == null)
            healthBar = UI_Manager.Instance.healthBar;

        healthBar.SetMaxHealth(maxHeath);
    }

    public void TakeDamage(int damage, Transform enemyTransform)
    {
        if (isDead) return;

        if (skill != null && skill.IsInvincible())
        {
            Debug.Log("Đang bất tử — không nhận sát thương!");
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        //Debug.Log("Player nhận: " + damage + " sát thương, máu còn lại: " + currentHealth);

        if (enemyTransform != null) knockback.GetKncockBack(enemyTransform, 15f);

        StartCoroutine(flash.FlashRoutine());
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

        Destroy(gameObject, .7f);
    }
}
