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

    private Renderer render;
    private GameManager gm;

    public Player player;
    public GameObject building;
    public Tile currentTile;

    public Color selectedColor = Color.cyan;
    public Color hoverColor = Color.magenta;
    public Color blockedColor = Color.grey;
    public Color attackColor = Color.red;

    private Color defaultMaterialColor;
    public Color DefaultMaterialColor { get { return defaultMaterialColor; } }

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
        render = gameObject.GetComponent<Renderer>(); //Verkrijgt de renderer van dit object.
        defaultMaterialColor = render.material.color;
        gm = GameManager.GetGameManager();
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
        if (currentTile.ID == gm.selectedTile.ID)
        {
            if (gm.GetPlayerController.currentPlayer.ID != currentTile.buildUnit.Player.ID)
            {
                ColorObject(BuildUnitColor.Blocked);
            }
            else
            {
                ColorObject(BuildUnitColor.Selected);
            }
        }
        else
        {
            currentTile.MouseClick();
        }
    }

    public void Hover()
    {
        if (!currentTile.hover)
        {
            currentTile.MouseHover();
        }
        if (currentTile.hover && !currentTile.selected)
        {
            if (gm.GetPlayerController.currentPlayer.ID != currentTile.buildUnit.Player.ID)
            {
                ColorObject(BuildUnitColor.Blocked);
            }
            else
            {
                ColorObject(BuildUnitColor.Hover);
            }
        }
    }

    public void Exit()
    {
        ColorObject(BuildUnitColor.Default);
        //currentTile.MouseExit();
    }

    public void Blocked()
    {
        ColorObject(BuildUnitColor.Blocked);
    }

    public int GetRange()
    {
        return range;
    }

    public RangeType GetRangeType()
    {
        return rangeType;
    }

    public void ColorObject(BuildUnitColor col)
    {
        switch (col)
        {
            case BuildUnitColor.Hover:
                render.material.color = hoverColor;
                break;
            case BuildUnitColor.Selected:
                render.material.color = selectedColor;
                break;
            case BuildUnitColor.Blocked:
                render.material.color = blockedColor;
                break;
            case BuildUnitColor.Attack:
                render.material.color = attackColor;
                break;
            default:
            case BuildUnitColor.Default:
                render.material.color = defaultMaterialColor;
                break;
        }
    }

}
