using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    public ArenaController arenaController;
    private bool activated;
    private ArenaID arenaID;

    private void Awake()
    {
        arenaID = GetComponent<ArenaID>();
    }

    private void Start()
    {
        if (arenaID != null &&
            ArenaSaveManager.Instance != null &&
            ArenaSaveManager.Instance.IsArenaActivated(arenaID.GetID()))
        {
            activated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            ArenaSaveManager.Instance?.MarkActivated(arenaID.GetID());

            arenaController.StartArena();
        }
    }
}
