using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    public ArenaController arenaController;
    private bool activated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            arenaController.StartArena();
        }
    }
}
