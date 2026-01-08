using UnityEngine;

public class ArenaID : MonoBehaviour
{
    [SerializeField] private string arenaID;

    public string GetID() => arenaID;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(arenaID))
        {
            arenaID = System.Guid.NewGuid().ToString();
        }
    }
#endif
}
