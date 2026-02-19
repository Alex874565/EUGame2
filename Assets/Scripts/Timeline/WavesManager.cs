using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class WavesManager : MonoBehaviour
{
    public WavesDatabase WavesDatabase { get; private set; }
    public int CurrentWaveIndex { get; private set; }
    
    private PlayableDirector _playableDirector;
    
    private void Start()
    {
        WavesDatabase = ServiceLocator.Instance.WavesDatabase;
        CurrentWaveIndex = 0;
        _playableDirector = GetComponent<PlayableDirector>();
        StartWave();
    }
    
    public void StartWave()
    {
        if (CurrentWaveIndex >= WavesDatabase.Waves.Count)
        {
            return;
        }
        
        WaveData currentWave = WavesDatabase.Waves[CurrentWaveIndex];

        ServiceLocator.Instance.UnitsManager.InitializeUnits(CurrentWaveIndex);
        
        _playableDirector.playableAsset = currentWave.TimelineAsset;
        _playableDirector.Play();
    }
    
    public void IncrementWaveIndex()
    {
        CurrentWaveIndex++;
    }
}