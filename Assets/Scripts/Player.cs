using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory(20);
    }

    public void DropItem(Collectable item)
    {
        Vector2 spawnLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle * 1.25f;

        Collectable droppedItem = Instantiate(
            item, 
            spawnLocation + spawnOffset, 
            Quaternion.identity);

        droppedItem.rb.AddForce(
            spawnOffset * 2f, ForceMode2D.Impulse);
    }

    public void DropItem(Collectable item, int numToDrop)
    {
       for(int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}
