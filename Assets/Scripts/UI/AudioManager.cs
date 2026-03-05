using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("UI Clips")]
    [SerializeField] private AudioClip clickClip;

    float masterVolume = 1f;
    float musicVolume = 1f;
    float sfxVolume = 1f;

    private void Start()
    {
        LoadVolumes();
        UpdateVolumes();
    }

    void UpdateVolumes()
    {
        musicSource.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
    }

    // ----------- SFX -----------

    public void PlayClick()
    {
        if (clickClip != null)
            sfxSource.PlayOneShot(clickClip);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // ----------- MUSIC -----------

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // ----------- VOLUME -----------

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        UpdateVolumes();
        SaveVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        UpdateVolumes();
        SaveVolumes();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        UpdateVolumes();
        SaveVolumes();
    }

    public float GetMasterVolume() => masterVolume;
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;

    // ----------- SAVE -----------

    void SaveVolumes()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
}