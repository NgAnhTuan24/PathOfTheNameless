using UnityEngine;
using System.Collections;

public class InvincibilitySkill : MonoBehaviour
{
    [Header("Cài đặt kỹ năng")]
    [SerializeField] private float invincibleDuration = 3f;  // thời gian bất tử
    [SerializeField] private float cooldownTime = 10f;        // thời gian hồi chiêu
    [SerializeField] private KeyCode activateKey = KeyCode.Space; // phím kích hoạt

    private bool isInvincible = false;
    private bool isCooldown = false;
    private float timer = 0f;

    private SpriteRenderer spriteRenderer;
    private PlayerController player;

    void Awake()
    {
        player = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Giảm thời gian hiệu lực hoặc hồi chiêu
        if (isInvincible)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                EndInvincibility();
        }
        else if (isCooldown)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                isCooldown = false;
        }

        // Nhấn phím để kích hoạt
        if (Input.GetKeyDown(activateKey) && !isInvincible && !isCooldown)
        {
            ActivateSkill();
        }
    }

    private void ActivateSkill()
    {
        isInvincible = true;
        timer = invincibleDuration;

        isCooldown = true;
        StartCoroutine(FlashInvincible());

        Debug.Log("Bật kỹ năng bất tử!");
    }

    private void EndInvincibility()
    {
        isInvincible = false;
        timer = cooldownTime;
        Debug.Log("Hết bất tử - bắt đầu hồi chiêu");
    }

    private IEnumerator FlashInvincible()
    {
        Color originalColor = spriteRenderer.color;
        float flashSpeed = 10f; // tốc độ nhấp nháy
        float elapsed = 0f;

        while (elapsed < invincibleDuration)
        {
            // dao động alpha kiểu sin -> nhấp nháy mượt
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed)) * 0.5f + 0.5f;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor; // reset lại
    }


    public bool IsInvincible()
    {
        return isInvincible;
    }
}
