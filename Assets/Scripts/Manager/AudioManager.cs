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
        Instance = this;
    }

    private void Start()
    {
        UpdateVolumes();
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
