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

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        DiChuyen();
        
    }

    void DiChuyen()
    {
        rb.velocity = huongDiChuyen * tocDoDiChuyen;
    }

    void UpdateAnimation()
    {
        animator.SetFloat("LookX", huongHoatAnh.x);
        animator.SetFloat("LookY", huongHoatAnh.y);
        animator.SetFloat("Speed", huongDiChuyen.magnitude);
    }
}
