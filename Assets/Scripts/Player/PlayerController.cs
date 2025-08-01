using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region
    public static PlayerController Instance;

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

    #endregion

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        // Tấn công bằng kiếm
        if (itemData.itemType == ItemType.kiem)
        {
            if (!isAction)
            {
                isAction = true;
                actionTimer = thoiGianHoiChieu;
                animator.SetTrigger("IsAttacking");
                Debug.Log("đã sử dụng kiếm");
            }
        }

        // Xử lý công cụ (cuốc, rìu, v.v.)
        else if (itemData.itemType == ItemType.congCu)
        {
            Vector3Int pos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            if (itemData.toolType == ToolType.Hoe)
            {
                if (GameManager.instance.tileManager.IsInteractable(pos))
                {
                    isAction = true;
                    actionTimer = .5f;
                    animator.SetTrigger("IsHoeing");
                    //GameManager.instance.tileManager.SetInteracted(pos);
                    GameManager.instance.tileManager.TillTile(pos);
                    Debug.Log("đã sử dụng cuốc");
                }
                else
                {
                    Debug.Log("Không thể cuốc ở vị trí này");
                }
            }
            else if (itemData.toolType == ToolType.Axe)
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
                        tree.Chop(); // Gọi chặt cây
                        Debug.Log("đã sử dụng rìu");
                    }
                }
                else
                {
                    Debug.Log("Không có cây ở phía trước để chặt");
                }
            }
        }

        // Trồng hạt giống
        else if (itemData.itemType == ItemType.hatGiong)
        {
            Vector3Int pos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            // Kiểm tra nếu ô đất đã được cuốc
            if (GameManager.instance.tileManager.IsInteracted(pos))
            {
                // Kiểm tra đã có cây trồng chưa (tránh trồng chồng lên)
                if (!GameManager.instance.tileManager.HasCrop(pos))
                {
                    isAction = true;
                    actionTimer = .5f;

                    // Gieo hạt (tạo cây)
                    GameObject crop = Instantiate(itemData.cropPrefab, pos + new Vector3(0.5f, 0.5f), Quaternion.identity);
                    GameManager.instance.tileManager.AddCrop(pos, crop);

                    // Giảm số lượng hạt giống
                    toolbarUI?.GetSelectedSlot()?.RemoveItem();
                    GameManager.instance.uiManager.RefreshAll();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        DiChuyen();
    }

    void DiChuyen()
    {
        if (isAction)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = huongDiChuyen * tocDoDiChuyen;
    }

    void UpdateAnimation()
    {
        animator.SetFloat("LookX", huongHoatAnh.x);
        animator.SetFloat("LookY", huongHoatAnh.y);
        animator.SetFloat("Speed", huongDiChuyen.magnitude);
    }
}
