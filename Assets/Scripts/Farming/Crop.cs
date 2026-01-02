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

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip harvestSound;
    [SerializeField] private float harvestVolume = 1f;

    private bool isFullyGrown => growStage == growSprites.Length - 1;

    private Vector3Int gridPos;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // gán spriteRenderer đúng cách

        if (growSprites.Length > 0)
        {
            spriteRenderer.sprite = growSprites[growStage];
        }

        timer = growTime;

        gridPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.volume = harvestVolume;

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

        TileManager.Instance.UpdateCropSaveData(gridPos, growStage, timer);
    }

    private void OnMouseDown()
    {
        if (!isFullyGrown) return;

        Item itemToAdd = GameManager.instance.itemManager.GetItemByName(harvestItemName);
        if (itemToAdd == null) return;

        for (int i = 0; i < yieldAmount; i++)
        {
            GameManager.instance.player.inventory.Add("Backpack", itemToAdd);
        }

        Vector3 worldPos = transform.position;
        Vector3Int gridPos = new Vector3Int(
            Mathf.FloorToInt(worldPos.x),
            Mathf.FloorToInt(worldPos.y),
            0
        );

        TileManager.Instance.savedCrops.RemoveAll(c => c.position == gridPos);

        audioSource.PlayOneShot(harvestSound);

        // Ẩn cây ngay lập tức
        spriteRenderer.enabled = false;

        // Reset đất
        GameManager.instance.tileManager.ResetTile(gridPos);

        // Destroy trễ để âm thanh phát xong
        Destroy(gameObject, harvestSound.length);

    }

    public void Load(int stage, float remainingTime)
    {
        growStage = stage;
        timer = remainingTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = growSprites[growStage];
    }

    public float GetRemainingTime()
    {
        return timer;
    }

}
