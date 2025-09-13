using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<EnemyHealth>())
        {
            EnemyHealth enemy = col.gameObject.GetComponent<EnemyHealth>();
            enemy.TakeDamage(damageAmount);
        }
    }


}
