using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WinUI : MonoBehaviour
{
    [SerializeField] private MenuStaggerAnimation stagger;

    [Header("UI")]
    [SerializeField] private Button hubButton;
    [SerializeField] private TextMeshProUGUI moneyText;

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
        gameObject.SetActive(false);
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
}