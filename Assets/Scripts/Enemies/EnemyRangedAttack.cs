using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Transform firePoint;

    private float cooldownTimer;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    public void Attack(Transform target)
    {
        if (cooldownTimer > 0 || target == null) return;

        cooldownTimer = attackCooldown;
        anim.SetTrigger("IsAttacking");

        Vector2 dir = (target.position - transform.position).normalized;
        FireProjectile(dir);
    }

    void FireProjectile(Vector2 dir)
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject proj = Instantiate(projectilePrefab, spawnPos, rotation);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir * projectileSpeed;
        }
    }
}
