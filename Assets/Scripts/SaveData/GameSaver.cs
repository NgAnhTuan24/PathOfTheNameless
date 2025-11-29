using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public string currentScene;
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;
    // Thêm dữ liệu khác nếu cần: coin, level, inventory...
}

public static class GameSaver
{
    private const string SAVE_KEY = "PlayerSaveData";

    public static void SaveGame()
    {
        SaveData data = new SaveData
        {
            currentScene = SceneManager.GetActiveScene().name
        };

        if (PlayerController.Instance != null)
        {
            Vector3 pos = PlayerController.Instance.transform.position;
            data.playerPosX = pos.x;
            data.playerPosY = pos.y;
            data.playerPosZ = pos.z;
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log($"Đã lưu game: {data.currentScene} - Player pos: {data.playerPosX}, {data.playerPosY}");
    }

    public static SaveData LoadGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return null;
    }

    public static bool HasSaveData() => PlayerPrefs.HasKey(SAVE_KEY);

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.DeleteKey("IsContinuing");
        PlayerPrefs.Save();
    }
}