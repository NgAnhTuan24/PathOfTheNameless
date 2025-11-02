using UnityEngine;

public class DummyTraining : MonoBehaviour
{
    [SerializeField] private int maxHealth = 999999;
    private int currentHealth;
    private int totalDamageTaken;
    private int lastDamage;

    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        lastDamage = damage;
        totalDamageTaken += damage;

        DamagePopup.Create(transform.position, damage, transform);
        StartCoroutine(flash.FlashRoutine());
    }
}
