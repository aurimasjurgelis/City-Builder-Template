using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ResourceManager : MonoBehaviour, IResourceManager
{
	[SerializeField]
	private int startMoneyAmount = 5000;
	[SerializeField]
	private int demolitionPrice = 20;
	[SerializeField]
	private float moneyCalculationInterval = 2;
	MoneyHelper moneyHelper;
	PopulationHelper populationHelper;

	private BuildingManager buildingManager;
	public UIController uiController;

	public int StartMoneyAmount { get => startMoneyAmount;  }
	public float MoneyCalculationInterval { get => moneyCalculationInterval;  }
	int IResourceManager.demolitionPrice { get => demolitionPrice; }

	private void Start()
	{
		moneyHelper = new MoneyHelper(startMoneyAmount);
		populationHelper = new PopulationHelper();
		UpdateUI();
	}

	public void PrepareResourceManager(BuildingManager buildingManager)
	{
		this.buildingManager = buildingManager;
		InvokeRepeating("CalculateTownIncome", 0, moneyCalculationInterval);
	}

	public bool SpendMoney(int amount)
	{
		if (CanIBuyIt(amount))
		{
			try
			{
				moneyHelper.ReduceMoney(amount);
				UpdateUI();
				return true;
			}
			catch (MoneyException)
			{

				ReloadGame();
			}
		}
		return false;
	}

	private void ReloadGame()
	{
		Debug.Log("End the game");
	}

	public bool CanIBuyIt(int amount)
	{
		return (moneyHelper.Money >= amount) ? true : false;
	}


	public void CalculateTownIncome()
	{
		try
		{
			moneyHelper.CalculateMoney(buildingManager.GetAllStructures());
			UpdateUI();
		}
		catch (MoneyException)
		{
			ReloadGame();
		}
	}

	private void OnDisable()
	{
		CancelInvoke();
	}

	public void AddMoney(int amount)
	{
		moneyHelper.AddMoney(amount);
		UpdateUI();
	}

	private void UpdateUI()
	{
		uiController.SetMoneyValue(moneyHelper.Money);
		uiController.SetPopulationValue(populationHelper.Population);
	}

	public int HowManyStructuresCanIPlace(int placementCost, int numberOfStructures)
	{
		int amount = (int)moneyHelper.Money / placementCost;
		return amount > numberOfStructures ? numberOfStructures : amount;
	}

	public void AddToPopulation(int value)
	{
		populationHelper.AddToPopulation(value);
		UpdateUI();
	}

	public void ReducePopulation(int value)
	{
		populationHelper.ReducePopulation(value);
		UpdateUI();
	}
}
