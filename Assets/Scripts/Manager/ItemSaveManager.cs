using System.Collections.Generic;
using UnityEngine;

public class ItemSaveManager : MonoBehaviour
{
    public static ItemSaveManager instance;

    private HashSet<string> itemCollected = new HashSet<string>();

    [SerializeField] private List<string> collectedList = new List<string>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void MarkAsRemoved(string id)
    {
        if (itemCollected.Add(id))
        {
            UpdateCollectedList();
        }
    }

    public void UnmarkAsRemoved(string id)
    {
        if (itemCollected.Contains(id))
        {
            itemCollected.Remove(id);
            UpdateCollectedList();
        }
    }

    public bool IsRemoved(string id)
    {
        return itemCollected.Contains(id);
    }

    private void UpdateCollectedList()
    {
        collectedList = new List<string>(itemCollected);
    }
}
