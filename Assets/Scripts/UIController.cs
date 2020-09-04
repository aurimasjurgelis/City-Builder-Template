using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
	private Action<string> OnBuildAreaHandler;
	private Action<string> OnBuildSingleStructureHandler;
	private Action<string> OnBuildRoadHandler;

	private Action OnCancelActionHandler;
	private Action OnConfirmActionHandler;
	private Action OnDemolishActionHandler;

	public StructureRepository structureRepository;
	public Button buildResidentialAreaButton;
	public Button cancelActionButton;
	public Button confirmActionButton;

	public GameObject cancelActionPanel;

	public GameObject buildingMenuPanel;
	public Button openBuildMenuButton;
	public Button demolishButton;

	public GameObject zonesPanel;
	public GameObject facilitiesPanel;
	public GameObject roadsPanel;
	public Button closeBuildMenuButton;

	public GameObject buildButtonPrefab;



	//Economy
	public TextMeshProUGUI moneyValue;
	public TextMeshProUGUI populationValue;

	public UIStructureInfoPanelHelper structurePanelHelper;

	public void HideStructureInfo()
	{
		structurePanelHelper.Hide();
	}

	private void Start()
	{


		cancelActionPanel.SetActive(false);
		buildingMenuPanel.SetActive(false);

		//buildResidentialAreaButton.onClick.AddListener(OnBuildAreaCallback);
		cancelActionButton.onClick.AddListener(OnCancelActionCallback);
		confirmActionButton.onClick.AddListener(OnConfirmActionCallback);

		openBuildMenuButton.onClick.AddListener(OnOpenBuildMenu);
		demolishButton.onClick.AddListener(OnDemolishHandler);
		closeBuildMenuButton.onClick.AddListener(OnCloseMenuHandler);
	}

    public bool GetStructureInfoVisibility()
    {
		return structurePanelHelper.gameObject.activeSelf;
    }

    public void SetMoneyValue(int money)
	{
		moneyValue.text = money.ToString();
	}
	private void Update()
	{
		
	}


	public void DisplayBasicStructureInfo(StructureBaseSO data)
	{
		structurePanelHelper.DisplayBasicStructureInfo(data);
	}

	public void DisplayZoneStructureInfo(ZoneStructureSO data)
	{
		structurePanelHelper.DisplayZoneStructureInfo(data);
	}

	public void DisplayFacilityStructureInfo(SingleFacilitySO data)
	{
		structurePanelHelper.DisplayFacilityStructureInfo(data);
	}



	private void OnConfirmActionCallback()
	{

		cancelActionPanel.SetActive(false);
		OnConfirmActionHandler?.Invoke();
	}

	private void OnCloseMenuHandler()
	{
		AudioManager.Instance.PlayButtonClickedSound();
		buildingMenuPanel.SetActive(false);
	}

	private void OnDemolishHandler()
	{
		AudioManager.Instance.PlayButtonClickedSound();
		OnDemolishActionHandler?.Invoke();
		cancelActionPanel.SetActive(true);
		OnCloseMenuHandler();
	}

	public void SetPopulationValue(int population)
	{
		populationValue.text = population.ToString();
	}

	private void OnOpenBuildMenu()
	{
		AudioManager.Instance.PlayButtonClickedSound();
		buildingMenuPanel.SetActive(true);
		PrepareBuildMenu();
	}


	private void PrepareBuildMenu()
	{
		CreateButtonsInPanel(zonesPanel.transform, structureRepository.GetZoneNames(), OnBuildAreaCallback);
		CreateButtonsInPanel(facilitiesPanel.transform, structureRepository.GetSingleStructureNames(), OnBuildSingleStructureCallback);
		CreateButtonsInPanel(roadsPanel.transform, new List<string>() { structureRepository.GetRoadStructureName() }, OnBuildRoadCallback);
	}

	private void CreateButtonsInPanel(Transform panelTransform, List<string> dataToShow, Action<string> callback)
	{
		/*		foreach (Transform child in panelTransform)
				{
					var button = child.GetComponent<Button>();
					if (button != null)
					{
						button.onClick.AddListener(OnBuildAreaCallback);
					}
				}*/

		if(dataToShow.Count > panelTransform.childCount)
		{
			int quantityDifference = dataToShow.Count - panelTransform.childCount;
			for(int i = 0; i < quantityDifference; i++)
			{
				Instantiate(buildButtonPrefab, panelTransform);
			}
		}
		for (int i = 0; i < panelTransform.childCount; i++)
		{
			var button = panelTransform.GetChild(i).GetComponent<Button>();
			if(button != null)
			{
				button.GetComponentInChildren<TextMeshProUGUI>().text = dataToShow[i];
				button.onClick.AddListener(() => callback(button.GetComponentInChildren<TextMeshProUGUI>().text));
			}
		}

	}

	private void OnBuildAreaCallback(string nameOfStructure)
	{
		PrepareUIForBuilding();
		OnBuildAreaHandler?.Invoke(nameOfStructure);
	}
	private void OnBuildRoadCallback(string nameOfStructure)
	{
		PrepareUIForBuilding();
		OnBuildRoadHandler?.Invoke(nameOfStructure);

	}
	private void OnBuildSingleStructureCallback(string nameOfStructure)
	{
		PrepareUIForBuilding();
		OnBuildSingleStructureHandler?.Invoke(nameOfStructure);
	}

	private void PrepareUIForBuilding()
	{
		cancelActionPanel.SetActive(true);
		OnCloseMenuHandler();
	}

	private void OnCancelActionCallback()
	{
		AudioManager.Instance.PlayButtonClickedSound();
		cancelActionPanel.SetActive(false);
		OnCancelActionHandler?.Invoke();
	}



	public void AddListenerOnBuildAreaEvent(Action<string> listener)
	{
		OnBuildAreaHandler += listener;
	}

	public void RemoveListenerOnBuildAreaEvent(Action<string> listener)
	{
		OnBuildAreaHandler -= listener;
	}
	public void AddListenerOnBuildSingleStructureEvent(Action<string> listener)
	{
		OnBuildSingleStructureHandler += listener;
	}

	public void RemoveListenerOnBuildSingleStructureEvent(Action<string> listener)
	{
		OnBuildSingleStructureHandler -= listener;
	}
	public void AddListenerOnBuildRoadEvent(Action<string> listener)
	{
		OnBuildRoadHandler += listener;
	}

	public void RemoveListenerOnBuildRoadHandlerEvent(Action<string> listener)
	{
		OnBuildRoadHandler -= listener;
	}


	public void AddListenerOnCancelActionEvent(Action listener)
	{
		OnCancelActionHandler += listener;
	}

	public void RemoveListenerOnCancelActionEvent(Action listener)
	{
		OnCancelActionHandler -= listener;
	}


	public void AddListenerOnConfirmActionEvent(Action listener)
	{
		OnConfirmActionHandler += listener;
	}

	public void RemoveListenerOnConfirmActionEvent(Action listener)
	{
		OnConfirmActionHandler -= listener;
	}

	public void AddListenerOnDemolishActionEvent(Action listener)
	{
		OnDemolishActionHandler += listener;
	}

	public void RemoveListenerOnDemolishActionEvent(Action listener)
	{
		OnDemolishActionHandler -= listener;
	}


}
