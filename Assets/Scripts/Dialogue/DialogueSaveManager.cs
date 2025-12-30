using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueSaveManager : MonoBehaviour
{
    public static DialogueSaveManager Instance;

    private HashSet<string> completedDialogueIDs = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsDialogueCompleted(string id)
    {
        return completedDialogueIDs.Contains(id);
    }

    public void MarkDialogueCompleted(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            completedDialogueIDs.Add(id);
        }
    }

    public List<string> GetCompletedDialogueIDs()
    {
        return new List<string>(completedDialogueIDs);
    }

    public void LoadCompletedDialogues(List<string> ids)
    {
        completedDialogueIDs.Clear();
        if (ids != null) completedDialogueIDs.AddRange(ids);
        Debug.Log($"Đã load {completedDialogueIDs.Count} hội thoại chạy 1 lần từ save");
    }
}
