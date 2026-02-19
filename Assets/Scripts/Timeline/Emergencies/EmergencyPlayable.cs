using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class EmergencyPlayable : PlayableBehaviour
{
    public List<EmergencySpawnData> SpawnData { get; set; }
    
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        foreach (var spawnData in SpawnData)
        {
            Vector2 spawnLocation = ServiceLocator.Instance.LocationsDatabase.GetLocationByName(spawnData.LocationName).Coordinates;
            ServiceLocator.Instance.EmergenciesManager.EmergencyFactory.SpawnEmergency(spawnData.EmergencyType, spawnLocation);
        }   
    }
}