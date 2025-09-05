using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBoundsHelper : MonoBehaviour
{
    public Tilemap tilemap;

    void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;

        Vector3 worldMin = tilemap.CellToWorld(bounds.min);
        Vector3 worldMax = tilemap.CellToWorld(bounds.max);

        Debug.Log($"Min: {worldMin}, Max: {worldMax}");
    }
}
