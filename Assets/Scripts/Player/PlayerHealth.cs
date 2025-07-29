using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHeath;

    [SerializeField] private int currentHealth;

    public HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHeath;

        healthBar.SetMaxHealth(maxHeath);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log("Player nhận: " + damage + " sát thương, máu còn lại: " + currentHealth);
        healthBar.SetHealth(currentHealth);
        Die();
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Player đã chết");
            // Thực hiện các hành động khi player chết
        }
    }
}
