using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using System.Collections;

[RequireComponent(typeof(PlayableDirector))]
public class WavesManager : MonoBehaviour
{
    [Header("UI Elements")]
    [field: SerializeField] private GameObject winPanel;
    [field: SerializeField] private GameObject losePanel;
    [field: SerializeField] private TMP_Text timeText;
    [field: SerializeField] private TMP_Text moneyText;
    [field: SerializeField] private TMP_Text emergenciesFailedText;
    
    public WavesDatabase WavesDatabase { get; private set; }
    public int CurrentWaveIndex { get; private set; }
    public WaveData CurrentWaveData { get; set; }
    
    public int CurrentMoney { get; private set; }
    public int CurrentEmergenciesFailed { get; private set; }
    
    private PlayableDirector _playableDirector;

    private float _time;
    
    private void Start()
    {
        WavesDatabase = ServiceLocator.Instance.WavesDatabase;
        CurrentWaveIndex = 0;
        _playableDirector = GetComponent<PlayableDirector>();
        StartWave();
    }

    public void Update()
    {
        if (!ServiceLocator.Instance.GameManager.IsPaused)
        {
            _time += Time.deltaTime;
            UpdateTime(CurrentWaveData.WaveDuration - _time);
        }
    }
    
    public void StartWave()
    {
        if (CurrentWaveIndex >= WavesDatabase.Waves.Count)
        {
            return;
        }
        
        CurrentWaveData = WavesDatabase.Waves[CurrentWaveIndex];

        ServiceLocator.Instance.UnitsManager.InitializeUnits(CurrentWaveIndex);
        
        _playableDirector.playableAsset = CurrentWaveData.TimelineAsset;
        _playableDirector.Play();
        
        UpdateMoney(CurrentWaveData.StartingMoney);
        UpdateEmergenciesFailed(0);
        
        _time = 0;
    }
    
    public void IncrementWaveIndex()
    {
        CurrentWaveIndex++;
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
        moneyText.text = $"{money}€";
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
        ServiceLocator.Instance.UnitsManager.DestroyUnits();
        ServiceLocator.Instance.EmergenciesManager.DestroyEmergencies();
        ServiceLocator.Instance.MinigamesManager.DestroyMinigames();
    }

    #region Coroutines

    public IEnumerator UpdateMoneyCoroutine(bool lost)
    {
        Color originalColor = moneyText.color;
        Vector3 originalScale = moneyText.gameObject.transform.localScale;
        moneyText.color = new Color(lost ? 0.6f : 0.2f, lost ? 0.2f : 0.6f, 0.2f);
        moneyText.gameObject.transform.localScale = lost ? originalScale * 0.8f : originalScale * 1.2f;
        yield return new WaitForSeconds(0.5f);
        moneyText.color = originalColor;
        moneyText.gameObject.transform.localScale = originalScale;
    }

    public IEnumerator UpdateEmergenciesCoroutine(bool added)
    {
        Color originalColor = emergenciesFailedText.color;
        Vector3 originalScale = emergenciesFailedText.gameObject.transform.localScale;
        emergenciesFailedText.color = new Color(added ? 0.6f : 0.2f, added ? 0.2f : 0.6f, 0.2f);
        emergenciesFailedText.gameObject.transform.localScale = added ? originalScale * 1.2f : originalScale * 0.8f;
        yield return new WaitForSeconds(0.5f);
        emergenciesFailedText.color = originalColor;
        emergenciesFailedText.gameObject.transform.localScale = originalScale;
    }

    #endregion
}