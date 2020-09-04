using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Zone Structure", menuName = "City Builder/Structure Data/Zone Structure")]
public class ZoneStructureSO : StructureBaseSO
{
	public bool upgradable;
	public GameObject[] prefabVariants;
	public UpgradeType[] availableUpgrades;
	public ZoneType zoneType;
	public int maxFacilitySearchRange;
}

[System.Serializable]
public struct UpgradeType
{
	public GameObject[] prefabVariants;
	public int happinessThreshold;
	public int newIncome;
	public int newUpkeep;
}


public enum ZoneType
{
	Residential,
	Agricultural,
	Commercial
}
