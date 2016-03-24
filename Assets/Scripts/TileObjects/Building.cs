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
    public GameObject building;
    public Tile currentTile;

    private static int uniqueID;
    public int ID;

    void Awake()
    {
        uniqueID = uniqueID + 1;
        ID = uniqueID;
        CheckBuildingCostArrayLength();
        building = this.gameObject;
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

    public void OnSpawn(Tile t, Player p)
    {
        currentTile = t;
        player = p;
        //TODO: verwijderen resources van player.
        p.RemoveResources(ResourceType.Mana, 10); //Test
        Debug.Log("Tile ID:" + t.ID + " player:" + p.playerName);
    }

    public bool Attack()
    {
        throw new NotImplementedException();
    }

    public bool Defend()
    {
        throw new NotImplementedException();
    }

    public bool Ugrade()
    {
        throw new NotImplementedException();
    }
}
