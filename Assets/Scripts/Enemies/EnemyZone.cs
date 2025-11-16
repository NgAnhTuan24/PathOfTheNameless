using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyZone : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.OnExitZone(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.OnEnterZone(this);
        }
    }
}
