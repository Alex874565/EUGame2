using System;
using UnityEngine;

public class MinigameBehaviour : MonoBehaviour
{
    [SerializeField] private MinigameType minigameType;

    public MinigameData MinigameData { get; private set; }

    private float _timerOnMap;
    private bool _isPlaying;
    
    private void Start()
    {
        MinigameData = ServiceLocator.Instance.MinigamesManager.MinigamesStats[minigameType];
        ServiceLocator.Instance.MinigamesManager.ActiveMinigames.Add(gameObject);
    }

    public void Update()
    {
        if (!_isPlaying)
        {
            _timerOnMap += Time.deltaTime;
            if(_timerOnMap >= MinigameData.TimeOnMap)
            {
                DestroySelf();
            }
        }
    }

    private void DestroySelf()
    {
        ServiceLocator.Instance.MinigamesManager.ActiveMinigames.Remove(gameObject);
        Destroy(gameObject);
    }
}