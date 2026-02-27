using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LoseUI : MonoBehaviour
{
    [SerializeField] private float moneyModifier = .5f;
    
    [SerializeField] private MenuStaggerAnimation stagger;

    [Header("UI")]
    [SerializeField] private Button hubButton;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Audio")]
    [Header("Money Audio")]
    [SerializeField] private AudioSource moneyAudioSource;
    [SerializeField] private AudioClip moneyCountingClip;

    [Header("Time Audio")]
    [SerializeField] private AudioSource timeAudioSource;
    [SerializeField] private AudioClip timeCountingClip;

    private void Awake()
    {
        hubButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("HubScene");
        });
    }

    private void Start()
    {
        int moneyEarned = 0;
        int secondsSurvived = 0;
        if (ServiceLocator.Instance != null && ServiceLocator.Instance.WavesManager != null)
        {
            moneyEarned = ServiceLocator.Instance.WavesManager.CurrentMoney * (int)moneyModifier;
            secondsSurvived = Mathf.FloorToInt(ServiceLocator.Instance.WavesManager.TimeSinceStart);
        }

        if (ServiceLocator.Instance != null && ServiceLocator.Instance.GameManager != null)
        {
            ServiceLocator.Instance.GameManager.WonLastWave = false;
        }

        //gameObject.SetActive(false);
        Show(moneyEarned, secondsSurvived);
    }

    public void Hide()
    {
        stagger.CloseMenu(() =>
        {
            gameObject.SetActive(false);
        });
    }

    // Change your Show method to handle both callbacks
    public void Show(int moneyEarned, float secondsSurvived)
    {
        gameObject.SetActive(true);
        
        // Pass two actions: one for money, one for time
        stagger.OpenMenu(
            onMoneyShown: () => AnimateMoney(moneyEarned),
            onTimeShown: () => AnimateTime(secondsSurvived)
        );
    }

    private void AnimateMoney(int moneyEarned)
    {
        moneyText.text = "0";

        // Create a temporary AudioSource for this counting
        AudioSource tempSource = moneyText.gameObject.AddComponent<AudioSource>();
        tempSource.clip = moneyCountingClip;
        tempSource.loop = true;
        tempSource.playOnAwake = false;
        tempSource.Play();

        DOVirtual.Float(0, moneyEarned, 1.2f, value =>
        {
            moneyText.text = Mathf.RoundToInt(value).ToString();
        })
        .SetEase(Ease.OutCubic)
        .SetUpdate(true)
        .OnComplete(() =>
        {
            tempSource.Stop();
            Destroy(tempSource); // clean up

            moneyText.transform
                .DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1)
                .SetUpdate(true);
        });
    }

    private void AnimateTime(float survivedSeconds)
    {
        timerText.text = "00:00";

        // Temporary AudioSource for counting
        AudioSource tempSource = timerText.gameObject.AddComponent<AudioSource>();
        tempSource.clip = timeCountingClip;
        tempSource.loop = true;
        tempSource.playOnAwake = false;
        tempSource.Play();

        DOVirtual.Float(0f, survivedSeconds, 1.2f, value =>
        {
            timerText.text = FormatTime(value);
        })
        .SetEase(Ease.OutCubic)
        .SetUpdate(true)
        .OnComplete(() =>
        {
            tempSource.Stop();
            Destroy(tempSource); // cleanup

            // Finish animation and sound
            timerText.transform
                .DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1)
                .SetUpdate(true);

        });
    }
    
    private string FormatTime(float seconds)
    {
        int totalSeconds = Mathf.FloorToInt(seconds);

        int minutes = totalSeconds / 60;
        int remainingSeconds = totalSeconds % 60;

        return $"{minutes:00}:{remainingSeconds:00}";
    }
}