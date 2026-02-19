using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public GameObject SpawnUnit(UnitType unitType)
    {
        GameObject unitPrefab = ServiceLocator.Instance.UnitsDatabase.Units.Find(u => u.Data.Type == unitType).Prefab;
        GameObject unit = Instantiate(unitPrefab);
        return unit;
    }
}