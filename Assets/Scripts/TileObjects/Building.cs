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
    public bool moveAble;
    public bool canAttack;
    public int[] buildingCost;
    public int[] resourcesIncrease;
    private int amountOfResources;
    public float movementSpeed;

    private Renderer render;
    private GameManager gm;

    public Player player;
    public GameObject building;
    public Tile currentTile;
    private Vector3 currentPosition;

    public Color selectedColor = Color.cyan;
    public Color hoverColor = Color.magenta;
    public Color blockedColor = Color.grey;
    public Color attackColor = Color.red;

    private Color defaultMaterialColor;
    public Color DefaultMaterialColor { get { return defaultMaterialColor; } }

    private static int uniqueID;
    public int ID;

    public bool moving;

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
        CheckResourceIncreaseArrayLength();
        building = this.gameObject;
        moving = false;
    }

    void Start()
    {
        currentPosition = transform.position;
    }

    void Update()
    {
        if (gameObject.transform.position != currentPosition && currentPosition != null)
        {
            transform.LookAt(currentPosition);
            float distance = Vector3.Distance(transform.position, currentPosition);
            if (distance <= 0.05f)
            {
                transform.position = currentPosition;
                moving = false;
            }
            else
            {
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
        }
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

    public void CheckResourceIncreaseArrayLength()
    {
        amountOfResources = GameManager.amountOfResources;
        if (resourcesIncrease.Length < amountOfResources)
        {
            Debug.LogError("Array length needs to be:" + amountOfResources);
            resourcesIncrease = new int[amountOfResources];
        }
        else if (resourcesIncrease.Length > amountOfResources)
        {
            Debug.LogError("The array is a bit too long so not all values will be used!");
            resourcesIncrease = new int[amountOfResources];
        }
    }

    public void OnSpawn(Tile t, Player p)
    {
        currentTile = t;
        player = p;
        for (int i = 0; i < buildingCost.Length; i++)
        {
            if (p.resources[i] >= buildingCost[i])
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
        if (currentTile.hover && !currentTile.selected && !currentTile.highlighted)
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
        if (gm.GetPlayerController.currentPlayer.ID == player.ID)
        {
            ColorObject(BuildUnitColor.Default);
        }
        else
        {
            ColorObject(BuildUnitColor.Blocked);
        }
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

    public bool TileHighlighted()
    {
        if (currentTile != null)
        {
            return currentTile.highlighted;
        }
        else
        {
            return false;
        }
    }

    public Tile getTile()
    {
        return currentTile;
    }

    public bool Move(Tile t)
    {
        //snelle movement test;
        if (t.buildUnit == null && moveAble)
        {
            if (currentTile.selected)
            {
                currentTile.MouseExit();
                currentTile.ResetTile();
                t.buildUnit = this;
                currentTile.buildUnit = null;
                currentTile = t;
                currentTile.MouseClick();
                moving = true;
                gm.GetPlayerController.Turn();

                //currentTile.gameObject = this;
                float y = currentTile.transform.position.y;
                currentPosition = new Vector3(currentTile.transform.position.x, y + (render.bounds.size.y / 2), currentTile.transform.position.z); //rotatie nog goed doen voor movement.
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool CanMove()
    {
        return moveAble;
    }

    public bool IsMoving()
    {
        return moving;
    }

    public bool Attack(Tile t)
    {
        if (t.buildUnit != null && canAttack)
        {
            if (!t.buildUnit.Defend(damage))
            {
                t.buildUnit = null;
                gm.GetPlayerController.Turn();
            }
            else
            {
                gm.GetPlayerController.Turn();
            }
            return true;
        }
        else
        {
            gm.GetPlayerController.Turn();
            return false;
        }
    }

    public bool CanAttack()
    {
        return canAttack;
    }

    public bool Defend(int damage)
    {
        if (damage >= health)
        {
            player.RemoveBuilding(this);
            Destroy(gameObject);
            return false;
        }
        else
        {
            if (armor >= damage)
            {
                armor -= damage;
                return true;
            }
            else if(armor > 0)
            {
                int remainingDamage = damage - armor;
                armor = 0;
                health -= remainingDamage;
                if(health <= 0)
                {
                    player.RemoveBuilding(this);
                    Destroy(gameObject);
                    return false;
                }
                return true;
            }
            else
            {
                health -= damage;
                return true;
            }
        }
    }

    public bool Upgrade()
    {
        throw new NotImplementedException();
    }

}
