using UnityEngine;

public class Crop : MonoBehaviour
{
    public int growStage = 0;
    public float growTime = 10f; // Thời gian mỗi giai đoạn
    public Sprite[] growSprites;
    private float timer;
    private SpriteRenderer spriteRenderer;

    [Header("Harvest")]
    public string harvestItemName; // tên vật phẩm thu được (trùng với itemData)
    public int yieldAmount = 1;    // số lượng thu hoạch

    private bool isFullyGrown => growStage == growSprites.Length - 1;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // gán spriteRenderer đúng cách

        if (growSprites.Length > 0)
        {
            spriteRenderer.sprite = growSprites[growStage];
        }

        timer = growTime;
    }

    void Update()
    {
        if (growStage < growSprites.Length - 1)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                growStage++;
                spriteRenderer.sprite = growSprites[growStage];
                timer = growTime;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isFullyGrown) return;

        // Tìm itemData từ tên
        Item itemToAdd = GameManager.instance.itemManager.GetItemByName(harvestItemName);
        if (itemToAdd != null)
        {
            for (int i = 0; i < yieldAmount; i++)
            {
                GameManager.instance.player.inventory.Add("Backpack", itemToAdd);
            }

            // ✅ Reset nền đất
            Vector3 worldPos = transform.position;
            Vector3Int gridPos = new Vector3Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), 0);
            GameManager.instance.tileManager.ResetTile(gridPos);

            // ✅ Xoá cây
            Destroy(gameObject);
        }
    }
}
