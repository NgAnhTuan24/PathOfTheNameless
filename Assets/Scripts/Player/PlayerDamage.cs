using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var enemy = col.GetComponent<EnemyHealth>();
        var dummy = col.GetComponent<DummyTraining>();

        if (enemy != null)
            enemy.TakeDamage(damageAmount);
        else if (dummy != null)
            dummy.TakeDamage(damageAmount);
    }


}
