using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button _buttonElement;
    [SerializeField] private Image _iconRenderer;

    [Header("Sprites")]
    [SerializeField] private Sprite _activeImage;
    [SerializeField] private Sprite _disableImage;

    private bool _isActive = true;

    private void Awake()
    {
        if (_buttonElement == null) _buttonElement = GetComponent<Button>();
        if (_iconRenderer == null) _iconRenderer = GetComponent<Image>();
    }

    private void Start()
    {
        UpdateIcon();
        _buttonElement.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _isActive = !_isActive;
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        _iconRenderer.sprite = _isActive ? _activeImage : _disableImage;
    }
}
