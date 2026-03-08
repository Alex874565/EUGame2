using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using System.Collections;

[RequireComponent(typeof(PlayableDirector))]
public class WavesManager : MonoBehaviour
{
    [Header("UI Elements")]
    [field: SerializeField] private WinUI winPanel;
    [field: SerializeField] private LoseUI losePanel;
    [field: SerializeField] private TMP_Text timeText;
    [field: SerializeField] private TMP_Text moneyText;
    [field: SerializeField] private TMP_Text emergenciesFailedText;
    
    public WavesDatabase WavesDatabase { get; private set; }
    public WaveData CurrentWaveData { get; set; }
    
    public int CurrentMoney { get; private set; }
    public int CurrentEmergenciesFailed { get; private set; }
    
    private PlayableDirector _playableDirector;

    public float TimeSinceStart { get; private set; }

    private bool waveEnded = false;
    
    private void Start()
    {
        WavesDatabase = ServiceLocator.Instance.WavesDatabase;
        _playableDirector = GetComponent<PlayableDirector>();
        StartWave(ServiceLocator.Instance.GameManager.WaveIndex);

        ServiceLocator.Instance.AudioManager.PlayGameplayMusic();
    }

    public void Update()
    {
        if (!ServiceLocator.Instance.GameManager.IsPaused)
        {
            TimeSinceStart += Time.deltaTime;
            UpdateTime(CurrentWaveData.WaveDuration - TimeSinceStart);
        }
    }
    
    public void StartWave(int index)
    {
        waveEnded = false;

        ServiceLocator.Instance.GameManager.ResumeGame();
        
        if (index >= WavesDatabase.Waves.Count)
        {
            return;
        }
        
        CurrentWaveData = new WaveData(WavesDatabase.Waves[index]);

        ServiceLocator.Instance.UnitsManager.InitializeUnits(index);
        
        _playableDirector.playableAsset = CurrentWaveData.TimelineAsset;
        _playableDirector.Play();
        
        UpdateMoney(CurrentWaveData.StartingMoney);
        UpdateEmergenciesFailed(0);
        
        TimeSinceStart = 0;
    }
    
    public void UpdateTime(float time)
    {
        timeText.text = $"{time:F0}s";
        
        if(time <= 0)
        {
            EndWave(true);
        }
    }
    
    public void UpdateMoney(int money)
    {
        StartCoroutine(UpdateMoneyCoroutine(money < CurrentMoney));
    
        CurrentMoney = money;
        moneyText.text = $"{money}";
        
        if(CurrentMoney < 0)
        {
            EndWave(false);
        }
    }
    
    public void UpdateEmergenciesFailed(int emergenciesFailed)
    {
        StartCoroutine(UpdateEmergenciesCoroutine(emergenciesFailed > CurrentEmergenciesFailed));
     
        CurrentEmergenciesFailed = emergenciesFailed;
        emergenciesFailedText.text = $"{emergenciesFailed}/{CurrentWaveData.EmergencyFailLimit}";
        
        if(CurrentEmergenciesFailed > CurrentWaveData.EmergencyFailLimit)
        {
            EndWave(false);
        }
    }

    public void EndWave(bool won)
    {
        if (waveEnded) return;
        waveEnded = true;
        StartCoroutine(EndWaveRoutine(won));
    }

    private IEnumerator EndWaveRoutine(bool won)
    {
        // 1. Visual/Audio feedback of the end
        _playableDirector.Stop();
        ServiceLocator.Instance.AudioManager.StopMusic(1.0f);
        
        // Stop all SFX
        var sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (var source in sources)
        {
            if (source != ServiceLocator.Instance.AudioManager.GetMusicSource())
                source.Stop();
        }

        // 2. Wait for a second so the user processes the loss/win
        // We use WaitForSecondsRealtime because we are about to pause
        yield return new WaitForSecondsRealtime(1.0f);

        // 3. Pause and Cleanup
        ServiceLocator.Instance.GameManager.PauseGame();
        ServiceLocator.Instance.UnitsManager.DestroyUnits();
        ServiceLocator.Instance.EmergenciesManager.DestroyEmergencies();
        ServiceLocator.Instance.MinigamesManager.DestroyMinigames();
        ServiceLocator.Instance.UIManager.Inventory.SetActive(false);

        // 4. Show UI
        if(won)
        {
            winPanel.Show(CurrentMoney);
        }
        else
        {
            int moneyEarned = Mathf.RoundToInt(CurrentWaveData.StartingMoney * (TimeSinceStart / CurrentWaveData.WaveDuration));
            losePanel.Show(moneyEarned, TimeSinceStart);
        }
    }

    public void PauseTimeline()
    {
        _playableDirector.Pause();
    }
    
    public void ResumeTimeline()
    {
        _playableDirector.Resume();
    }

    #region Coroutines

    public IEnumerator UpdateMoneyCoroutine(bool lost)
    {
        Color originalColor = moneyText.color;
        Vector3 originalScale = moneyText.gameObject.transform.localScale;
        moneyText.color = new Color(lost ? 0.6f : 0.2f, lost ? 0.2f : 0.6f, 0.2f);
        moneyText.gameObject.transform.localScale = lost ? originalScale * 0.8f : originalScale * 1.2f;
        yield return new WaitForSecondsRealtime(0.5f);
        moneyText.color = originalColor;
        moneyText.gameObject.transform.localScale = originalScale;
    }

    public IEnumerator UpdateEmergenciesCoroutine(bool added)
    {
        Color originalColor = emergenciesFailedText.color;
        Vector3 originalScale = emergenciesFailedText.gameObject.transform.localScale;
        emergenciesFailedText.color = new Color(added ? 0.6f : 0.2f, added ? 0.2f : 0.6f, 0.2f);
        emergenciesFailedText.gameObject.transform.localScale = added ? originalScale * 1.2f : originalScale * 0.8f;
        yield return new WaitForSecondsRealtime(0.5f);
        emergenciesFailedText.color = originalColor;
        emergenciesFailedText.gameObject.transform.localScale = originalScale;
    }

    #endregion
}