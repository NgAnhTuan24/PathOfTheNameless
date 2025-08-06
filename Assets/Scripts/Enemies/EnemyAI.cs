using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float doiHuongDiChuyen;

    [SerializeField] private float tamPhatHien;
    [SerializeField] private LayerMask playerLayer;
    
    private enum TrangThai 
    {
        DiChuyenNgauNhien,
        DuoiTheo,
    }

    private TrangThai trangThai;
    private EnemyPathfinding enemyPathfinding;
    private Coroutine roamingCoroutine;
    private Transform playerTarget;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        trangThai = TrangThai.DiChuyenNgauNhien;
    }

    private void Start()
    {
        roamingCoroutine = StartCoroutine(RoamingRoutine());
    }

    private void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, tamPhatHien, playerLayer);

        if (hit != null)
        {
            if (trangThai != TrangThai.DuoiTheo)
            {
                playerTarget = hit.transform;
                trangThai = TrangThai.DuoiTheo;

                if (roamingCoroutine != null)
                    StopCoroutine(roamingCoroutine);
            }

            // Cập nhật hướng đuổi theo player
            Vector2 huong = (playerTarget.position - transform.position).normalized;
            enemyPathfinding.DiChuyenDen(huong);
        }
        else
        {
            if (trangThai != TrangThai.DiChuyenNgauNhien)
            {
                playerTarget = null;
                trangThai = TrangThai.DiChuyenNgauNhien;
                roamingCoroutine = StartCoroutine(RoamingRoutine());
            }
        }
    }

    private IEnumerator RoamingRoutine()
    {
        while (trangThai == TrangThai.DiChuyenNgauNhien)
        {
           DiChuyenNgauNhien();
            yield return new WaitForSeconds(doiHuongDiChuyen);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void DiChuyenNgauNhien()
    {
        Vector2 huongNgauNhien = GetRoamingPosition();
        enemyPathfinding.DiChuyenDen(huongNgauNhien);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DiChuyenNgauNhien();
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ vòng tròn phát hiện player trong Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tamPhatHien);
    }
}
