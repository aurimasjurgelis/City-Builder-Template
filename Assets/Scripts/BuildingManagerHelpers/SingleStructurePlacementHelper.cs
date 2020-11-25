using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleStructurePlacementHelper : StructureModificationHelper
{
	public SingleStructurePlacementHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, IResourceManager resourceManager) : base(structureRepository, grid, placementManager, resourceManager)
	{

	}

	public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
	{
		base.PrepareStructureForModification(inputPosition, structureName, structureType);
		GameObject buildingPrefab = structureData.prefab;
		Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
		var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
		if (grid.IsCellTaken(gridPosition) == false)
		{
			if (structuresToBeModified.ContainsKey(gridPositionInt))
			{
				resourceManager.AddMoney(structureData.placementCost);
				RevokeStructurePlacementAt(gridPositionInt);
			}
			else if(resourceManager.CanIBuyIt(structureData.placementCost))
			{
				PlaceNewStructureAt(buildingPrefab, gridPosition);
				resourceManager.SpendMoney(structureData.placementCost);
			}
		}
	}

	private void PlaceNewStructureAt(GameObject buildingPrefab, Vector3 gridPosition)
	{
		structuresToBeModified.Add(Vector3Int.FloorToInt(gridPosition), placementManager.CreateGhostStructure(gridPosition, buildingPrefab));
	}

	private void RevokeStructurePlacementAt(Vector3Int gridPositionInt)
	{
		var structure = structuresToBeModified[gridPositionInt];
		placementManager.DestroySingleStructure(structure);
		structuresToBeModified.Remove(gridPositionInt);
	}

	public override void ConfirmModifications()
	{
		placementManager.PlaceStructuresOnTheMap(structuresToBeModified.Values);
		foreach (var keyValuePair in structuresToBeModified)
		{
			grid.PlaceStructureOnTheGrid(keyValuePair.Value, keyValuePair.Key, structureData);
		}
		structuresToBeModified.Clear();
	}

	//public override void CancelModifications()
	//{
	//	Debug.Log("alskdjflsadf");
	//	//placementManager.DestroySingleStructure(structuresToBeModified.Values);
	//	structuresToBeModified.Clear();
	//}


	public override void CancelModifications()
	{
		resourceManager.AddMoney(structuresToBeModified.Count * structureData.placementCost);
		base.CancelModifications();
	}
}
