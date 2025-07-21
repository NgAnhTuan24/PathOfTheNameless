using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryManager inventory;

    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3Int pos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            if(GameManager.instance.tileManager.IsInteractable(pos))
            {
                Debug.Log("Tile is interactable");
                GameManager.instance.tileManager.SetInteracted(pos);
            }
        }    
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
