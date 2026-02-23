using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    [Header("Clips")]
    [SerializeField] private AudioClip clickClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    public void PlayClick()
    {
        if (clickClip == null) return;
        audioSource.PlayOneShot(clickClip);
    }
}