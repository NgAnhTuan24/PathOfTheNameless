using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Settings")]
    public bool musicOn = true;
    public bool sfxOn = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        EnsureAudioSources();
    }

    private void Start()
    {
        UpdateVolumes();
    }

    private void EnsureAudioSources()
    {
        // Nếu musicSource bị null → tìm child tên "Music"
        if (musicSource == null)
        {
            Transform musicTrans = transform.Find("Music");
            if (musicTrans != null)
                musicSource = musicTrans.GetComponent<AudioSource>();
        }

        // Nếu sfxSource bị null → tìm child tên "SFX"
        if (sfxSource == null)
        {
            Transform sfxTrans = transform.Find("SFX");
            if (sfxTrans != null)
                sfxSource = sfxTrans.GetComponent<AudioSource>();
        }

        // Nếu vẫn null (hiếm), log lỗi để dễ debug
        if (musicSource == null) Debug.LogError("AudioManager: Không tìm thấy Music AudioSource!");
        if (sfxSource == null) Debug.LogError("AudioManager: Không tìm thấy SFX AudioSource!");
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;
        UpdateVolumes();
    }

    public void ToggleSFX()
    {
        sfxOn = !sfxOn;
        UpdateVolumes();
    }

    public void UpdateVolumes()
    {
        if (musicSource != null)
            musicSource.mute = !musicOn;

        if (sfxSource != null)
            sfxSource.mute = !sfxOn;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (!sfxOn || clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }
}
