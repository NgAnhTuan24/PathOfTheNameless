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
    public TMP_Text knockBackResistText;
    public TMP_Text skillPointText;
    public TMP_Text levelText;
    public TMP_Text expText;

    private void Start()
    {
        Refresh();
    }

    void Refresh()
    {
        healthText.text = "Máu: (đang chưa có)";
        defenseText.text = "Giáp: (đang chưa có)";
        damageText.text = "Sát thương: (đang chưa có)";
        speedText.text = "Tốc độ di chuyển: (đang chưa có)";
        knockBackResistText.text = "Kháng đẩy lùi: (đang chưa có)";
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
