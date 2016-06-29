using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

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

    public GameObject rotationalObject;

    public Color selectedColor = Color.cyan;
    public Color hoverColor = Color.magenta;
    public Color blockedColor = Color.grey;
    public Color attackColor = Color.red;

    //Stats Canvas
    public GameObject unitStatsCanvas;
    public GameObject unitStatsText;
    public int resourcePosition = 60;
    private GameObject[] unitStats;
    private Text healthText;
    private Text armorText;
    private Text playerText;
    private Text GoldIncrease;
    private Text LumberIncrease;
    private Text ManaIncrease;

    private Color defaultMaterialColor;
    public Color DefaultMaterialColor { get { return defaultMaterialColor; } }

    private static int uniqueID;
    public int ID;

    public bool moving;
    public bool attacking;

    public Player Player
    {
        get
        {
            return player;
        }
    }

    void Awake()
    {
        render = gameObject.GetComponentInChildren<Renderer>(); //Verkrijgt de renderer van dit object.
        defaultMaterialColor = render.material.color;
        gm = GameManager.GetGameManager();
        uniqueID = uniqueID + 1;
        ID = uniqueID;
        CheckBuildingCostArrayLength();
        CheckResourceIncreaseArrayLength();
        building = this.gameObject;
        moving = false;
        InstantiateStatsCanvas();
    }

    private void InstantiateStatsCanvas()
    {
        if (unitStatsCanvas == null)
        {
            Debug.LogError("No canvas");
        }
        else
        {
            unitStatsCanvas = (GameObject)Instantiate(unitStatsCanvas);
            unitStatsCanvas.transform.SetParent(transform, false);
        }
        if (unitStatsText != null)
        {
            unitStats = new GameObject[6];

            unitStats[0] = (GameObject)Instantiate(unitStatsText);
            unitStats[0].transform.SetParent(unitStatsCanvas.transform, false);
            unitStats[0].transform.localPosition = new Vector3(unitStats[0].transform.localPosition.x, 25f, unitStats[0].transform.localPosition.z);
            healthText = unitStats[0].GetComponent<Text>();

            unitStats[1] = (GameObject)Instantiate(unitStatsText);
            unitStats[1].transform.SetParent(unitStatsCanvas.transform, false);
            unitStats[1].transform.localPosition = new Vector3(unitStats[1].transform.localPosition.x, 7.5f, unitStats[1].transform.localPosition.z);
            armorText = unitStats[1].GetComponent<Text>();

            unitStats[2] = (GameObject)Instantiate(unitStatsText);
            unitStats[2].transform.SetParent(unitStatsCanvas.transform, false);
            unitStats[2].transform.localPosition = new Vector3(unitStats[2].transform.localPosition.x, -10f, unitStats[2].transform.localPosition.z);
            playerText = unitStats[2].GetComponent<Text>();
            playerText.text = "player not initialized";

            if(buildingType == BuildingType.Harvester)
            {
                unitStats[3] = (GameObject)Instantiate(unitStatsText);
                unitStats[3].transform.SetParent(unitStatsCanvas.transform, false);
                unitStats[3].transform.localPosition = new Vector3(unitStats[0].transform.localPosition.x + resourcePosition, 25f, unitStats[3].transform.localPosition.z);
                GoldIncrease = unitStats[3].GetComponent<Text>();

                unitStats[4] = (GameObject)Instantiate(unitStatsText);
                unitStats[4].transform.SetParent(unitStatsCanvas.transform, false);
                unitStats[4].transform.localPosition = new Vector3(unitStats[1].transform.localPosition.x + resourcePosition, 7.5f, unitStats[4].transform.localPosition.z);
                LumberIncrease = unitStats[4].GetComponent<Text>();

                unitStats[5] = (GameObject)Instantiate(unitStatsText);
                unitStats[5].transform.SetParent(unitStatsCanvas.transform, false);
                unitStats[5].transform.localPosition = new Vector3(unitStats[2].transform.localPosition.x + resourcePosition, -10f, unitStats[5].transform.localPosition.z);
                ManaIncrease = unitStats[5].GetComponent<Text>();
            }
        }
        ShowStatsText();
        unitStatsCanvas.SetActive(false);
    }

    public void ShowStatsText()
    {
        if (!unitStatsCanvas.activeSelf) unitStatsCanvas.SetActive(true);

        healthText.text = "Health: " + health.ToString();
        armorText.text = "Armor: " + armor.ToString();
        if (player != null && player.ID != gm.GetPlayerController.currentPlayer.ID)
        {
            playerText.text = "Player: Enemy";
        }
        else
        {
            playerText.text = "Player: You";
        }

        if(buildingType == BuildingType.Harvester)
        {
            GoldIncrease.text = "Gold: " + resourcesIncrease[0].ToString();
            LumberIncrease.text = "Lumber: " + resourcesIncrease[1].ToString();
            ManaIncrease.text = "Mana: " + resourcesIncrease[2].ToString();
        }
    }

    void Start()
    {
        currentPosition = transform.position;
    }

    void Update()
    {
        if (currentPosition != null && gameObject.transform.position != currentPosition)
        {
            transform.LookAt(currentPosition);
            float distance = Vector3.Distance(transform.position, currentPosition);
            if (distance <= 0.05f)
            {
                transform.position = currentPosition;
                moving = false;
                gm.currentlyPerformingAction = false;
                gm.GetPlayerController.Turn();
            }
            else
            {
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
        }
        if (currentTile.hover || currentTile.selected)
        {
            ShowStatsText();
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
        unitStatsCanvas.SetActive(true);
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
        unitStatsCanvas.SetActive(true);
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
        unitStatsCanvas.SetActive(false);
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
        unitStatsCanvas.SetActive(true);
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

    public void LookAt(Tile t)
    {
        Vector3 target = t.transform.position;
        target.y = transform.position.y;
        transform.LookAt(target);
    }

    public bool Move(Tile t, bool turn)
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

                if (turn)
                    LookAt(t);

                gm.currentlyPerformingAction = true;
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
            if (rotationalObject != null)
            {
                LookAt(t);
            }
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
        if (armor == 0 && damage >= health)
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
            else if (armor > 0)
            {
                int remainingDamage = damage - armor;
                armor = 0;
                health -= remainingDamage;
                if (health <= 0)
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
