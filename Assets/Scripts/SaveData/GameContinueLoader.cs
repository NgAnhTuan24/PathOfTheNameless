using UnityEngine;
using UnityEngine.SceneManagement;

public class GameContinueLoader : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("IsContinuing", 0) == 1)
        {
            PlayerPrefs.DeleteKey("IsContinuing"); // dùng 1 lần thôi
            PlayerPrefs.Save();

            var data = GameSaver.LoadGame();
            if (data != null && PlayerController.Instance != null)
            {
                Vector3 pos = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
                PlayerController.Instance.transform.position = pos;

                Debug.Log("Tiếp tục game - Đặt player về vị trí: " + pos);
            }
        }
    }
}