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
        //Show(100);
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

        // Create a temporary AudioSource for this counting
        AudioSource tempSource = moneyText.gameObject.AddComponent<AudioSource>();
        tempSource.clip = countingClip;
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
}