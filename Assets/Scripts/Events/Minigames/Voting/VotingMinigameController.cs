using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class VotingMinigameController : MinigameController
{
    [SerializeField] private Vector2 votesToWinRange = new Vector2(10, 20);
    [SerializeField] private UrnBehaviour urn;
    
    [Header("UI Settings")]
    [SerializeField] private TMP_Text counter;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
        
    [field: Header("Votes Spawning Settings")]
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(1f, 3f);
    [SerializeField] private GameObject votePrefab;
    
    private float _minigameTimer;
    private float _votesCollected;
    private int _votesToWin;
    private bool _gamePlaying;
    
    private float _spawnTimer;
    private float _currentSpawnInterval;
    
    private Collider2D _spawnArea;
    private Bounds _spawnAreaBounds;
    
    private List<GameObject> _activeVotes;

    private void Awake()
    {
        _spawnArea = GetComponent<Collider2D>();
        _activeVotes = new List<GameObject>();
    }
    
    private void Update()
    {
        if (!_gamePlaying) return;
        
        _minigameTimer += Time.deltaTime;
        timer.text = $"{Mathf.CeilToInt(Data.TimeLimit - _minigameTimer)}s";
        if (_minigameTimer >= Data.TimeLimit)
        {
            StartCoroutine(End(false));
        }
        
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _currentSpawnInterval)
        {
            SpawnVote();
        }
    }
    
    public override void StartMinigame()
    {
        _spawnAreaBounds = _spawnArea.bounds;
        
        _currentSpawnInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
        _votesToWin = Random.Range((int)votesToWinRange.x, (int)votesToWinRange.y + 1);
        
        counter.text = $"0/{_votesToWin}";
        
        urn.Minigame = this;
        
        _gamePlaying = true;
    }

    private void SpawnVote()
    {
        bool spawnLeft = Random.value > 0.5f;
        float spawnX = spawnLeft ? _spawnAreaBounds.min.x : _spawnAreaBounds.max.x;
        float spawnY = Random.Range(_spawnAreaBounds.min.y, _spawnAreaBounds.max.y);
        GameObject vote = Instantiate(votePrefab, new Vector3(spawnX, spawnY, 0f), Quaternion.identity, transform);
        vote.GetComponent<VoteBehaviour>().StartMoving(spawnLeft);
        _activeVotes.Add(vote);
        
        _spawnTimer = 0f;
        _currentSpawnInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
    }
    
    public void RegisterVote(GameObject vote)
    {
        _votesCollected += 1;
        counter.text = $"{_votesCollected}/{_votesToWin}";
        if(_votesCollected >= _votesToWin)
        {
            StartCoroutine(End(true));
        }
        
        _activeVotes.Remove(vote);
        Destroy(vote);
    }
    
    private IEnumerator End(bool won)
    {
        _gamePlaying = false;
        
        foreach (var vote in _activeVotes)
        {
            Destroy(vote);
        }
        _activeVotes.Clear();
        
        ServiceLocator.Instance.MinigamesManager.CloseMinigame();
        
        if (won)
        {
            winUI.SetActive(true);
        }
        else
        {
            loseUI.SetActive(true);
        }
        
        yield return new WaitForSecondsRealtime(1f);
    }
}