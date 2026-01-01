using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TilledTile
{
    public Vector3Int position;
    public float timer;

    public TilledTile(Vector3Int pos, float countdown)
    {
        position = pos;
        timer = countdown;
    }
}

[System.Serializable]
public class CropSaveData
{
    public Vector3Int position;
    public string cropId;
    public int growStage;
    public float timer;
}

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile interactedTile;

    public float tillResetTime = 10f; // thời gian reset
    [SerializeField] private List<TilledTile> tilledTiles = new List<TilledTile>();

    private Dictionary<Vector3Int, GameObject> crops = new Dictionary<Vector3Int, GameObject>();

    public List<CropSaveData> savedCrops = new();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject mapObj = GameObject.FindGameObjectWithTag("InteractableMap");
        if (mapObj == null)
        {
            interactableMap = null;
            return;
        }

        interactableMap = mapObj.GetComponent<Tilemap>();

        InitMap();            
        RestoreCrops();
    }

    private void InitMap()
    {
        if (interactableMap == null) return;

        foreach (var pos in interactableMap.cellBounds.allPositionsWithin)
        {
            if (interactableMap.HasTile(pos))
            {
                interactableMap.SetTile(pos, hiddenInteractableTile);
            }
        }

        // Nếu muốn load lại trạng thái đất đã cuốc từ tilledTiles:
        foreach (var t in tilledTiles)
        {
            interactableMap.SetTile(t.position, interactedTile);
        }
    }


    void Update()
    {
        for (int i = tilledTiles.Count - 1; i >= 0; i--)
        {
            TilledTile tile = tilledTiles[i];
            tile.timer -= Time.deltaTime;

            if (tile.timer <= 0f)
            {
                // Nếu tại tile đó KHÔNG có cây
                if (!crops.ContainsKey(tile.position))
                {
                    // Reset về tile ban đầu
                    ResetTile(tile.position);
                }

                tilledTiles.RemoveAt(i);
            }
        }
    }

    public bool IsInteractable(Vector3Int pos)
    {
        if (interactableMap == null) return false;

        TileBase tile = interactableMap.GetTile(pos);

        if(tile != null)
        {
            if(tile.name == "Interactable")
            {
                return true;
            }
        }

        return false;
    }

    public void SetInteracted(Vector3Int pos)
    {
        if (interactableMap == null) return;

        interactableMap.SetTile(pos, interactedTile);
    }

    public bool IsInteracted(Vector3Int pos)
    {
        if (interactableMap == null) return false;

        TileBase tile = interactableMap.GetTile(pos);
        return tile == interactedTile;
    }

    public bool HasCrop(Vector3Int pos)
    {
        return crops.ContainsKey(pos);
    }

    public void AddCrop(Vector3Int pos, GameObject crop)
    {
        if (!crops.ContainsKey(pos))
        {
            crops.Add(pos, crop);
        }
    }

    public void ResetTile(Vector3Int pos)
    {
        if (interactableMap == null) return;

        interactableMap.SetTile(pos, hiddenInteractableTile);
        if (crops.ContainsKey(pos))
        {
            crops.Remove(pos);
        }
    }

    public void TillTile(Vector3Int pos)
    {
        if (interactableMap == null) return;

        interactableMap.SetTile(pos, interactedTile);

        // Thêm vào danh sách đất đã cuốc (nếu chưa có)
        if (!tilledTiles.Any(t => t.position == pos))
        {
            tilledTiles.Add(new TilledTile(pos, tillResetTime));
        }
    }

    private void RestoreCrops()
    {
        if (interactableMap == null) return;

        crops.Clear();

        foreach (var data in savedCrops)
        {
            Item item = GameManager.instance.itemManager.GetItemByName(data.cropId);
            if (item == null) continue;

            GameObject crop = Instantiate(
                item.data.cropPrefab,
                data.position + new Vector3(0.5f, 0.5f),
                Quaternion.identity
            );

            Crop cropComp = crop.GetComponent<Crop>();
            cropComp.Load(data.growStage, data.timer);

            crops[data.position] = crop;
            interactableMap.SetTile(data.position, interactedTile);
        }
    }

    public void UpdateCropSaveData(Vector3Int pos, int stage, float timer)
    {
        var data = savedCrops.FirstOrDefault(c => c.position == pos);
        if (data != null)
        {
            data.growStage = stage;
            data.timer = timer;
        }
    }

}
