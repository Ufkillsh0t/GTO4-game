using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour, IResources
{
    private static int id;
    public int[] resources;
    public string playerName;
    public Vector3 cameraPosition;
    public Building[] buildings;
    public Unit[] units;

    public int GetPlayerID { get { return id; } }

    void Awake()
    {
        id += 1;
        resources = new int[GameManager.amountOfResources];
    }

    public void AddBuilding(Building b)
    {
        if (buildings == null)
        {
            buildings = new Building[1];
            buildings[0] = b;
        }
        else
        {
            Building[] newBuildings = new Building[buildings.Length + 1];
            for (int i = 0; i < buildings.Length; i++)
            {
                newBuildings[i] = buildings[i];
            }
            newBuildings[buildings.Length] = b;
            buildings = newBuildings;
        }
    }

    /// <summary>
    /// Verwijdert een gebouw uit de array met gebouwen.
    /// </summary>
    /// <param name="b"></param>
    public void RemoveBuilding(Building b)
    {
        Building[] newBuildings = new Building[buildings.Length - 1];
        int j = 0; //Voor het toevoegen van de juiste buildings;
        for (int i = 0; i < buildings.Length; i++)
        {
            if (b.ID != buildings[i].ID)
            {
                newBuildings[j] = buildings[i];
                j++;
            }
        }
        buildings = newBuildings;
    }

    public void AddUnit(Unit u)
    {
        if (units == null)
        {
            units = new Unit[1];
            units[0] = u;
        }
        else
        {
            Unit[] newUnits = new Unit[units.Length + 1];
            for (int i = 0; i < units.Length; i++)
            {
                newUnits[i] = units[i];
            }
            newUnits[buildings.Length] = u;
            units = newUnits;
        }
    }

    /// <summary>
    /// Verwijdert een gebouw uit de array met gebouwen.
    /// </summary>
    /// <param name="b"></param>
    public void RemoveUnit(Building b)
    {
        Unit[] newUnits = new Unit[units.Length - 1];
        int j = 0; //Voor het toevoegen van de juiste buildings;
        for (int i = 0; i < units.Length; i++)
        {
            if (b.ID != units[i].ID)
            {
                newUnits[j] = newUnits[i];
                j++;
            }
        }
        units = newUnits;
    }

    public bool EnoughResources(int[] cost)
    {
        if (cost.Length != resources.Length)
        {
            Debug.LogError("There are more costValues then there are resources!");
            return false;
        }
        else
        {
            for (int i = 0; i < resources.Length; i++)
            {
                if(cost[i] > resources[i])
                {
                    Debug.Log("Cost of resource " + (ResourceType)i + " is too high!");
                    return false;
                }
            }
            return true;
        }
    }

    public void AddPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    public void AddResources(ResourceType resource, int amount)
    {
        resources[(int)resource] += amount;
    }

    public void RemoveResources(ResourceType resource, int amount)
    {
        resources[(int)resource] -= amount;
    }
}
