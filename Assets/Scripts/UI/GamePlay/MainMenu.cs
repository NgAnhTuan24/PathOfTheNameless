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

    private void OnEnable()
    {
        CheckSaveGameExists();
    }

    void CheckSaveGameExists()
    {
        bool hasSave = GameSaver.HasSaveData();

        if (continueButton != null)
        {
            continueButton.interactable = hasSave;

            // Optional: đổi màu chữ khi bị tắt
            var text = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                if (hasSave)
                {
                    text.color = new Color32(0xFF, 0xA9, 0x00, 0xFF);
                }
                else
                {
                    text.color = new Color(1f, 0.66f, 0f, 0.5f);
                }
            }
        }
    }

    public void NewGame()
    {
       GameSaver.DeleteSave();

        SceneManager.LoadScene(gameSceneName);
    }

    public void ContinueGame()
    {
        if (!GameSaver.HasSaveData())
        {
            Debug.LogWarning("Không có dữ liệu save để tiếp tục!");
            return;
        }

        SaveData data = GameSaver.LoadGame();

        if (string.IsNullOrEmpty(data.currentScene))
        {
            Debug.LogError("Scene lưu bị lỗi, chuyển về New Game");
            NewGame();
            return;
        }

        PlayerPrefs.SetInt("IsContinuing", 1);
        PlayerPrefs.Save();

        // Load đúng scene đã lưu
        SceneManager.LoadScene(data.currentScene);
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