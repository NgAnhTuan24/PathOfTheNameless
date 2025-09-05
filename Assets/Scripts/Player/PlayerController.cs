using System.Collections;
using System.Collections.Generic;
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

    #endregion

    protected override void Awake()
    {
        base.Awake();
        //Instance = this;
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
        Vector3Int pos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

        switch (itemData.toolType)
        {
            case ToolType.Hoe:
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
        Vector3Int pos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

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
