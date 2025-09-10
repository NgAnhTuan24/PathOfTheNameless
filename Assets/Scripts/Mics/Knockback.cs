using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool gettingKnockedBack
    {
        get; private set;
    }

    [SerializeField] private float timeKnock;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKncockBack(Transform dameSource, float knockBackThrust)
    {
        gettingKnockedBack = true;
        Vector2 diference = (transform.position - dameSource.position).normalized * knockBackThrust * rb.mass;
        rb.AddForce(diference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(timeKnock);
        rb.velocity = Vector2.zero;
        gettingKnockedBack = false;
    }
}
