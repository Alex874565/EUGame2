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
            ServiceLocator.Instance.GameManager.WonLastWave = true;
            ServiceLocator.Instance.GameManager.WaveIndex += 1;
            ServiceLocator.Instance.PlayerManager.AddMoney(moneyEarned);
            ServiceLocator.Instance.SaveManager.SaveGame(new SaveData(
                ServiceLocator.Instance.GameManager.WaveIndex,
                ServiceLocator.Instance.GameManager.WonLastWave,
                ServiceLocator.Instance.PlayerManager.Money,
                ServiceLocator.Instance.PlayerManager.OwnedUpgrades,
                ServiceLocator.Instance.PlayerManager.StartingUnits
            ));
                
                
            stagger.OpenMenu(() =>
            {
                AnimateMoney(moneyEarned);
            });
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