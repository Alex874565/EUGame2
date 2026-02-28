using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
public class SigningMinigameController : MinigameController
{
    [SerializeField] private StampBehaviour stamp;
    [FormerlySerializedAs("PetitionsPrefabs")] [SerializeField] private List<GameObject> petitionsPrefabs;
    [SerializeField] private Vector2 petitionSpawnInterval;
    
    private Collider2D _spawnArea;
    
    private float _timeSinceLastSpawn;
    private float _currentSpawnInterval;
    
    private List<GameObject> _activePetitions;
    
    private void Awake()
    {
        _spawnArea = GetComponent<Collider2D>();
        _activePetitions = new List<GameObject>();
        base.Awake();
    }

    private void Update()
    {
        base.Update();
        
        if (!GamePlaying) return;
        
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn >= _currentSpawnInterval)
        {
            SpawnPetition();
            _timeSinceLastSpawn = 0f;
        }
    }
    
    public override void StartMinigame()
    {
        base.StartMinigame();
        SpawnPetition();
    }
    
    private void SpawnPetition()
    {
        bool spawnLeft = Random.value < 0.5f;
        float spawnX = spawnLeft ? _spawnArea.bounds.min.x : _spawnArea.bounds.max.x;
        float spawnY = Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        Vector3 spawnRotation = new Vector3(0f, 0f, Random.Range(0f, 360f));
        GameObject petition = petitionsPrefabs[Random.Range(0, petitionsPrefabs.Count)];
        GameObject spawnedPetition = Instantiate(petition, spawnPosition, Quaternion.Euler(spawnRotation), transform);
        spawnedPetition.GetComponent<PetitionBehaviour>().StartMoving(spawnLeft ? Vector2.right : Vector2.left);
        spawnedPetition.GetComponent<PetitionBehaviour>().OnPetitionSigned += OnPetitionSigned;
        spawnedPetition.GetComponent<PetitionBehaviour>().Stamp = stamp;
        spawnedPetition.transform.SetAsFirstSibling();
        _activePetitions.Add(spawnedPetition);
        _currentSpawnInterval = Random.Range(petitionSpawnInterval.x, petitionSpawnInterval.y);
    }
    
    private void OnPetitionSigned()
    {
        AddScore(1);
    }

    protected override void Cleanup()
    {
        foreach (GameObject activePetition in _activePetitions)
        {
            if (activePetition != null)
            {
                Destroy(activePetition);
            }
        }
        _activePetitions.Clear();
    }
}