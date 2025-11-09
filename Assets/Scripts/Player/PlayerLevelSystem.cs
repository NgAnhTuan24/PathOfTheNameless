using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    private int currentLevel = 1;
    private float currentExp = 0f;
    private float expToNextLevel = 100f;

    private ExpBar expBar;

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

    public void AddExperience(float exp)
    {
        currentExp += exp;
        while (currentExp >= expToNextLevel)
            LevelUp();

        UpdateUI();
    }

    void LevelUp()
    {
        currentExp -= expToNextLevel;
        currentLevel++;
        expToNextLevel *= 1.5f;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddExperience(50f);
            Debug.Log("Đã cộng 50 EXP!");
        }
    }
}
