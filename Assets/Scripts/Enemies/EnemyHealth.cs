using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Enemy took damage: " + damage + ", Current Health: " + currentHealth);
        Die();
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Enemy died.");
            // Add death logic here, such as playing an animation or destroying the enemy object
            Destroy(gameObject);
        }
    }
}
