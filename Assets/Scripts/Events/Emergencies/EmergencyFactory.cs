using UnityEngine;

public class EmergencyFactory : MonoBehaviour
{
    public void SpawnEmergency(EmergencyType emergencyType, Vector3 position)
    {
        GameObject emergencyPrefab = ServiceLocator.Instance.EmergenciesDatabase.Emergencies.Find(emergency => emergency.EmergencyData.EmergencyType == emergencyType).EmergencyPrefab;
        Instantiate(emergencyPrefab, position, Quaternion.identity);
    }
}