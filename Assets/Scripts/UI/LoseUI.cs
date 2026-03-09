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
        gameObject.SetActive(false);
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
        moneyEarned = moneyEarned / 2;
        // Setup initial state for animation
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        
        cg.alpha = 0;
        transform.localScale = Vector3.one * 0.85f;
        gameObject.SetActive(true);

        // Sequence for smooth entry
        Sequence entrySequence = DOTween.Sequence().SetUpdate(true);
        entrySequence.Join(cg.DOFade(1f, 0.5f));
        entrySequence.Join(transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
        
        entrySequence.OnComplete(() => 
        {
            // Logic and Save
            ServiceLocator.Instance.PlayerManager.AddMoney(moneyEarned);
            ServiceLocator.Instance.GameManager.WonLastWave = false;
            ServiceLocator.Instance.SaveManager.SaveGame(new SaveData(
                ServiceLocator.Instance.GameManager.WaveIndex,
                ServiceLocator.Instance.GameManager.WonLastWave,
                ServiceLocator.Instance.PlayerManager.Money,
                ServiceLocator.Instance.PlayerManager.OwnedUpgrades,
                ServiceLocator.Instance.PlayerManager.StartingUnits
            ));

            // Start counting animations
            stagger.OpenMenu(
                onMoneyShown: () => AnimateMoney(moneyEarned),
                onTimeShown: () => AnimateTime(secondsSurvived)
            );
        });
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