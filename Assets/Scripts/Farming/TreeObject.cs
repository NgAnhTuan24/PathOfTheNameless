using UnityEngine;

[System.Serializable]
public class DropItem
{
    public GameObject vatPhamRoi;     
    public int soLuongToiThieu = 1;        
    public int soLuongToiDa = 3;         
    [Range(0f, 1f)] public float tiLeRoi = 1f; //(1 = 100%)
}

public class TreeObject : MonoBehaviour
{
    [Header("Danh sách vật phẩm có thể rơi")]
    [SerializeField] private DropItem[] possibleDrops;

    [Header("Chặt cây")]
    [SerializeField] private int soLanChatToiThieu = 2;
    [SerializeField] private int soLanChatToiDa = 5;
    private int soLanChat = 0; // Đếm số lần đã chặt
    private int dieukienDeCayDo;
    [SerializeField] private GameObject deathVFX;

    private GenerateID newID;

    private void Awake()
    {
        newID = GetComponent<GenerateID>();
    }

    private void Start()
    {
        dieukienDeCayDo = Random.Range(soLanChatToiThieu, soLanChatToiDa + 1);
    }

    public void Chop()
    {
        soLanChat++;
        Debug.Log($"Chặt cây {soLanChat}/{dieukienDeCayDo}");

        if (soLanChat >= dieukienDeCayDo)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            DropItems();
            Destroy(gameObject);
        }
    }

    private void DropItems()
    {
        foreach (DropItem drop in possibleDrops)
        {
            if (Random.value <= drop.tiLeRoi)
            {
                int amount = Random.Range(drop.soLuongToiThieu, drop.soLuongToiDa + 1);
                for (int i = 0; i < amount; i++)
                {
                    Vector3 dropPos = transform.position + (Vector3)(Random.insideUnitCircle * 0.5f);
                    GameObject obj = Instantiate(drop.vatPhamRoi, dropPos, Quaternion.identity);
                    
                   newID = obj.GetComponent<GenerateID>();
                    if (newID != null) 
                    {
                        newID.CreateID();
                        ItemSaveManager.instance?.UnmarkAsRemoved(newID.GetID());
                    }
                }
            }
        }
    }
}
