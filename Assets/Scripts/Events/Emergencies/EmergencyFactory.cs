using UnityEngine;

public class EmergencyFactory : MonoBehaviour
{
    public void SpawnEmergency(EmergencyType emergencyType, LocationData spawnLocationData)
    {
        GameObject emergenciesLayer = ServiceLocator.Instance.UIManager.EmergenciesLayer;
        GameObject emergencyPrefab = ServiceLocator.Instance.EmergenciesDatabase.Emergencies.Find(emergency => emergency.EmergencyData.EmergencyType == emergencyType).EmergencyPrefab;
        GameObject emergency = Instantiate(emergencyPrefab, spawnLocationData.Coordinates, Quaternion.identity, emergenciesLayer.transform);
        emergency.GetComponent<EmergencyBehaviour>().LocationData = spawnLocationData;
    }
}