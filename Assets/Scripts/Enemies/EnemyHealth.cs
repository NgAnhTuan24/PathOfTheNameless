using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject deathVFX;

    private int currentHealth;


    private Knockback knockback;
    private Flash flash;

    public event Action OnEnemyDeath;

    [Header("Exp system")]
    [SerializeField] private int minExp = 10;
    [SerializeField] private int maxExp = 50;
    private PlayerLevelSystem playerLevelSystem;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();

        playerLevelSystem = FindObjectOfType<PlayerLevelSystem>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Enemy nhận: " + damage + " sát thương, máu hiện tại là: " + currentHealth);
        DamagePopup.Create(transform.position, damage, transform);

        knockback.GetKncockBack(PlayerController.Instance.transform, 15f);
        StartCoroutine(flash.FlashRoutine());
        Die();
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);

            OnEnemyDeath?.Invoke();

            if (playerLevelSystem != null)
            {
                int randomExp = UnityEngine.Random.Range(minExp, maxExp + 1);
                playerLevelSystem.AddExperience(randomExp);
            }

            Destroy(gameObject);
        }
    }
}
