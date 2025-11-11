using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Upgrade Buttons")]
    public Button healthButton;
    public Button defenseButton;
    public Button damageButton;
    public Button speedButton;

    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private PlayerDamage playerDamage;
    private PlayerLevelSystem playerLevelSystem;

    private void Awake()
    {
        // Kiểm tra các thành phần giao diện
        if (!healthText || !damageText || !defenseText || !speedText || !skillPointText || !levelText || !expText || !healthButton || !defenseButton || !damageButton || !speedButton)
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

        healthButton.onClick.AddListener(() => UpgradeStat("health", 5, 1)); // Tăng 10 máu, tốn 1 điểm
        defenseButton.onClick.AddListener(() => UpgradeStat("defense", 1, 1)); // Tăng 5 giáp
        damageButton.onClick.AddListener(() => UpgradeStat("damage", 1, 1)); // Tăng 5 sát thương
        speedButton.onClick.AddListener(() => UpgradeStat("speed", 1, 1)); // Tăng 1 tốc độ

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
            damageText.text = $"Sát thương: {playerDamage.GetDamageAmount()}";
        }
        else
        {
            damageText.text = "Sát thương: (Không tìm thấy)";
        }

        if (playerController != null && speedText != null)
        {
            speedText.text = $"Tốc độ: {playerController.GetMovementSpeed()}";
        }
        else
        {
            speedText.text = "Tốc độ: (Không tìm thấy)";
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

        UpdateUpgradeButtons();
    }

    void UpgradeStat(string statType, float amount, int skillCost)
    {
        if (playerLevelSystem == null || playerLevelSystem.GetSkillPoints() < skillCost) return;

        switch (statType)
        {
            case "health":
                playerHealth.IncreaseMaxHealth(amount);
                break;
            case "damage":
                playerDamage.IncreaseDamage(amount);
                break;
            case "defense":
                playerHealth.IncreaseMaxArmor(amount);
                break;
            case "speed":
                playerController.IncreaseMovementSpeed(amount);
                break;
        }

        playerLevelSystem.SpendSkillPoints(skillCost);
        Refresh();
    }

    void UpdateUpgradeButtons()
    {
        int skillPoints = playerLevelSystem?.GetSkillPoints() ?? 0;
        healthButton.gameObject.SetActive(skillPoints > 0);
        defenseButton.gameObject.SetActive(skillPoints > 0);
        damageButton.gameObject.SetActive(skillPoints > 0);
        speedButton.gameObject.SetActive(skillPoints > 0);
    }

    public void Toggle()
    {
        if (UI_Manager.Instance != null && UI_Manager.Instance.IsInventoryOpen) UI_Manager.Instance.OpenCloseInventoryUI();

        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
            Refresh();
    }
}
