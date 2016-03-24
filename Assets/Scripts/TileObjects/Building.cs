using UnityEngine;
using System.Collections;
using System;

public class Building : MonoBehaviour, IBuildUnit {

    public BuildingType type;
    public int health;
    public int damage;
    public int armor;
    public int[] buildingCost;
    private int amountOfResources;
    public Player player;

    void Awake()
    {
        CheckBuildingCostArrayLength();
        player = GameManager.GetGameManager().GetPlayerController.currentPlayer;
    }

    public void CheckBuildingCostArrayLength()
    {
        amountOfResources = System.Enum.GetNames(typeof(ResourceType)).Length;
        if (buildingCost.Length < amountOfResources)
        {
            Debug.LogError("Array length needs to be:" + amountOfResources);
            buildingCost = new int[amountOfResources];
        }
        else if (buildingCost.Length > amountOfResources)
        {
            Debug.LogError("The array is a bit too long so not all values will be used!");
            buildingCost = new int[amountOfResources];
        }
    }

    public bool Attack()
    {
        throw new NotImplementedException();
    }

    public bool Defend()
    {
        throw new NotImplementedException();
    }
}
