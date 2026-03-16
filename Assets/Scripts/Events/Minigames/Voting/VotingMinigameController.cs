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

    [field: Header("Votes Spawning Settings")]
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(1f, 3f);
    [SerializeField] private GameObject votePrefab;

    [Header("Camera Compensation")]
    [SerializeField] private float referenceOrthographicSize = 5f;

    private float _spawnTimer;
    private float _currentSpawnInterval;

    private Collider2D _spawnArea;
    private List<GameObject> _activeVotes = new();

    private Camera _uiCamera;

    private void Awake()
    {
        _spawnArea = GetComponent<Collider2D>();

        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null && parentCanvas.worldCamera != null)
        {
            _uiCamera = parentCanvas.worldCamera;
        }
        else
        {
            _uiCamera = Camera.main;
        }

        base.Awake();
    }

    private void OnEnable()
    {
        _spawnTimer = 0f;
        StartMinigame();
    }

    private new void Update()
    {
        if (!GamePlaying) return;

        base.Update();

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _currentSpawnInterval)
        {
            SpawnVote();
        }
    }

    public override void StartMinigame()
    {
        _currentSpawnInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
        urn.Minigame = this;

        base.StartMinigame();
    }

    private float GetCameraScaleMultiplier()
    {
        if (_uiCamera == null || !_uiCamera.orthographic)
            return 1f;

        return _uiCamera.orthographicSize / referenceOrthographicSize;
    }

    private Bounds GetCompensatedSpawnBounds()
    {
        Bounds sourceBounds = _spawnArea.bounds;
        float multiplier = GetCameraScaleMultiplier();

        Vector3 scaledSize = new Vector3(
            sourceBounds.size.x,
            sourceBounds.size.y,
            sourceBounds.size.z
        );

        return new Bounds(sourceBounds.center, scaledSize);
    }

    private void SpawnVote()
    {
        Bounds spawnBounds = GetCompensatedSpawnBounds();

        bool spawnLeft = Random.value > 0.5f;
        float spawnX = spawnLeft ? spawnBounds.min.x : spawnBounds.max.x;
        float spawnY = Random.Range(spawnBounds.min.y, spawnBounds.max.y);

        GameObject vote = Instantiate(
            votePrefab,
            new Vector3(spawnX, spawnY, 0f),
            Quaternion.identity,
            transform
        );

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

    protected override void Cleanup()
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