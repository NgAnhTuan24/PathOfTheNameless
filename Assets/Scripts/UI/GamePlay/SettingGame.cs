using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingGame : MonoBehaviour
{
    [Header("Setting Game")]
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button soundFXBtn;

    [SerializeField] private Button returnBtn;
    [SerializeField] private Button fullScreenBtn;
    [SerializeField] private Button homeBtn;

    private bool isFullScreen;

    private void Awake()
    {
        setupSoundBtn();
        setupGameBtn();
    }

    void setupSoundBtn()
    {
        var soundSetting = transform.Find("SoundSettings");
        if (soundSetting == null)
        {
            Debug.Log("Không tìm thấy SoundSettings"); return;
        }
        musicBtn = soundSetting.Find("Music")?.GetComponent<Button>();
        soundFXBtn = soundSetting.Find("SoundFX")?.GetComponent<Button>();
        if (musicBtn == null) Debug.LogError("Không tìm thấy nút Music");
        if (soundFXBtn == null) Debug.LogError("Không tìm thấy nút SoundFX");
    }

    void setupGameBtn()
    {
        var gameSetting = transform.Find("GameSettings");
        if (gameSetting == null)
        {
            Debug.Log("Không tìm thấy GameSettings"); return;
        }

        returnBtn = gameSetting.Find("Return")?.GetComponent<Button>();
        fullScreenBtn = gameSetting.Find("FullScreen")?.GetComponent<Button>();
        homeBtn = gameSetting.Find("Home")?.GetComponent<Button>();

        if (returnBtn == null) Debug.LogError("Không tìm thấy nút Return");
        if (fullScreenBtn == null) Debug.LogError("Không tìm thấy nút FullScreen");
        if (homeBtn == null) Debug.LogError("Không tìm thấy nút Home");
    }

    private void OnEnable()
    {
        if (returnBtn) returnBtn.onClick.RemoveAllListeners();
        if (fullScreenBtn) fullScreenBtn.onClick.RemoveAllListeners();
        if (homeBtn) homeBtn.onClick.RemoveAllListeners();

        if (returnBtn) returnBtn.onClick.AddListener(ClosePanel);
        if (fullScreenBtn) fullScreenBtn.onClick.AddListener(ToggleFullscreen);
        if (homeBtn) homeBtn.onClick.AddListener(GoToMainMenu);

        // Cập nhật trạng thái fullscreen hiện tại
        isFullScreen = Screen.fullScreen;
    }

    void ClosePanel()
    {
        UI_Manager.Instance?.ToggleSettingGame();
    }

    void ToggleFullscreen()
    {
        isFullScreen = !isFullScreen;

        if (isFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(1280, 720, false);
        }
    }

    void GoToMainMenu()
    {
        GameSaver.SaveGame();

        ClosePanel();
        Time.timeScale = 1f;

        if (UI_Manager.Instance != null && UI_Manager.Instance.uiRoot != null)
            Destroy(UI_Manager.Instance.uiRoot);

        if (UI_Manager.Instance != null)
            Destroy(UI_Manager.Instance.gameObject);

        if (PlayerController.Instance != null)
            Destroy(PlayerController.Instance.gameObject);

        if (GameManager.instance != null)
            Destroy(GameManager.instance.gameObject);

        SceneManager.LoadScene("MainMenu");
    }
}
