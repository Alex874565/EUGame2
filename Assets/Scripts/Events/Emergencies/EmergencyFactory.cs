using UnityEngine;

public class EmergencyFactory : MonoBehaviour
{
    public void SpawnEmergency(EmergencyType emergencyType, LocationData spawnLocationData)
    {
        GameObject emergenciesLayer = ServiceLocator.Instance.UIManager.EmergenciesLayer;
        GameObject emergencyPrefab = ServiceLocator.Instance.EmergenciesDatabase.Emergencies.Find(emergency => emergency.EmergencyData.EmergencyType == emergencyType).EmergencyPrefab;
        GameObject emergency = Instantiate(emergencyPrefab, spawnLocationData.Coordinates, Quaternion.identity, emergenciesLayer.transform);
        EmergencyBehaviour emergencyBhvr = emergency.GetComponent<EmergencyBehaviour>();
        emergencyBhvr.LocationData = spawnLocationData;
        // When the emergency is completed, eliminate it from the EmergenciesManager
        // so its reference doesn't remain in the emergencies list.
        emergencyBhvr.OnCompletedEmergency += () =>
        ServiceLocator.Instance.EmergenciesManager.EliminateEmergency(emergency);
    }
}