using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerHealth>())
        {
            PlayerHealth pl = col.gameObject.GetComponent<PlayerHealth>();
            pl.TakeDamage(damageAmount);
        }
    }


}
