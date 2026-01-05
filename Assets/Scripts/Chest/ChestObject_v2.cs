using UnityEngine;

public class ChestObject_v2 : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] dropPrefabs;
    [SerializeField] private float dropDis = 1f;
    [SerializeField] private float dropSpacing = 1f;
    [SerializeField] private float dropForce;
    [SerializeField] private float interactDistance = 1.5f;
    [SerializeField] private float destroyDelay = 2f;

    [Header("Sound")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioSource audioSource;

    private Vector2 facingDir = Vector2.down;
    private bool isOpened = false;
    private Transform player;

    private PlayerHealth playerHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (player == null || (playerHealth != null && playerHealth.IsDead)) return;

        OpenChest();
    }

    void OpenChest()
    {
        if (isOpened) return;
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.F))
        {
            isOpened = true;
            animator.SetTrigger("IsOpen");

            if (audioSource != null && openSound != null)
            {
                AudioManager.Instance.PlaySFX(openSound);
            }

            Invoke(nameof(DropItem), 0.5f);

            Destroy(gameObject, destroyDelay);
        }
    }

    void DropItem()
    {
        for (int i = 0; i < dropPrefabs.Length; i++)
        {
            Vector2 randomDir = (facingDir + Random.insideUnitCircle * dropSpacing).normalized;

            Vector3 spawnPos = transform.position + (Vector3)(randomDir * dropDis);

            GameObject obj = Instantiate(dropPrefabs[i], spawnPos, Quaternion.identity);

            GenerateID newID = obj.GetComponent<GenerateID>();
            if (newID != null)
            {
                newID.CreateID();
                ItemSaveManager.instance?.UnmarkAsRemoved(newID.GetID());
            }

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(randomDir * dropForce, ForceMode2D.Impulse);

                rb.AddTorque(Random.Range(-dropForce, dropForce) * 0.1f, ForceMode2D.Impulse);
            }
        }
    }
}
