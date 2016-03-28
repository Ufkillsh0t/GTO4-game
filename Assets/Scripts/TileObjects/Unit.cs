﻿using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour, IBuildUnit
{

    public UnitType type;
    public int health;
    public int damage;
    public int armor;
    public int[] unitCost;
    private int amountOfResources;
    public Player player;
    public GameObject unit;
    public Tile currentTile;

    private static int uniqueID;
    public int ID;


    void Awake()
    {
        uniqueID = uniqueID + 1;
        ID = uniqueID;
        CheckUnitCostArrayLength();
        unit = this.gameObject;
    }

    public void CheckUnitCostArrayLength()
    {
        amountOfResources = GameManager.amountOfResources;
        if (unitCost.Length < amountOfResources)
        {
            Debug.LogError("Array length needs to be:" + amountOfResources);
            unitCost = new int[amountOfResources];
        }
        else if (unitCost.Length > amountOfResources)
        {
            Debug.LogError("The array is a bit too long so not all values will be used!");
            unitCost = new int[amountOfResources];
        }
    }

    public void OnSpawn(Tile t, Player p)
    {
        currentTile = t;
        player = p;
        for (int i = 0; i < unitCost.Length; i++)
        {
            if (p.resources[i] > unitCost[i])
            {
                p.resources[i] -= unitCost[i];
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

    public bool Defend()
    {
        throw new NotImplementedException();
    }

    public bool Ugrade()
    {
        throw new NotImplementedException();
    }
}
