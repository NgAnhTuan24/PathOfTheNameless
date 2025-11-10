using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    private const int MAX_LEVEL = 5;
    private const int SKILL_POINTS = 5;

    private int currentLevel = 1;
    private int currentExp = 0;
    private int expToNextLevel = 100;
    private int skillPoints = 0;
    private int totalExp = 0;

    private ExpBar expBar;

    public int GetCurrentLevel() => currentLevel;
    public int GetCurrentExp() => currentExp;
    public int GetExpToNextLevel() => expToNextLevel;
    public int GetTotalExp() => totalExp;
    public int GetSkillPoints() => skillPoints;

    void Start()
    {
        AutoAssignUI();
        UpdateUI();
    }

    void AutoAssignUI()
    {
        if (UI_Manager.Instance != null && UI_Manager.Instance.uiRoot != null)
        {
            Transform uiRoot = UI_Manager.Instance.uiRoot.transform;
            var expBarTransform = uiRoot.Find("ExpBar");
            if (expBarTransform != null)
                expBar = expBarTransform.GetComponent<ExpBar>();

            if (expBar == null)
                Debug.LogWarning("Không tìm thấy ExpBar hoặc chưa gán ExpBar script!");
        }
    }

    public void AddExperience(int exp)
    {
        currentExp += exp;
        totalExp += exp;
        while (currentExp >= expToNextLevel && currentLevel < MAX_LEVEL) LevelUp();

        UpdateUI();
    }

    void LevelUp()
    {
        currentExp -= expToNextLevel;
        currentLevel++;
        skillPoints += SKILL_POINTS;
        expToNextLevel = 100 * (int)Mathf.Pow(2f, currentLevel);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (expBar != null)
        {
            expBar.SetExp(currentExp, expToNextLevel);
            expBar.SetLevel(currentLevel);
        }
    }
}
