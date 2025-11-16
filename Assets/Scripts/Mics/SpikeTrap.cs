using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private float activeTime; // thời gian gai bật
    private int damage = 1;

    private Animator spikeAnimator;
    private bool isTriggered;
    private bool isActive = false;

    private void Start()
    {
        spikeAnimator = GetComponent<Animator>();
        spikeAnimator.SetBool("IsActive", isActive);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(ActivateTrap(other));
        }
    }

    private IEnumerator ActivateTrap(Collider2D player)
    {
        isTriggered = true;

        // Bật gai
        isActive = true;
        spikeAnimator.SetBool("IsActive", true);

        yield return new WaitForSeconds(0.2f); // Delay nhỏ cho hiệu ứng đâm

        if (player != null && player.CompareTag("Player"))
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(damage, transform);
        }

        // Giữ gai bật
        yield return new WaitForSeconds(activeTime);

        // Thu lại
        isActive = false;
        spikeAnimator.SetBool("IsActive", false);

        yield return new WaitForSeconds(0.5f); // chờ trước khi cho phép kích hoạt lại
        isTriggered = false;
    }
}
