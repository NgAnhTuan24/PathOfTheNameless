using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.GetComponent<PlayerHealth>())
        {
            PlayerHealth pl = col.gameObject.GetComponent<PlayerHealth>();

            if (pl != null)
            {
                pl.TakeDamage(damageAmount, transform);
            }
        }
    }
}
