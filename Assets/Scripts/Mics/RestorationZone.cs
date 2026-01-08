using UnityEngine;
using System.Collections;

public class RestorationZone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int healthRecoveryAmount = 1;
    [SerializeField] private int armorRecoveryAmount = 1;
    [SerializeField] private float tickRate = 1.0f;

    private Coroutine recoveryCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                recoveryCoroutine = StartCoroutine(RecoverRoutine(playerHealth));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (recoveryCoroutine != null)
            {
                StopCoroutine(recoveryCoroutine);
                recoveryCoroutine = null;
            }
        }
    }

    private IEnumerator RecoverRoutine(PlayerHealth player)
    {
        while (true)
        {
            player.Heal(healthRecoveryAmount);

            RestoreArmor(player, armorRecoveryAmount);

            yield return new WaitForSeconds(tickRate);
        }
    }

    private void RestoreArmor(PlayerHealth player, int amount)
    {
        int current = player.GetCurrentArmor();
        int max = player.GetMaxArmor();

        if (current < max)
        {
            player.RestoreArmor(amount);
        }
    }
}