using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineManager : MonoBehaviour
{
    public WavesDatabase WavesDatabase { get; private set; }

    private int _currentWaveIndex;
    private PlayableDirector _playableDirector;
    
    private void Start()
    {
        WavesDatabase = ServiceLocator.Instance.WavesDatabase;
        _currentWaveIndex = 0;
        _playableDirector = GetComponent<PlayableDirector>();
        StartWave();
    }
    
    public void StartWave()
    {
        if (_currentWaveIndex >= WavesDatabase.Waves.Count)
        {
            return;
        }
        
        WaveData currentWave = WavesDatabase.Waves[_currentWaveIndex];

        _playableDirector.playableAsset = currentWave.TimelineAsset;
        _playableDirector.Play();
    }
    
    public void IncrementWaveIndex()
    {
        _currentWaveIndex++;
    }
}