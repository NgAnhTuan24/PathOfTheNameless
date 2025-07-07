using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableType type;
    private void OnTriggerEnter2D(Collider2D collectable)
    {
        Player player = collectable.GetComponent<Player>();

        if (player)
        {
            player.inventory.Add(type);
            Destroy(this.gameObject);
        }
    }
}

public enum CollectableType
{
    NONE, caChua
}