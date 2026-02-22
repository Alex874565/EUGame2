using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public GameObject SpawnUnit(UnitType unitType)
    {
        GameObject unitPrefab = ServiceLocator.Instance.UnitsDatabase.Units.Find(u => u.Data.Type == unitType).Prefab;
        GameObject unit = Instantiate(unitPrefab);
        ServiceLocator.Instance.UnitsManager.ActiveUnits.Add(unit);
        return unit;
    }
    
    public GameObject SpawnPlacementUnit(UnitType unitType, LocationName startLocation)
    {
        GameObject unitPrefab = ServiceLocator.Instance.UnitsDatabase.Units.Find(u => u.Data.Type == unitType).Prefab;
        GameObject ghostUnit = Instantiate(unitPrefab);
        Vector2 startPosition = ServiceLocator.Instance.LocationsDatabase.Locations.Find(u => u.Name == startLocation).Coordinates;
        ghostUnit.AddComponent<PlacementController>();
        ghostUnit.GetComponent<PlacementController>().StartPosition = startPosition; 
        ghostUnit.GetComponent<UnitBehaviour>().IsInteractable = false;
        ServiceLocator.Instance.UnitsManager.ActiveUnits.Add(ghostUnit);
        return ghostUnit;
    }
    
    public GameObject SpawnMovingUnit(UnitType unitType)
    {
        GameObject unitPrefab = ServiceLocator.Instance.UnitsDatabase.Units.Find(u => u.Data.Type == unitType).Prefab;
        GameObject unit = Instantiate(unitPrefab);
        unit.AddComponent<MovementController>();
        unit.GetComponent<UnitBehaviour>().IsInteractable = false;
        ServiceLocator.Instance.UnitsManager.ActiveUnits.Add(unit);
        return unit;
    }
}