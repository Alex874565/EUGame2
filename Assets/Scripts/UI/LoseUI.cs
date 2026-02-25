using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LoseUI : MonoBehaviour
{
    [SerializeField] private MenuStaggerAnimation stagger;

    [Header("UI")]
    [SerializeField] private Button hubButton;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip countingClip;
    [SerializeField] private AudioClip finalDingClip;

    private void Awake()
    {
        hubButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("HubScene");
        });
    }

    private void Start()
    {
        Show(100);
    }

    public void Hide()
    {
        stagger.CloseMenu(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void Show(int moneyEarned)
    {
        gameObject.SetActive(true);
        stagger.OpenMenu(() =>
        {
            AnimateMoney(moneyEarned);
        });
    }

    private void AnimateMoney(int moneyEarned)
    {
        moneyText.text = "0";

        audioSource.clip = countingClip;
        audioSource.loop = true;
        audioSource.Play();

        DOVirtual.Float(0, moneyEarned, 1.2f, value =>
        {
            moneyText.text = Mathf.RoundToInt(value).ToString();
        })
        .SetEase(Ease.OutCubic)
        .SetUpdate(true)
        .OnComplete(() =>
        {
            audioSource.Stop();
            audioSource.PlayOneShot(finalDingClip);

            moneyText.transform
                .DOScale(1.2f, 0.15f)
                .OnComplete(() =>
                {
                    moneyText.transform.DOScale(1f, 0.15f);
                });
        });
    }

    private void AnimateTime(float survivedSeconds)
    {
        timerText.text = "00:00";

        DOVirtual.Float(0f, survivedSeconds, 1.5f, value =>
        {
            timerText.text = FormatTime(value);
        })
        .SetEase(Ease.OutCubic)
        .SetUpdate(true);
    }

    private string FormatTime(float seconds)
    {
        int totalSeconds = Mathf.FloorToInt(seconds);

        int minutes = totalSeconds / 60;
        int remainingSeconds = totalSeconds % 60;

        return $"{minutes:00}:{remainingSeconds:00}";
    }
}