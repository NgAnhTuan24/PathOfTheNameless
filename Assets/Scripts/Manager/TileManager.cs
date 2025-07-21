using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile interactedTile;

    void Start()
    {
        foreach(var pos in interactableMap.cellBounds.allPositionsWithin)
        {
            if (interactableMap.HasTile(pos))
            {
                interactableMap.SetTile(pos, hiddenInteractableTile);
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
}
