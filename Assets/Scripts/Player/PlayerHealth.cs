using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region
    [SerializeField] private int maxHeath;
    private int currentHealth;

    [SerializeField] private int maxArmor;
    private int currentArmor;

    public HealthBar healthBar;
    public ArmorBar armorBar;

    private Knockback knockback;
    private Flash flash;
    private InvincibilitySkill skill;

    private Animator anim;
    private Rigidbody2D rb;

    private bool isDead = false;

    public bool IsDead => isDead;

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHeath;
    public int GetCurrentArmor() => currentArmor;
    public int GetMaxArmor() => maxArmor;
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
        currentArmor = maxArmor;

        if (healthBar == null)
            healthBar = UI_Manager.Instance.healthBar;
        if (armorBar == null)
            armorBar = UI_Manager.Instance.armorBar;

        healthBar.SetMaxHealth(maxHeath);
        armorBar.SetMaxArmor(maxArmor);
    }

    public void TakeDamage(int damage, Transform enemyTransform)
    {
        if (isDead) return;

        if (skill != null && skill.IsInvincible())
        {
            Debug.Log("Đang bất tử — không nhận sát thương!");
            return;
        }

        if (currentArmor > 0)
        {
            int armorDamage = Mathf.Min(damage, currentArmor);
            currentArmor -= armorDamage;
            damage -= armorDamage;
            armorBar.SetArmor(currentArmor);
        }

        if (damage > 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0);
            healthBar.SetHealth(currentHealth);
            StartCoroutine(flash.FlashRoutine());
            if (enemyTransform != null) knockback.GetKncockBack(enemyTransform, 15f);
            if (currentHealth <= 0) Die();
        }

        //Debug.Log("Player nhận: " + damage + " sát thương, máu còn lại: " + currentHealth);



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
