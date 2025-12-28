using System;
using UnityEngine;

public class ChestID : MonoBehaviour
{
    [SerializeField] private string id;

    public string ID => id;

    private void Reset()
    {
        GenerateID();
    }

    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        id = Guid.NewGuid().ToString();
    }
}
