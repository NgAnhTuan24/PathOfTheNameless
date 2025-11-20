using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Game Scene Name")]
    [SerializeField] private string gameSceneName;

    [Header("Button Main Menu")]
    public Button newGameButton;
    public Button continueButton;
    public Button exitButton;

    private void Awake()
    {
        // Gắn sự kiện cho các nút
        if (newGameButton) newGameButton.onClick.AddListener(NewGame);
        if (continueButton) continueButton.onClick.AddListener(ContinueGame);
        if (exitButton) exitButton.onClick.AddListener(ExitGame);

        // Kiểm tra xem có save game không để bật/tắt nút Continue
        CheckSaveGameExists();
    }

    void CheckSaveGameExists()
    {
        bool hasSave = PlayerPrefs.HasKey("PlayerSaveData") || System.IO.File.Exists(Application.persistentDataPath + "/save.gamesave");
        // Thay dòng trên tùy cách bạn lưu game (PlayerPrefs, JSON, binary...)

        if (continueButton != null)
        {
            continueButton.interactable = hasSave; // tắt nút nếu chưa có save

            // Optional: đổi màu chữ khi bị tắt
            var text = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.color = hasSave ? Color.white : new Color(1, 1, 1, 0.3f);
        }
    }

    public void NewGame()
    {
        // Xóa save cũ (nếu muốn bắt đầu lại hoàn toàn)
        PlayerPrefs.DeleteAll();
        // hoặc xóa file save tùy cách bạn lưu

        SceneManager.LoadScene(gameSceneName);
    }

    public void ContinueGame()
    {
        // Load thẳng scene game, trong scene game bạn sẽ tự load dữ liệu save
        SceneManager.LoadScene(gameSceneName);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}