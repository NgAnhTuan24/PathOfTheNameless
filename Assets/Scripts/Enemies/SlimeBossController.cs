using UnityEngine;
public class SlimeBossController : MonoBehaviour { 
    [Header("Move Settings")]
    [SerializeField] float chaseSpeed = 5f;
    [SerializeField] float idleTime = 2f;

    [Header("Detection Settings")]
    [SerializeField] float detectionRadius = 4f; 
    [SerializeField] float attackRadius = 1.5f; 
    [SerializeField] LayerMask playerLayer;

    [Header("Attack Settings")]
    [SerializeField] float attackCooldown = 2f;
    float attackCooldownTimer;

    [Header("Projectile Attack")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletCount = 8;
    [SerializeField] float bulletSpawnRadius = 0.3f;

    Rigidbody2D rb; 
    Animator anim; 
    Transform player; 
    Knockback knockback; 
    
    enum State { Idle, Chase, Attack } 
    State currentState;

    float stateTimer;
    bool isAttacking;

    void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        currentState = State.Idle;
        stateTimer = idleTime;
    }

    void Update()
    {
        if (knockback.gettingKnockedBack) return;
        
        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;

        DetectPlayer();

        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;

            case State.Chase:
                ChaseState();
                break;

            case State.Attack:
                AttackState();
                break;
        }
    }

    void ShootRadialProjectiles()
    {
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            Vector2 spawnPos = (Vector2)transform.position + dir * bulletSpawnRadius;

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            bullet.GetComponent<SlimeBullet>().Init(dir);

            angle += angleStep;
        }
    }

    public void OnAttackShoot()
    {
        ShootRadialProjectiles();
    }

    void IdleState()
    {
        rb.velocity = Vector2.zero;
        anim.SetBool("IsChasing", false);

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0 && player != null && attackCooldownTimer <= 0)
        {
            ChangeState(State.Chase);
        }
    }

    void ChaseState()
    {
        anim.SetBool("IsChasing", true);

        if (player == null)
        {
            ChangeState(State.Idle);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRadius && attackCooldownTimer <= 0)
        {
            ChangeState(State.Attack);
            return;
        }

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * chaseSpeed;
    }

    void AttackState()
    {
        rb.velocity = Vector2.zero;

        if (!isAttacking)
        {
            isAttacking = true;
            PerformAttack();
        }
    }

    void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (hit != null)
        {
            player = hit.transform;
        }
        else
        {
            player = null;
        }
    }

    void PerformAttack()
    {
        if (player == null) return;

        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        if (Mathf.Abs(dirToPlayer.x) < 0.3f)
        {
            anim.SetTrigger("Attack");
        }
        else if (dirToPlayer.x < 0)
        {
            anim.SetTrigger("AttackLeft");
        }
        else
        {
            anim.SetTrigger("AttackRight");
        }
    }


    public void EndAttack()
    {
        isAttacking = false;
        attackCooldownTimer = attackCooldown;
        ChangeState(State.Chase);
    }

    void ChangeState(State newState)
    {
        currentState = newState;

        if (newState == State.Idle)
            stateTimer = idleTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}