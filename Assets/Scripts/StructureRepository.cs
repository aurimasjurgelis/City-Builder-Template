﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureRepository : MonoBehaviour
{
	public CollectionSO modelDataCollection;

	public List<string> GetZoneNames()
	{
		return modelDataCollection.zoneList.Select(zone => zone.buildingName).ToList();
	}

	public List<string> GetSingleStructureNames()
	{
		return modelDataCollection.singleStructureList.Select(facility => facility.buildingName).ToList();
	}

	public string GetRoadStructureName()
	{
		return modelDataCollection.roadStructure.buildingName;
	
	}

	public GameObject GetBuildingPrefabByName(string structureName, StructureType structureType)
	{
		GameObject structurePrefabToReturn = null;
		switch (structureType)
		{
			case StructureType.Zone:
				structurePrefabToReturn = GetZoneBuildingPrefabByName(structureName);
				break;
			case StructureType.SingleStructure:
				structurePrefabToReturn = GetSingleStructureBuildingPrefabByName(structureName);
				break;
			case StructureType.Road:
				structurePrefabToReturn = GetRoadBuildingPrefab();
				break;
			default:
				throw new Exception("No such type implemented. " + structureType);
		}
		if(structurePrefabToReturn == null)
		{
			throw new Exception("No prefab for that name: " + structureName);
		}
		return structurePrefabToReturn;
	}

	private GameObject GetRoadBuildingPrefab()
	{
		return modelDataCollection.roadStructure.prefab;
	}

	private GameObject GetSingleStructureBuildingPrefabByName(string structureName)
	{
		var foundStructure = modelDataCollection.singleStructureList.Where(structure => structure.buildingName == structureName).FirstOrDefault();
		if(foundStructure != null)
		{
			return foundStructure.prefab;
		}
		return null;
	}

	public StructureBaseSO GetStructureData(string structureName, StructureType structureType)
	{
		switch (structureType)
		{
			case StructureType.Zone:
				return modelDataCollection.zoneList.Where(structure => structure.buildingName == structureName).FirstOrDefault();
			case StructureType.SingleStructure:
				return modelDataCollection.singleStructureList.Where(structure => structure.buildingName == structureName).FirstOrDefault();
			case StructureType.Road:
				return modelDataCollection.roadStructure;
			case StructureType.None:
				return null;
		}
		return null;

	}

	private GameObject GetZoneBuildingPrefabByName(string structureName)
	{
		var foundStructure = modelDataCollection.zoneList.Where(structure => structure.buildingName == structureName).FirstOrDefault();
		if (foundStructure != null)
		{
			return foundStructure.prefab;
		}
		return null;
	}
}

public enum StructureType
{ 
	Zone,
	SingleStructure,
	Road,
	None


}


