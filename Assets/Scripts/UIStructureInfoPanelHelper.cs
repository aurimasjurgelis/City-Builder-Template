using System.CodeDom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStructureInfoPanelHelper : MonoBehaviour
{
	public TextMeshProUGUI nameText, incomeText, upkeepText, clientsText;
	public Toggle powerToggle, waterToggle, roadToggle;

	private void Start()
	{
		Hide();
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void DisplayBasicStructureInfo(StructureBaseSO data)
	{
		Show();
		HideElement(clientsText.gameObject);
		HideElement(powerToggle.gameObject);
		HideElement(waterToggle.gameObject);
		HideElement(roadToggle.gameObject);
		SetText(nameText, data.buildingName);
		SetText(incomeText, data.GetIncome().ToString());
		SetText(upkeepText, data.upkeepCost.ToString());
	}

	public void DisplayZoneStructureInfo(ZoneStructureSO data)
	{
		Show();
		HideElement(clientsText.gameObject);

		SetText(nameText, data.buildingName);
		SetText(incomeText, data.GetIncome().ToString());
		SetText(upkeepText, data.upkeepCost.ToString());

		if (data.requirePower)
		{
			SetToggle(powerToggle, data.HasPower());
		}
		else
		{
			HideElement(powerToggle.gameObject);
		}
		if (data.requireWater)
		{
			SetToggle(waterToggle, data.HasWater());
		}
		else
		{
			HideElement(waterToggle.gameObject);
		}
		if (data.requireRoadAccess)
		{
			SetToggle(roadToggle, data.HasRoadAccess());
		}
		else
		{
			HideElement(roadToggle.gameObject);
		}
	}


	public void DisplayFacilityStructureInfo(SingleFacilitySO data)
	{
		Show();
		HideElement(clientsText.gameObject);

		SetText(nameText, data.buildingName);
		SetText(incomeText, data.GetIncome().ToString());
		SetText(upkeepText, data.upkeepCost.ToString());

		if (data.requirePower)
		{
			SetToggle(powerToggle, data.HasPower());
		}
		else
		{
			HideElement(powerToggle.gameObject);
		}
		if (data.requireWater)
		{
			SetToggle(waterToggle, data.HasWater());
		}
		else
		{
			HideElement(waterToggle.gameObject);
		}
		if (data.requireRoadAccess)
		{
			SetToggle(roadToggle, data.HasRoadAccess());
		}
		else
		{
			HideElement(roadToggle.gameObject);
		}


		SetText(clientsText, data.GetNumberOfCustomers() + "/" + data.maxCustomers);
	}

	private void HideElement(GameObject element)
	{
		element.transform.parent.gameObject.SetActive(false);
	}

	private void ShowElement(GameObject element)
	{
		element.transform.parent.gameObject.SetActive(true);
	}

	private void SetText(TextMeshProUGUI element, string value)
	{
		ShowElement(element.gameObject);
		element.text = value;
	}

	private void SetToggle(Toggle element, bool value)
	{
		ShowElement(element.gameObject);
		element.isOn = value;
	}
}
