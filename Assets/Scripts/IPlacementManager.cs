using System.Collections.Generic;
using UnityEngine;

public interface IPlacementManager
{
	GameObject CreateGhostStructure(Vector3 gridPosition, GameObject buildingPrefab, RotationValue rotationValue = RotationValue.R0);
	void DestroySingleStructure(GameObject structure);
	void DestroyStructures(IEnumerable<GameObject> structureCollection);
	GameObject PlaceStructureOnTheMap(Vector3 gridPosition, GameObject buildingPrefab, RotationValue rotationValue);
	void PlaceStructuresOnTheMap(IEnumerable<GameObject> structureCollection);
	void ResetBuildingMaterials(GameObject structure);
	void SetBuildingForDemolition(GameObject structureToDemolish);
	GameObject MoveStructureOnTheMap(Vector3Int positionToPlaceStructure, GameObject gameObjectToReuse, GameObject prefab);
}