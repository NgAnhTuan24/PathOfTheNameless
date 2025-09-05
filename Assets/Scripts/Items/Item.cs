using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    public ItemData data;

    [HideInInspector]
    public Rigidbody2D rb;

    private GenerateID generateID;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        generateID = GetComponent<GenerateID>();
    }


    public void GenerateID() => generateID.CreateID();
    public void MarkAsRemoved() => generateID.MarkAsRemoved();
    public string GetID() => generateID.GetID();
}
