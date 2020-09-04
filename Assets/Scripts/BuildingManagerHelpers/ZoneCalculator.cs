using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ZoneCalculator 
{
	public static bool CheckIfPositionHasChanged(Vector3 inputPosition, Vector3 previousPosition, GridStructure grid)
	{
		return Vector3Int.FloorToInt(grid.CalculateGridPosition(inputPosition))
			.Equals(Vector3Int.FloorToInt(grid.CalculateGridPosition(previousPosition))) == false;
	}

	public static void PrepareStartAndEndPoints(Vector3 startPoint, Vector3 endPoint, ref Vector3Int minPoint, ref Vector3Int maxPoint, Vector3 mapBottomLeftCorner)
	{
		Vector3 startPositionForCalculations = new Vector3(startPoint.x, 0, startPoint.z);
		Vector3 endPositionForCalculation = new Vector3(endPoint.x, 0, endPoint.z);

		if((startPoint.z > endPoint.z && startPoint.x < endPoint.x) || (startPoint.z < endPoint.z && startPoint.x > endPoint.x)) 
		{
			startPositionForCalculations = new Vector3(startPoint.x, 0, endPoint.z);
			endPositionForCalculation = new Vector3(endPoint.x, 0, startPoint.z);
		}

		var startPointDistance = Mathf.Abs(Vector3.Distance(mapBottomLeftCorner, startPositionForCalculations));
		var endPointDistance = Mathf.Abs(Vector3.Distance(mapBottomLeftCorner, endPositionForCalculation));

		minPoint = Vector3Int.FloorToInt(startPointDistance < endPointDistance ? startPositionForCalculations : endPositionForCalculation);
		maxPoint = Vector3Int.FloorToInt(startPointDistance >= endPointDistance ? startPositionForCalculations : endPositionForCalculation);
		
	}

	public static void CalculateZone(HashSet<Vector3Int> newPositionsSet, Dictionary<Vector3Int, GameObject> structuresToBeModified,
		Queue<GameObject> gameObjectsToReuse)
	{
		HashSet<Vector3Int> existingStructuresPositions = new HashSet<Vector3Int>(structuresToBeModified.Keys);
		existingStructuresPositions.IntersectWith(newPositionsSet);
		HashSet<Vector3Int> structuresPositionsToDisable = new HashSet<Vector3Int>(structuresToBeModified.Keys);
		structuresPositionsToDisable.ExceptWith(newPositionsSet);

		foreach(var positionToDisable in structuresPositionsToDisable)
		{
			var structure = structuresToBeModified[positionToDisable];
			structure.SetActive(false);
			gameObjectsToReuse.Enqueue(structure);
			structuresToBeModified.Remove(positionToDisable);
		}

		foreach(var position in existingStructuresPositions)
		{
			newPositionsSet.Remove(position);
		}
	}
}
