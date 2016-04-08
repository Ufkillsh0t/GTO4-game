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
    public int ID;

    public int GetPlayerID { get { return ID; } }

    void Awake()
    {
        id += 1;
        ID = id;
        resources = new int[GameManager.amountOfResources];
    }

    /// <summary>
    /// Voegt een gebouw toe aan deze speler.
    /// </summary>
    /// <param name="b">Het gebouw dat toegevoegd moet worden</param>
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
    /// <param name="b">Het gebouw dat verwijderd moet worden.</param>
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

    /// <summary>
    /// Voegt een unit toe aan deze speler.
    /// </summary>
    /// <param name="u">Unit die je aan deze speler wilt toevoegen!</param>
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
            newUnits[units.Length] = u;
            units = newUnits;
        }
    }

    /// <summary>
    /// Verwijdert een gebouw uit de array met gebouwen.
    /// </summary>
    /// <param name="b">Unit die je van deze speler wilt verwijderen.</param>
    public void RemoveUnit(Unit u)
    {
        Unit[] newUnits = new Unit[units.Length - 1];
        int j = 0; //Voor het toevoegen van de juiste buildings;
        for (int i = 0; i < units.Length; i++)
        {
            if (u.ID != units[i].ID)
            {
                newUnits[j] = newUnits[i];
                j++;
            }
        }
        units = newUnits;
    }

    /// <summary>
    /// Kijkt of de speler genoeg resources heeft om iets te kopen.
    /// </summary>
    /// <param name="cost">De hoeveelheid die een gebouw of unit kost.</param>
    /// <returns>Of de speler genoeg resources heeft voor de gegeven kosten</returns>
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

    /// <summary>
    /// Voegt een playernaam toe aan deze speler.
    /// </summary>
    /// <param name="playerName"></param>
    public void AddPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    /// <summary>
    /// Voegt resources aan deze speler toe.
    /// </summary>
    /// <param name="resource">Welk type resource toegevoegd moet worden.</param>
    /// <param name="amount">Hoeveel er moet worden toegevoegd van de gegeven resource</param>
    public void AddResources(ResourceType resource, int amount)
    {
        resources[(int)resource] += amount;
    }

    /// <summary>
    /// Verwijdert resources van deze speler.
    /// </summary>
    /// <param name="resource">Van welke type resource er wat verwijder moet worden.</param>
    /// <param name="amount">Hoeveel er van het gegeven type resource verwijderd moet worden.</param>
    public void RemoveResources(ResourceType resource, int amount)
    {
        resources[(int)resource] -= amount;
    }
}
