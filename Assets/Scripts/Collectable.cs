using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collectable)
    {
        Player player = collectable.GetComponent<Player>();

        if (player)
        {
            player.count++;
            Destroy(this.gameObject);
        }
    }
}
