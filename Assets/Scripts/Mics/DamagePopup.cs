using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;

    private Vector3 lastPosition; // vị trí để popup bay lên
    private Transform target;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, Transform targetTransform = null)
    {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = .5f; // thời gian popup hiển thị

        target = targetTransform;
        if (target != null)
            lastPosition = target.position; // lưu vị trí ban đầu
        else
            lastPosition = transform.position;
    }

    private void Update()
    {
        float moveYSpeed = 3.5f;

        // Nếu target còn sống → theo target
        if (target != null)
        {
            lastPosition = target.position; // cập nhật vị trí
        }

        // bay lên dần dựa trên lastPosition
        transform.position = lastPosition + new Vector3(0, moveYSpeed * (1 - disappearTimer), 0);

        // Fade out
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0)
                Destroy(gameObject);
        }
    }

    public static DamagePopup Create(Vector3 pos, int damageAmount, Transform targetTransform = null)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.popupText, pos, Quaternion.identity);
        DamagePopup popup = damagePopupTransform.GetComponent<DamagePopup>();
        popup.Setup(damageAmount, targetTransform);
        return popup;
    }
}
