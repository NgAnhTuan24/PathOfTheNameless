using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;

    public UI_Manager uiManager;

    public Player player;

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
        uiManager = GetComponent<UI_Manager>();

        player = FindObjectOfType<Player>();
    }
}
