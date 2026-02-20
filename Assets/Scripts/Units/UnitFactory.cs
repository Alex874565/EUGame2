using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public GameObject SpawnUnit(UnitType unitType)
    {
        GameObject unitPrefab = ServiceLocator.Instance.UnitsDatabase.Units.Find(u => u.Data.Type == unitType).Prefab;
        GameObject unit = Instantiate(unitPrefab);
        return unit;
    }
    
    public GameObject SpawnPlacementUnit(UnitType unitType)
    {
        GameObject unitPrefab = ServiceLocator.Instance.UnitsDatabase.Units.Find(u => u.Data.Type == unitType).Prefab;
        GameObject ghostUnit = Instantiate(unitPrefab);
        ghostUnit.AddComponent<PlacementController>();
        return ghostUnit;
    }
    
    public GameObject SpawnMovingUnit(UnitType unitType)
    {
        GameObject unitPrefab = ServiceLocator.Instance.UnitsDatabase.Units.Find(u => u.Data.Type == unitType).Prefab;
        GameObject unit = Instantiate(unitPrefab);
        unit.AddComponent<MovementController>();
        return unit;
    }
}