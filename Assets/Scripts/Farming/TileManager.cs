using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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


public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile interactedTile;

    public float tillResetTime = 10f; // thời gian reset
    private List<TilledTile> tilledTiles = new List<TilledTile>();

    private Dictionary<Vector3Int, GameObject> crops = new Dictionary<Vector3Int, GameObject>();

    void Start()
    {
        try
        {
            foreach (var pos in interactableMap.cellBounds.allPositionsWithin)
            {
                if (interactableMap.HasTile(pos))
                {
                    interactableMap.SetTile(pos, hiddenInteractableTile);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Lỗi trong TileManager.Start(), Lỗi null: " + ex.Message);
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
        interactableMap.SetTile(pos, interactedTile);
    }

    public bool IsInteracted(Vector3Int pos)
    {
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
        interactableMap.SetTile(pos, hiddenInteractableTile);
        if (crops.ContainsKey(pos))
        {
            crops.Remove(pos);
        }
    }

    public void TillTile(Vector3Int pos)
    {
        interactableMap.SetTile(pos, interactedTile);

        // Thêm vào danh sách đất đã cuốc (nếu chưa có)
        if (!tilledTiles.Any(t => t.position == pos))
        {
            tilledTiles.Add(new TilledTile(pos, tillResetTime));
        }
    }
}
