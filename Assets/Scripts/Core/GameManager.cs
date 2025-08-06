using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;

    public TileManager tileManager;

    public UI_Manager uiManager;

    public SceneManagement sceneManagement;

    public ItemDropper player;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
            
        DontDestroyOnLoad(gameObject);

        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
        uiManager = GetComponent<UI_Manager>();
        sceneManagement = GetComponent<SceneManagement>();

        player = FindObjectOfType<ItemDropper>();
    }
}
