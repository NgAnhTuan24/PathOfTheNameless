using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

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

    Rigidbody2D rb;
    Animator anim;
    Transform player;
    Knockback knockback;

    enum State { Idle, Patrol, Chase, Attack }
    State currentState;

    float stateTimer;
    Vector2 moveDir;

    void Awake()
    {
        instance = this;
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
        CheckState();
        HandleState();
        UpdateAnimator();
    }

    void CheckState()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (hit != null)
        {
            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if (ph != null && !ph.isDead)
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
                if (hasAttack) anim.SetTrigger("IsAttacking");
                break;
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

    public void ClearTarget()
    {
        player = null;
        ChangeState(State.Idle);
    }

    void UpdateAnimator()
    {
        Vector2 vel = rb.velocity;
        anim.SetFloat("Move X", vel.x);
        anim.SetFloat("Move Y", vel.y);
        anim.SetFloat("Speed", vel.magnitude);
    }
}
