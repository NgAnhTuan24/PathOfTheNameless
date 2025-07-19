using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableType type;
    public Sprite icon;

    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collectable)
    {
        Player player = collectable.GetComponent<Player>();

        if (player)
        {
            player.inventory.Add(this);
            Destroy(this.gameObject);
        }
    }
}

public enum CollectableType
{
    NONE, 
    go,

    // Hạt giống các loại
    hatGiongLua,
    hatGiongNgo,
    hatGiongCaChua,
    hatGiongCuCaiTrang,
    hatGiongCaRot,
    hatGiongDauTay,
    hatGiongLac,
    hatGiongTaoDo,
    hatGiongTaoXanh,

    // Nông sản sau khi thu hoạch
    lua, 
    ngo, 
    caChua, 
    cuCaiTrang, 
    caRot, 
    dauTay, 
    lac, 
    taoDo,
    taoXanh
}