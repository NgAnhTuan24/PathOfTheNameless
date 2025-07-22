using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region

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

    void Start()
    {
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
                    GameManager.instance.tileManager.SetInteracted(pos);
                }
            }
            else if (itemData.toolType == ToolType.Axe)
            {
                Debug.Log("đã sử dụng rìu");
            }
        }

        // Trồng hạt giống
        else if (itemData.itemType == ItemType.hatGiong)
        {

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
