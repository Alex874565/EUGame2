using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("Volumes")]
    [Range(0f,1f)] [SerializeField] private float masterVolume = 1f;
    [Range(0f,1f)] [SerializeField] private float musicVolume = 1f;
    [Range(0f,1f)] [SerializeField] private float sfxVolume = 1f;

    private AudioClip currentMusic;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ApplyVolumes();
    }

    void ApplyVolumes()
    {
        musicSource.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
    }

    // MUSIC SWITCH
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || currentMusic == clip) return;
        currentMusic = clip;

        musicSource.DOKill();
        musicSource.DOFade(0f, 0.5f).SetUpdate(true).OnComplete(() =>
        {
            musicSource.clip = clip;
            musicSource.Play();
            musicSource.DOFade(musicVolume * masterVolume, 0.5f).SetUpdate(true);
        });
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameplayMusic()
    {
        Debug.Log("PLAY GAMEPLAY MUSIC CALLED");
        PlayMusic(gameplayMusic);
    }

    // VOLUME
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        ApplyVolumes();
    }

    public float GetMusicVolume() => musicVolume;

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        ApplyVolumes();
    }

    public float GetSFXVolume() => sfxVolume;

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        ApplyVolumes();
    }

    public float GetMasterVolume() => masterVolume;

    // SFX
    public void PlayUI(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void StopMusic(float fadeDuration = 0.5f)
    {
        musicSource.DOKill();
        // SetUpdate(true) is critical for pausing!
        musicSource.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            musicSource.Stop();
            musicSource.clip = null;
            currentMusic = null;
            musicSource.volume = musicVolume * masterVolume;
        });
    }

    public AudioSource GetMusicSource()
    {
        return musicSource;
    }
}