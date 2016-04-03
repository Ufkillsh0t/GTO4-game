using UnityEngine;
using System.Collections;
using System;

public class Building : MonoBehaviour, IBuildUnit
{

    public BuildingType buildingType;
    public RangeType rangeType;
    public int range;
    public int health;
    public int damage;
    public int armor;
    public int[] buildingCost;
    private int amountOfResources;
    public Player player;
    public GameObject building;
    public Tile currentTile;

    private static int uniqueID;
    public int ID;

    public Player Player
    {
        get
        {
            return player;
        }
    }

    void Awake()
    {
        uniqueID = uniqueID + 1;
        ID = uniqueID;
        CheckBuildingCostArrayLength();
        building = this.gameObject;
    }

    public void CheckBuildingCostArrayLength()
    {
        amountOfResources = GameManager.amountOfResources;
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

    public void OnSpawn(Tile t, Player p)
    {
        currentTile = t;
        player = p;
        for (int i = 0; i < buildingCost.Length; i++)
        {
            if (p.resources[i] > buildingCost[i])
            {
                p.resources[i] -= buildingCost[i];
            }
            else
            {
                Debug.LogError("A object has been spawned that has a higher cost than the players resources!");
            }
        }
        Debug.Log("Tile ID:" + t.ID + " player:" + p.playerName);
    }

    public bool Attack()
    {
        throw new NotImplementedException();
    }

    public bool Defend(int damage)
    {
        throw new NotImplementedException();
    }

    public bool Upgrade()
    {
        throw new NotImplementedException();
    }

    public void Select()
    {
        throw new NotImplementedException();
    }

    public void Hover()
    {
        throw new NotImplementedException();
    }

    public void Exit()
    {
        throw new NotImplementedException();
    }

    public int GetRange()
    {
        return range;
    }

    public RangeType GetRangeType()
    {
        return rangeType;
    }
}
