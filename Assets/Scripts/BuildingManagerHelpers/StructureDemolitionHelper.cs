﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureDemolitionHelper : StructureModificationHelper
{
	Dictionary<Vector3Int, GameObject> roadToDemolish = new Dictionary<Vector3Int, GameObject>();
	public StructureDemolitionHelper(StructureRepository structureRepository, GridStructure grid, PlacementManager placementManager, IResourceManager resourceManager) : base(structureRepository, grid, placementManager, resourceManager)
	{
	}

	public override void CancelModifications()
	{
		foreach(var item in structuresToBeModified)
		{
			resourceManager.AddMoney(resourceManager.demolitionPrice);
		}
		this.placementManager.PlaceStructuresOnTheMap(structuresToBeModified.Values);
		structuresToBeModified.Clear();
	}

	public override void ConfirmModifications()
	{
		foreach (var gridPosition in structuresToBeModified.Keys)
		{
			PrepareStructureForDemolition(gridPosition);
			grid.RemoveStructureFromTheGrid(gridPosition);
		}
		var roadPlacementHelper = StructureModificationFactory.GetHelper(typeof(PlayerBuildingRoadState));
		foreach (var keyValuePair in roadToDemolish)
		{
			Dictionary<Vector3Int, GameObject> neighboursDictionary = RoadManager.GetRoadNeighboursForPosition(grid, keyValuePair.Key);
			if(neighboursDictionary.Count > 0)
			{
				var structureData = grid.GetStructureDataFromTheGrid(neighboursDictionary.Keys.First());
				((RoadPlacementModificationHelper)roadPlacementHelper).ModifyRoadCellsOnTheGrid(neighboursDictionary, structureData);
			}
		}
		this.placementManager.DestroyStructures(structuresToBeModified.Values);
		structuresToBeModified.Clear();
	}

	private void PrepareStructureForDemolition(Vector3Int gridPosition)
	{
		var data = grid.GetStructureDataFromTheGrid(gridPosition);
		if(data != null)
		{
			Type dataType = data.GetType();
			if(dataType == typeof(ZoneStructureSO) && ((ZoneStructureSO)data).zoneType == ZoneType.Residential)
			{
				resourceManager.ReducePopulation(1);
			}
			StructureEconomyManager.DemolitionStructureLogic(dataType, gridPosition, grid);
		}
	}

	public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
	{
		Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
		if (grid.IsCellTaken(gridPosition))
		{
			var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
			var structure = grid.GetStructureFromTheGrid(gridPosition);
			if (structuresToBeModified.ContainsKey(gridPositionInt))
			{
				resourceManager.AddMoney(resourceManager.demolitionPrice);
				RevokeStructureDemolitionAt(gridPositionInt, structure);
			}
			else if(resourceManager.CanIBuyIt(resourceManager.demolitionPrice))
			{
				AddStructureForDemolition(gridPositionInt, structure);
				resourceManager.SpendMoney(resourceManager.demolitionPrice);
			}
		}
	}

	private void AddStructureForDemolition(Vector3Int gridPositionInt, GameObject structure)
	{
		structuresToBeModified.Add(gridPositionInt, structure);
		placementManager.SetBuildingForDemolition(structure);
		if(RoadManager.CheckIfNeighbourIsRoadOnTheGrid(grid,gridPositionInt) && roadToDemolish.ContainsKey(gridPositionInt) == false)
		{
			roadToDemolish.Add(gridPositionInt, structure);
		}
	}

	private void RevokeStructureDemolitionAt(Vector3Int gridPositionInt, GameObject structure)
	{
		placementManager.ResetBuildingMaterials(structure);
		structuresToBeModified.Remove(gridPositionInt);
		if (RoadManager.CheckIfNeighbourIsRoadOnTheGrid(grid, gridPositionInt) && roadToDemolish.ContainsKey(gridPositionInt) == true)
		{
			roadToDemolish.Remove(gridPositionInt);
		}
	}




}