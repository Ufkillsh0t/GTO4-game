using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour, IResources {

    public int[] resources;
    public string playerName;
    public Vector3 cameraPosition;
    public Building[] buildings;

    void Awake()
    {
        resources = new int[System.Enum.GetNames(typeof(ResourceType)).Length];
    }

    public void AddBuilding(Building b)
    {
        if(buildings == null)
        {
            buildings = new Building[1];
            buildings[0] = b;
        }
        else
        {
            Building[] newBuildings = new Building[buildings.Length + 1];
            for(int i = 0; i < buildings.Length; i++)
            {
                newBuildings[i] = buildings[i];
            }
            newBuildings[buildings.Length + 1] = b;
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
        for(int i = 0; i < buildings.Length; i++)
        {
            if(b.ID != buildings[i].ID)
            {
                newBuildings[j] = buildings[i];
                j++;
            }
        }
        buildings = newBuildings;
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
