using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    #region
    //public static PlayerController Instance;

    [Header("Properties")]
    [SerializeField] private float tocDoDiChuyen = 5f;

    private Rigidbody2D rb;
    Animator animator;
    private Vector2 huongDiChuyen;
    private Vector2 huongHoatAnh = new Vector2(0, -1);

    // biến quản lí hành động
    [SerializeField] private float thoiGianHoiChieu = 0.5f; // delay giữa mỗi lần tấn công
    private bool isAction = false;
    private float actionTimer = 0f;

    [SerializeField] private ParticleSystem dustEffect;

    private Knockback knockback;

    public float GetMovementSpeed() => tocDoDiChuyen;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        //Instance = this;
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameEvents.ChangedStats();
    }

    public void SetMovementSpeed(float newSpeed)
    {
        tocDoDiChuyen = newSpeed;
        GameEvents.ChangedStats(); // Kích hoạt sự kiện
    }

    void Update()
    {
        huongDiChuyen = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (huongDiChuyen != Vector2.zero)
        {
            huongHoatAnh = huongDiChuyen;
        }

        if (isAction)
        {
            actionTimer -= Time.deltaTime;
            if (actionTimer <= 0f)
            {
                isAction = false;
            }
        }

        Action();
        UpdateAnimation();
    }

    void Action()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0) || isAction) return;

        var toolbarUI = GameManager.instance.uiManager.GetInventoryUI("Toolbar")?.GetComponent<Toolbar_UI>();
        var slot = toolbarUI?.GetSelectedSlot();
        if (slot == null || slot.itemName == "") return;

        var itemData = GameManager.instance.itemManager.GetItemByName(slot.itemName)?.data;
        if (itemData == null) return;

        switch (itemData.itemType)
        {
            case ItemType.kiem:
                HandleSword();
                break;

            case ItemType.congCu:
                HandleTool(itemData);
                break;

            case ItemType.hatGiong:
                HandleSeed(itemData, toolbarUI);
                break;
            case ItemType.tieuThu:
                HandleConsumable(itemData, toolbarUI);
                break;
        }
    }

    private void HandleSword()
    {
        isAction = true;
        actionTimer = thoiGianHoiChieu;
        animator.SetTrigger("IsAttacking");
        Debug.Log("đã sử dụng kiếm");
    }

    private void HandleTool(ItemData itemData)
    {
        var tileManager = GameManager.instance.tileManager;

        switch (itemData.toolType)
        {
            case ToolType.Hoe:
                if (tileManager == null || tileManager.interactableMap == null) return;
                Vector3Int pos = GameManager.instance.tileManager.interactableMap.WorldToCell(transform.position);
                HandleHoe(pos);
                break;

            case ToolType.Axe:
                HandleAxe();
                break;
        }
    }

    private void HandleHoe(Vector3Int pos)
    {
        if (GameManager.instance.tileManager.IsInteractable(pos))
        {
            isAction = true;
            actionTimer = .5f;
            animator.SetTrigger("IsHoeing");
            GameManager.instance.tileManager.TillTile(pos);
            Debug.Log("đã sử dụng cuốc");
        }
        else
        {
            Debug.Log("Không thể cuốc ở vị trí này");
        }
    }

    private void HandleAxe()
    {
        Vector2 facing = huongHoatAnh;
        Vector2 rayOrigin = transform.position;
        float rayDistance = 1f;

        Debug.DrawRay(rayOrigin, facing * rayDistance, Color.red, 0.5f);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, facing, rayDistance, LayerMask.GetMask("Tree"));

        if (hit.collider != null)
        {
            TreeObject tree = hit.collider.GetComponent<TreeObject>();
            if (tree != null)
            {
                isAction = true;
                actionTimer = .5f;
                animator.SetTrigger("IsAxeing");
                tree.Chop();
                Debug.Log("đã sử dụng rìu");
            }
        }
        else
        {
            Debug.Log("Không có cây ở phía trước để chặt");
        }
    }

    private void HandleSeed(ItemData itemData, Toolbar_UI toolbarUI)
    {
        Vector3Int pos = GameManager.instance.tileManager.interactableMap.WorldToCell(transform.position);

        if (GameManager.instance.tileManager.IsInteracted(pos) &&
            !GameManager.instance.tileManager.HasCrop(pos))
        {
            isAction = true;
            actionTimer = .5f;

            GameObject crop = Instantiate(itemData.cropPrefab, pos + new Vector3(0.5f, 0.5f), Quaternion.identity);
            GameManager.instance.tileManager.AddCrop(pos, crop);

            toolbarUI?.GetSelectedSlot()?.RemoveItem();
            GameManager.instance.uiManager.RefreshAll();
            Debug.Log("Đã gieo hạt giống");
        }
        else
        {
            Debug.Log("Không thể gieo hạt tại đây");
        }
    }

    private void HandleConsumable(ItemData itemData, Toolbar_UI toolbarUI)
    {
        isAction = true;
        actionTimer = 0.5f;

        switch (itemData.consumableType)
        {
            case ConsumableType.HealthPotion:
                GameManager.instance.player.GetComponent<PlayerHealth>().Heal((int)itemData.effectValue);
                break;
            case ConsumableType.StrengthPotion:
                StartCoroutine(ApplyStrengthBoost((int)itemData.effectValue, itemData.effectDuration));
                break;
            case ConsumableType.SpeedPotion:
                StartCoroutine(ApplySpeedBoost((int)itemData.effectValue, itemData.effectDuration));
                break;
        }

        // Xóa vật phẩm khỏi inventory sau khi sử dụng
        toolbarUI?.GetSelectedSlot()?.RemoveItem();
        GameManager.instance.uiManager.RefreshAll();
        Debug.Log($"Đã sử dụng {itemData.itemName}");
    }

    private IEnumerator ApplyStrengthBoost(int strengthAmount, float duration)
    {
        var playerDamage = GetComponentInChildren<PlayerDamage>();
        playerDamage.IncreaseDamage(strengthAmount);
        Debug.Log($"Tăng sức mạnh thêm {strengthAmount} trong {duration} giây.");

        yield return new WaitForSeconds(duration);

        playerDamage.IncreaseDamage(-strengthAmount);
        Debug.Log($"Sức mạnh trở về bình thường.");
    }

    private IEnumerator ApplySpeedBoost(int speedBoost, float duration)
    {
        IncreaseMovementSpeed(speedBoost);
        Debug.Log($"Tăng tốc độ di chuyển thêm {speedBoost} trong {duration} giây.");

        yield return new WaitForSeconds(duration);

        IncreaseMovementSpeed(-speedBoost);
        Debug.Log($"Tốc độ di chuyển trở về bình thường.");

    }

    private void FixedUpdate()
    {
        if (knockback.gettingKnockedBack) return;
        DiChuyen();
    }

    void DiChuyen()
    {
        if (isAction)
        {
            rb.velocity = Vector2.zero;
            if (dustEffect.isPlaying) dustEffect.Stop();
            return;
        }

        rb.velocity = huongDiChuyen * tocDoDiChuyen;

        DustEffect();
    }

    void DustEffect()
    {
        if(huongDiChuyen.magnitude > .1f){
            if(!dustEffect.isPlaying) dustEffect.Play();
        }
        else
        {
            if(dustEffect.isPlaying) dustEffect.Stop();
        }
    }

    void UpdateAnimation()
    {
        animator.SetFloat("LookX", huongHoatAnh.x);
        animator.SetFloat("LookY", huongHoatAnh.y);
        animator.SetFloat("Speed", huongDiChuyen.magnitude);
    }

    public void IncreaseMovementSpeed(float amount)
    {
        tocDoDiChuyen += amount;
        GameEvents.ChangedStats();
    }
}
