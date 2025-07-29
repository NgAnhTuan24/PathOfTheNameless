using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collectable)
    {
        ItemDropper player = collectable.GetComponent<ItemDropper>();

        if (player)
        {
            Item item = GetComponent<Item>();

            if(item != null)
            {
                player.inventory.Add("Backpack", item);
                Destroy(this.gameObject);
            }
        }
    }
}