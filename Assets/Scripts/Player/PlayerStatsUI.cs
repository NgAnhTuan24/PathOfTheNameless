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
    private PlayerLevelSystem playerLevelSystem;

    private void Awake()
    {
        // Kiểm tra các thành phần giao diện
        if (!healthText || !damageText || !defenseText || !speedText || !skillPointText || !levelText || !expText)
        {
            Debug.LogError("Một hoặc nhiều thành phần TextMeshProUGUI chưa được gán trong PlayerStatsUI.");
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerController = player.GetComponent<PlayerController>();
            playerDamage = player.GetComponentInChildren<PlayerDamage>();
            playerLevelSystem = player.GetComponent<PlayerLevelSystem>();
        }
        else
        {
            Debug.LogError("Player GameObject not found. Ensure it has the 'Player' tag.");
        }

        GameEvents.OnChangedStats += Refresh;
    }

    private void OnDestroy()
    {
        GameEvents.OnChangedStats -= Refresh;
    }

    private void Start()
    {
        Refresh();
    }

    void Refresh()
    {
        if (playerHealth != null && healthText != null && defenseText != null)
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

        if (playerDamage != null && damageText != null)
        {
            // Get damage from PlayerDamage
            damageText.text = $"Sát thương: {playerDamage.GetDamageAmount()}";
        }
        else
        {
            damageText.text = "Sát thương: (Không tìm thấy)";
        }

        if (playerController != null && speedText != null)
        {
            // Get movement speed from PlayerController
            speedText.text = $"Tốc độ di chuyển: {playerController.GetMovementSpeed()}";
        }
        else
        {
            speedText.text = "Tốc độ di chuyển: (Không tìm thấy)";
        }

        if (playerLevelSystem != null && levelText != null && expText != null && skillPointText != null)
        {
            levelText.text = $"Cấp: {playerLevelSystem.GetCurrentLevel()}";
            expText.text = $"Kinh nghiệm: {playerLevelSystem.GetTotalExp()}";
            skillPointText.text = $"Điểm kỹ năng: {playerLevelSystem.GetSkillPoints()}";
        }
        else
        {
            levelText.text = "Cấp: (Không tìm thấy)";
            expText.text = "Kinh nghiệm: (Không tìm thấy)";
            skillPointText.text = "Điểm kỹ năng: (Không tìm thấy)";
        }
    }

    public void Toggle()
    {
        if (UI_Manager.Instance != null && UI_Manager.Instance.IsInventoryOpen) UI_Manager.Instance.OpenCloseInventoryUI();

        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
            Refresh();
    }
}
