using System.Collections;
using UnityEngine;
using TMPro;

public class MinigameController : MonoBehaviour
{
    [field: SerializeField] public MinigameType Type { get; set; }
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _loseUI;
    
    protected MinigameData Data { get; set; }
    
    private float _score;
    private float _timeSinceStart;
    
    protected bool GamePlaying;
    
    protected void Awake()
    {
        Data = ServiceLocator.Instance.MinigamesManager.MinigamesData[Type];
        GamePlaying = false;
    }

    protected void Update()
    {
        if (!GamePlaying) return;
        
        _timeSinceStart += Time.deltaTime;
        UpdateTimer();
        if (_timeSinceStart > Data.TimeLimit)
        {
            EndMinigame(false);
        }
    }
    
    public virtual void StartMinigame()
    {
        _score = 0;
        _timeSinceStart = 0;
        UpdateScore();
        UpdateTimer();
        GamePlaying = true;
    }

    private void GiveReward()
    {
        int currentMoney = ServiceLocator.Instance.WavesManager.CurrentMoney;
        ServiceLocator.Instance.WavesManager.UpdateMoney(currentMoney + Data.Reward);
    }

    protected void AddScore(int value)
    {
        _score += value;
        if(_score >= Data.ScoreToWin)
        {
            StartCoroutine(EndMinigame(true));
        }
        else
        {
            UpdateScore();
        }
    }
    
    private void UpdateScore()
    {
        _scoreText.text = $"Score: {_score}/{Data.ScoreToWin}";
    }

    private void UpdateTimer()
    {
        _timerText.text = $"{Mathf.RoundToInt(Data.TimeLimit - _timeSinceStart)}s";
    }
    
    
    private IEnumerator EndMinigame(bool won)
    {
        GamePlaying = false;
        Cleanup();
        if (won)
        {
            _winUI.SetActive(true);
            GiveReward();
        }
        else
        {
            _loseUI.SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        _winUI.SetActive(false);
        _loseUI.SetActive(false);
        ServiceLocator.Instance.MinigamesManager.CloseMinigame();
    }

    protected virtual void Cleanup()
    {
        return;
    }
}