﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public PlacementManager placementManager;
	public StructureRepository structureRepository;
	public IInputManager inputManager;
	public UIController uiController;

	public int width, length;
	public CameraMovement cameraMovement;

	public LayerMask inputMask;
	private BuildingManager buildingManager;


	private GridStructure grid;
	private int cellSize = 3;


	private PlayerState state;

	public PlayerSelectionState selectionState;
	public PlayerBuildingSingleStructureState buildingSingleStructureState;
	public PlayerDemolitionState demolishState;
	public PlayerBuildingRoadState buildingRoadState;
	public PlayerBuildingZoneState buildingAreaState;

	public PlayerState State { get => state; }

	public ResourceManager resourceManager;
	//public GameObject resourceManagerGameObject;

	private void Awake()
	{
		PrepareStates();

#if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
		inputManager = gameObject.AddComponent<InputManager>();
#endif

#if (UNITY_IOS || UNITY_ANDROID)
		
#endif
	}

	private void PrepareStates()
	{
		buildingManager = new BuildingManager(cellSize, width, length, placementManager, structureRepository, resourceManager);
		resourceManager.PrepareResourceManager(buildingManager);
		selectionState = new PlayerSelectionState(this, buildingManager);
		demolishState = new PlayerDemolitionState(this, buildingManager);
		buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, buildingManager);
		buildingAreaState = new PlayerBuildingZoneState(this, buildingManager);
		buildingRoadState = new PlayerBuildingRoadState(this, buildingManager);
		state = selectionState;
	}



	// Start is called before the first frame update
	void Start()
	{

		//resourceManager = resourceManagerGameObject.GetComponent<IResourceManager>();
		PrepareGameComponents();
		AssignInputListeners();
		AssignUIControllerListeners();
	}

	private void PrepareGameComponents()
	{
		inputManager.MouseInputMask = inputMask;
		cameraMovement.SetCameraLimits(0, width, 0, length);
	}

	private void AssignUIControllerListeners()
	{
		uiController.AddListenerOnBuildAreaEvent((structureName) => state.OnBuildArea(structureName));
		uiController.AddListenerOnBuildSingleStructureEvent((structureName) => state.OnBuildSingleStructure(structureName));
		uiController.AddListenerOnBuildRoadEvent((structureName) => state.OnBuildRoad(structureName));
		uiController.AddListenerOnCancelActionEvent(() => state.OnCancel());
		uiController.AddListenerOnDemolishActionEvent(() => state.OnDemolishAction());
		uiController.AddListenerOnConfirmActionEvent(() => state.OnConfirmAction());
	}

	private void AssignInputListeners()
	{
		inputManager.AddListenerOnPointerDownEvent((position) => state.OnInputPointerDown(position));
		inputManager.AddListenerOnPointerSecondChangeEvent((position) => state.OnInputPanChange(position));
		inputManager.AddListenerOnPointerSecondUpEvent(() => state.OnInputPanUp());
		inputManager.AddListenerOnPointerChangeEvent((position) => state.OnInputPointerChange(position));
		inputManager.AddListenerOnPointerUpEvent(() => state.OnInputPointerUp());
	}



	public void TransitionToState(PlayerState newState, string variable)
	{

		this.state = newState;
		this.state.EnterState(variable);
	}
}