using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float tocDoDiChuyen;

    private Vector2 huongDiChuyen;
    private Rigidbody2D rb;
    private Knockback knockback;
    private Animator animator;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (knockback.gettingKncokedBack) { return; }

        rb.MovePosition(rb.position + huongDiChuyen * (tocDoDiChuyen * Time.fixedDeltaTime));

        if (huongDiChuyen != Vector2.zero)
        {
            animator.SetFloat("Move X", huongDiChuyen.x);
            animator.SetFloat("Move Y", huongDiChuyen.y);
        }
    }

    public void DiChuyenDen(Vector2 viTriDich)
    {
        huongDiChuyen =  viTriDich;
    }
}
