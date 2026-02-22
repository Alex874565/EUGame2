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
            LocationData spawnLocation = ServiceLocator.Instance.LocationsDatabase.GetLocationByName(spawnData.LocationName);
            ServiceLocator.Instance.EmergenciesManager.EmergencyFactory.SpawnEmergency(spawnData.EmergencyType, spawnLocation);
        }   
    }
}