using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("Text Fields")]
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text defenseText;
    public TMP_Text speedText;
    public TMP_Text skillPointText;
    public TMP_Text levelText;
    public TMP_Text expText;

    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private PlayerDamage playerDamage;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerController = player.GetComponent<PlayerController>();
            playerDamage = player.GetComponentInChildren<PlayerDamage>();
        }
        else
        {
            Debug.LogError("Player GameObject not found. Ensure it has the 'Player' tag.");
        }
    }

    private void Start()
    {
        Refresh();
    }

    void Refresh()
    {
        if (playerHealth != null)
        {
            // Get current and max health/armor from PlayerHealth
            healthText.text = $"Máu: {playerHealth.GetCurrentHealth()}/{playerHealth.GetMaxHealth()}";
            defenseText.text = $"Giáp: {playerHealth.GetCurrentArmor()}/{playerHealth.GetMaxArmor()}";
        }
        else
        {
            healthText.text = "Máu: (Không tìm thấy)";
            defenseText.text = "Giáp: (Không tìm thấy)";
        }

        if (playerDamage != null)
        {
            // Get damage from PlayerDamage
            damageText.text = $"Sát thương: {playerDamage.GetDamageAmount()}";
        }
        else
        {
            damageText.text = "Sát thương: (Không tìm thấy)";
        }

        if (playerController != null)
        {
            // Get movement speed from PlayerController
            speedText.text = $"Tốc độ di chuyển: {playerController.GetMovementSpeed()}";
        }
        else
        {
            speedText.text = "Tốc độ di chuyển: (Không tìm thấy)";
        }

        skillPointText.text = "Điểm nâng kỹ năng: (đang chưa có)";
        levelText.text = "Cấp: (đang chưa có)";
        expText.text = "Kinh nghiệm: (đang chưa có)";
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
            Refresh();
    }
}
