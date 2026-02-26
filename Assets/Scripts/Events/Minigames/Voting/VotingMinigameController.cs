using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class VotingMinigameController : MinigameController
{
    [SerializeField] private UrnBehaviour urn;
    
    [Header("UI Settings")]
    [SerializeField] private TMP_Text counter;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
        
    [field: Header("Votes Spawning Settings")]
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(1f, 3f);
    [SerializeField] private GameObject votePrefab;
    
    private float _spawnTimer;
    private float _currentSpawnInterval;
    
    private Collider2D _spawnArea;
    private Bounds _spawnAreaBounds;
    
    private List<GameObject> _activeVotes;

    private void Start()
    {
        _spawnArea = GetComponent<Collider2D>();
        _activeVotes = new List<GameObject>();
        
        base.Start();
    }
    
    private void Update()
    {
        base.Update();
        
        if (!GamePlaying) return;
        
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
        
        urn.Minigame = this;
        
        base.StartMinigame();
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
        AddScore(1);
        
        _activeVotes.Remove(vote);
        Destroy(vote);
    }
    
    protected  override void Cleanup()
    {
        foreach (GameObject vote in _activeVotes)
        {
            if (vote != null)
            {
                Destroy(vote);
            }
        }
        _activeVotes.Clear();
    }
}