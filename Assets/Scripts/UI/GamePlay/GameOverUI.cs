using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button homeButton;

    private void Awake()
    {
        if (homeButton == null)
            homeButton = transform.Find("HomeButton")?.GetComponent<Button>();

        if (homeButton != null)
            homeButton.onClick.AddListener(GoToMainMenu);
        else
            Debug.LogError("Không tìm thấy HomeButton trong GameOverUI");
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    void GoToMainMenu()
    {
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
