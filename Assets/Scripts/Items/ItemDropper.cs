using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public InventoryManager inventory;

    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();
    }

    public void DropItem(Item item)
    {
        Vector2 spawnLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle * 1.25f;

        Item droppedItem = Instantiate(
            item, 
            spawnLocation + spawnOffset, 
            Quaternion.identity);

        droppedItem.rb.AddForce(
            spawnOffset * 2f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
       for(int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}
