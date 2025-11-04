using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 5f;

    [Header("Detection Settings")]
    [SerializeField] float detectionRadius = 4f;
    [SerializeField] float attackRadius = 1.5f;
    [SerializeField] LayerMask playerLayer;

    [Header("Patrol Settings")]
    [SerializeField] float patrolTime = 2f;
    [SerializeField] float idleTime = 2f;

    [Header("Enemy Abilities")]
    [SerializeField] bool hasAttack = true;
    bool isAttacking = false;

    Rigidbody2D rb;
    Animator anim;
    Transform player;
    Knockback knockback;

    enum State { Idle, Patrol, Chase, Attack }
    State currentState;

    float stateTimer;
    Vector2 moveDir;
    Vector2 lastMoveDir;

    private IEnemyAttack enemyAttack;

    void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyAttack = GetComponent<IEnemyAttack>();
    }

    void Start()
    {
        currentState = State.Idle;
        stateTimer = idleTime;
    }

    void Update()
    {
        if (knockback.gettingKnockedBack) return;
        CheckState();
        HandleState();
        UpdateAnimator();
        UpdateLookDirection();
    }

    void CheckState()
    {
        if (isAttacking) return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (hit != null)
        {
            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if (ph != null && !ph.IsDead)
            {
                player = hit.transform;
                float dis = Vector2.Distance(transform.position, player.position);

                if (dis <= attackRadius && hasAttack)
                {
                    ChangeState(State.Attack);
                }
                else
                {
                    ChangeState(State.Chase);
                }
            }
            else
            {
                player = null;
                ChangeState(State.Idle);
            }
        }
        else
        {
            if (currentState == State.Chase || currentState == State.Attack)
            {
                player = null;
                ChangeState(State.Idle);
            }
        }
    }

    void HandleState()
    {
        switch (currentState)
        {
            case State.Idle:
                rb.velocity = Vector2.zero;
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    moveDir = Random.insideUnitCircle.normalized;
                    stateTimer = patrolTime;
                    ChangeState(State.Patrol);
                }
                break;

            case State.Patrol:
                rb.velocity = moveDir * walkSpeed;
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    rb.velocity = Vector2.zero;
                    stateTimer = idleTime;
                    ChangeState(State.Idle);
                }
                break;

            case State.Chase:
                if (player != null)
                {
                    Vector2 dir = (player.position - transform.position).normalized;
                    rb.velocity = dir * runSpeed;
                }
                break;

            case State.Attack:
                rb.velocity = Vector2.zero;
                if (!isAttacking && player != null)
                {
                    StartCoroutine(PerformAttack());
                }
                break;
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        enemyAttack?.Attack(player); // gọi hàm bắn đạn + trigger animation

        // Giả sử animation attack dài khoảng 1 giây
        yield return new WaitForSeconds(.5f);

        isAttacking = false;

        // Sau khi tấn công xong, kiểm tra lại player để đổi trạng thái
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= attackRadius)
                ChangeState(State.Attack);
            else if (distance <= detectionRadius)
                ChangeState(State.Chase);
            else
                ChangeState(State.Idle);
        }
        else
        {
            ChangeState(State.Idle);
        }
    }


    void ChangeState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    void UpdateAnimator()
    {
        Vector2 vel = rb.velocity;

        if (vel.sqrMagnitude > 0.01f)
        {
            lastMoveDir = vel.normalized;
            anim.SetFloat("Move X", lastMoveDir.x);
            anim.SetFloat("Move Y", lastMoveDir.y);
        }
        else
        {
            anim.SetFloat("Move X", lastMoveDir.x);
            anim.SetFloat("Move Y", lastMoveDir.y);
        }

        anim.SetFloat("Speed", vel.magnitude);
    }

    void UpdateLookDirection()
    {
        if (player == null) return; // Không có player thì khỏi update

        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        anim.SetFloat("Move X", dirToPlayer.x);
        anim.SetFloat("Move Y", dirToPlayer.y);
    }

}
