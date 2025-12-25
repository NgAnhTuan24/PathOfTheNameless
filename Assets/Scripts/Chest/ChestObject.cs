using UnityEngine;

public class ChestObject : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] dropPrefabs;
    [SerializeField] private float dropDis = 1f;
    [SerializeField] private float dropSpacing = 1f;
    [SerializeField] private float dropForce;
    [SerializeField] private float interactDistance = 1.5f;

    [SerializeField] private ChestID id;

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

        if (id != null && ChestSaveManager.Instance != null && ChestSaveManager.Instance.IsChestOpened(id.ID))
        {
            isOpened = true;
            animator.SetTrigger("IsOpen");
        }
    }

    void Update()
    {
        if(player == null || (playerHealth != null && playerHealth.IsDead)) return;

        OpenChest();
    }

    void OpenChest()
    {
        if(isOpened) return;
        float distance = Vector2.Distance(transform.position, player.position);
        if(distance <= interactDistance && Input.GetKeyDown(KeyCode.F))
        {
            isOpened = true;
            animator.SetTrigger("IsOpen");

            if (audioSource != null && openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }

            if (id != null && ChestSaveManager.Instance != null)
            {
                ChestSaveManager.Instance.MarkChestAsOpened(id.ID);
            }

            Invoke(nameof(DropItem), 0.5f);
        }
    }

    void DropItem()
    {
        Vector2 sideDir = Vector2.Perpendicular(facingDir);

        for (int i = 0; i < dropPrefabs.Length; i++)
        {
            float offset = (i - (dropPrefabs.Length - 1) / 2f) * dropSpacing;

            Vector3 spawnPos = transform.position + (Vector3)(facingDir * dropDis) + (Vector3)(sideDir * offset);

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
                rb.AddForce(facingDir * dropForce, ForceMode2D.Impulse);
            }
        }
    }
}
