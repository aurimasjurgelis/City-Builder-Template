using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingZoneState : PlayerState
{
	string structureName;
	BuildingManager buildingManager;
	public PlayerBuildingZoneState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager)
	{
		this.buildingManager = buildingManager;

	}

	public override void OnConfirmAction()
	{
		AudioManager.Instance.PlayPlaceBuildingSound();
		this.buildingManager.ConfirmModification();
		base.OnConfirmAction();
	}

	public override void OnBuildRoad(string structureName)
	{
		this.buildingManager.CancelModification();
		base.OnBuildRoad(structureName);
	}

	public override void OnBuildSingleStructure(string structureName)
	{
		this.buildingManager.CancelModification();
		base.OnBuildSingleStructure(structureName);
	}

	public override void OnDemolishAction()
	{
		this.buildingManager.CancelModification();
		base.OnDemolishAction();
	}


	public override void OnCancel()
	{
		this.buildingManager.CancelModification();//remove this if you want to change the design a little bit
		this.gameManager.TransitionToState(this.gameManager.selectionState, null);
	}

	public override void EnterState(string structureName)
	{
		this.buildingManager.PrepareBuildingManager(this.GetType());
		this.structureName = structureName;
	}
	public override void OnInputPointerDown(Vector3 position)
	{

		this.buildingManager.PrepareStructureForModification(position, structureName, StructureType.Zone);

	}

	public override void OnInputPointerChange(Vector3 position)
	{
		this.buildingManager.PrepareStructureForModification(position, structureName, StructureType.Zone);
		
	}

	public override void OnInputPointerUp()
	{
		this.buildingManager.StopContinuousPlacement();
	}
}
