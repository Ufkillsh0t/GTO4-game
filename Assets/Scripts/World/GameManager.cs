using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Tile selectedTile;
    public Tile hoverTile;
    public GridGenerator gen;

    //Buildings
    public GameObject[] buildings; //Misschien buildings van maken met GetComponent in de load?
    public Building[] buildingClasses;
    public GameObject[] units;
    public Unit[] unitClasses;

    //UI
    public Canvas canvas;
    public GameObject informationPanel;
    public GameObject messagePanel;
    public GameObject messageTextObject;
    private Text messageText;
    public GameObject textPreFab;
    public float textStartPosition = 20f;
    private float textPosition;
    private GameObject infoPanel;
    private GameObject[] panelText;

    //ENDUI
    public GameObject endPanel;
    public GameObject shownEndPanel;
    public Text endPanelText;

    [Range(1, 4)]
    public int amountOfPlayers;

    public bool currentlyPerformingAction { get; set; }

    //Gold, Lumber, Mana
    public int[] startResources;
    public static int amountOfResources = System.Enum.GetNames(typeof(ResourceType)).Length;

    public Camera currentCamera;
    public CameraMovement moveCamera;
    public AudioSource audio;

    private PlayerController playerController;
    public PlayerController GetPlayerController { get { return playerController; } }

    void Awake()
    {
        instance = this;
        CheckResourceArrayLength();

        informationPanel = (GameObject)Resources.Load("Prefabs/UI/UIPanel");
        endPanel = (GameObject)Resources.Load("Prefabs/UI/EndGamePanel");
        textPreFab = (GameObject)Resources.Load("Prefabs/UI/UIText");
        buildings = System.Array.ConvertAll(Resources.LoadAll("Prefabs/Models/Buildings"), item => (GameObject)item); //Laat alle gameObjecten van de folder Building in. Alle buildings moeten een building script hebben.
        units = System.Array.ConvertAll(Resources.LoadAll("Prefabs/Models/Units"), item => (GameObject)item); //zelfde als hierboven.

        GetBuildUnitClasses();

        //Debug.Log(buildings[0].GetComponent<Building>().ID);

        gen = GridGenerator.GetGridGenerator();

        infoPanel = (GameObject)Instantiate(informationPanel);
        infoPanel.transform.SetParent(canvas.transform, false);

        textPosition = textStartPosition;

        panelText = new GameObject[amountOfResources + 1];

        if (messagePanel != null)
        {
            if (messageTextObject != null)
            {
                messageText = messageTextObject.GetComponent<Text>();
                messageText.text = "Welcome to the game";
            }
            else
            {
                messageText = messagePanel.GetComponent<Text>();
            }
        }
        else
        {
            Debug.LogWarning("Couldn't find the message panel");
        }

        panelText[0] = (GameObject)Instantiate(textPreFab);
        panelText[0].transform.SetParent(infoPanel.transform, false);
        panelText[0].transform.localPosition = new Vector3(panelText[0].transform.localPosition.x, textPosition, panelText[0].transform.localPosition.z);

        Text playerName = panelText[0].GetComponent<Text>();
        playerName.text = "Name: ";

        for (int i = 1; i < (amountOfResources + 1); i++)
        {
            textPosition -= 20;
            panelText[i] = (GameObject)Instantiate(textPreFab);
            panelText[i].transform.SetParent(infoPanel.transform, false);
            panelText[i].transform.localPosition = new Vector3(panelText[i].transform.localPosition.x, textPosition, panelText[i].transform.localPosition.z);

            Text t = panelText[i].GetComponent<Text>();

            ResourceType r = (ResourceType)i - 1;
            t.text = r.ToString() + ": 0";
        }
    }

    private void GetBuildUnitClasses()
    {
        buildingClasses = new Building[buildings.Length];
        unitClasses = new Unit[units.Length];

        for (int i = 0; i < buildingClasses.Length; i++)
        {
            buildingClasses[i] = buildings[i].GetComponent<Building>();
        }

        for (int i = 0; i < unitClasses.Length; i++)
        {
            unitClasses[i] = units[i].GetComponent<Unit>();
        }
    }

    // Use this for initialization
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

        currentCamera = FindObjectOfType<Camera>();
        moveCamera = currentCamera.GetComponent<CameraMovement>();

        for (int i = 0; i < amountOfPlayers; i++)
        {
            Player p = gameObject.AddComponent<Player>();
            string playerNameString = "Player" + (i + 1).ToString();
            p.AddPlayerName(playerNameString);
            for (int j = 0; j < amountOfResources; j++)
            {
                p.AddResources((ResourceType)j, startResources[j]);
            }
            playerController.AddPlayers(p);
        }

        playerController.SetPlayerSpawnPoints();
        playerController.ShowSpawnableTiles();
        playerController.currentPlayer = playerController.players[0];
    }

    public void GameOverMenu(Player p)
    {
        string gameOverMessage = p.playerName + " has won!";
        endPanelText.text = gameOverMessage;
        SetMessageText(gameOverMessage);
        shownEndPanel = (GameObject)Instantiate(endPanel);
        shownEndPanel.SetActive(true);
        shownEndPanel.transform.SetParent(canvas.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        RayCastMouse();
        UiUpdate();
        if (Input.GetKeyDown(KeyCode.M))
        {
            MuteAudio();
        }
    }

    private void MuteAudio()
    {
        audio.mute = !audio.mute;
    }

    /// <summary>
    /// Retourneert de huidige instantie van de GameManager
    /// </summary>
    /// <returns>De huidige GameManager</returns>
    public static GameManager GetGameManager()
    {
        return instance;
    }

    /// <summary>
    /// Voert een RayCast uit naar de mouse positie van de huidige camera.
    /// </summary>
    public void RayCastMouse() //Deze methode werkt morgen aan het selecteren en hoveren zelf werken en unit movement!
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.DrawLine(ray.origin, hit.point);
                RayCastMouseHit(hit);
            }
        }
    }

    /// <summary>
    /// Kijkt wat voor object de raycast heeft geraakt.
    /// </summary>
    /// <param name="hit">Het object dat de raycast heeft geraakt.</param>
    public void RayCastMouseHit(RaycastHit hit)
    {
        if (!currentlyPerformingAction)
        {
            if (hit.collider.gameObject.tag == "TileObject")
            {
                IBuildUnit ibu = hit.collider.gameObject.GetComponent<IBuildUnit>();
                BuildUnitSelection(ibu);
            }
            else if (hit.collider.gameObject.tag == "Tile")
            {
                Tile t = hit.collider.gameObject.GetComponent<Tile>();
                TileSelection(t);
            }
        }
    }

    /// <summary>
    /// Kijkt of het meegegeven object een geselecteerde of hoverende kleur nodig heeft.
    /// </summary>
    /// <param name="t"></param>
    public void TileSelection(Tile t)
    {
        if (Input.GetMouseButtonDown(0) && !t.selected && !t.highlighted) //Later nog optie voor het klikken met de rechter muisknop.
        {
            SelectTile(t);
        }
        else if (Input.GetMouseButtonDown(0) && !t.selected && t.highlighted && t.hover)
        {
            if (selectedTile.buildUnit != null && t.buildUnit != null)
            {
                if (t.buildUnit.Player.ID != playerController.currentPlayer.ID)
                {
                    selectedTile.buildUnit.Attack(t);
                }
                else
                {
                    SetMessageText("Cannot attack your own player!");
                }
            }
            if (selectedTile.buildUnit != null && selectedTile.buildUnit.CanMove() && t.buildUnit == null)
            {
                selectedTile.buildUnit.Move(t, true);
            }
        }
        else if (Input.GetMouseButtonDown(1) && !t.selected && t.highlighted)
        {
            SelectTile(t);
        }

        if (hoverTile == null)
        {
            Debug.Log("hover");
            //hoverTile = t;
            if (!t.selected)
            {
                t.MouseHover();
            }
        }
        if (hoverTile != null && hoverTile.ID != t.ID)
        {
            t.MouseHover();
        }
    }

    /// <summary>
    /// Selecteerd een gegeven tile.
    /// </summary>
    /// <param name="t">De tile die geselecteerd moet worden.</param>
    public void SelectTile(Tile t)
    {
        if (selectedTile != null)
        {
            if (t.ID != selectedTile.ID)
            {
                selectedTile.MouseExit();
                t.MouseClick();
            }
            else
            {
                selectedTile.SelectTile();
                Debug.Log("Het huidige tile is geselecteerd in de gamemanager maar niet in zijn script zelf!");
            }
        }
        else
        {
            t.MouseClick();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ibu"></param>
    public void BuildUnitSelection(IBuildUnit ibu)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!ibu.TileHighlighted() && !ibu.getTile().selected)
            {
                ibu.Select();
            }
            else
            {
                if (ibu.getTile().buildUnit.Player.ID != playerController.currentPlayer.ID)
                {
                    selectedTile.buildUnit.Attack(ibu.getTile());
                }
                else
                {
                    ibu.Select();
                }
                //ibu.Defend();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (ibu.TileHighlighted() && !ibu.getTile().selected)
            {
                ibu.Select();
            }
        }
        else
        {
            ibu.Hover();
        }
    }

    /// <summary>
    /// Kijkt of de resource array genoeg resources heeft.
    /// </summary>
    public void CheckResourceArrayLength()
    {
        if (startResources.Length < amountOfResources)
        {
            Debug.LogError("Array length needs to be:" + amountOfResources);
            startResources = new int[amountOfResources];
        }
        else if (startResources.Length > amountOfResources)
        {
            Debug.LogError("The array is a bit too long so not all values will be used!");
            startResources = new int[amountOfResources];
        }
    }

    /// <summary>
    /// Update de UI
    /// </summary>
    public void UiUpdate()
    {
        Text playerName = panelText[0].GetComponent<Text>();
        playerName.text = "Name: " + playerController.currentPlayer.playerName;

        for (int i = 1; i < (amountOfResources + 1); i++)
        {
            Text t = panelText[i].GetComponent<Text>();
            ResourceType r = (ResourceType)i - 1;
            t.text = r.ToString() + ": " + playerController.currentPlayer.resources[(int)r];
        }
    }

    /// <summary>
    /// Voegt een gebouw toe aan het speelveld en de huidige speler.
    /// </summary>
    /// <param name="buildingType">Welk gebouw je wilt spawnen</param>
    public void AddBuilding(int buildingType)
    {
        //Controleer voor genoeg resources.
        Building building = buildings[buildingType].GetComponent<Building>();

        if (building != null && playerController.currentPlayer.EnoughResources(building.buildingCost) && selectedTile.buildUnit == null && selectedTile.PlayerID == playerController.currentPlayer.ID)
        {
            Building spawnedBuilding = selectedTile.SpawnObject(building.gameObject).GetComponent<Building>(); //test
            if (spawnedBuilding != null)
            {
                spawnedBuilding.OnSpawn(selectedTile, playerController.currentPlayer);
                playerController.currentPlayer.AddBuilding(spawnedBuilding);
                selectedTile.MouseClick();
                playerController.Turn();
            }
            else
            {
                SetMessageText("There is already a object on that tile");
            }
        }
        else
        {
            SetMessageText("Couldn't spawn building due too a low amount of resources or the building couldn't be initialized! Resources required, Gold: " 
                + building.buildingCost[0].ToString() + " Lumber: " + building.buildingCost[1].ToString() + " Mana: "  + building.buildingCost[2].ToString());
        }
    }

    /// <summary>
    /// Voegt een Unit toe aan het speelveld en de huidige speler.
    /// </summary>
    /// <param name="unitType">Welk type Unit je wilt spawnen</param>
    public void AddUnit(int unitType)
    {
        Unit unit = units[unitType].GetComponent<Unit>();
        if (units != null && playerController.currentPlayer.EnoughResources(unit.unitCost) && selectedTile.buildUnit == null && selectedTile.PlayerID == playerController.currentPlayer.ID)
        {
            Unit spawnedUnit = selectedTile.SpawnObject(unit.gameObject).GetComponent<Unit>();
            if (spawnedUnit != null)
            {
                spawnedUnit.OnSpawn(selectedTile, playerController.currentPlayer);
                playerController.currentPlayer.AddUnit(spawnedUnit);
                selectedTile.MouseClick();
                playerController.Turn();
            }
            else
            {
                SetMessageText("There is already a object on that tile");
            }
        }
        else
        {
            SetMessageText("Couldn't spawn building due too a low amount of resources or the building couldn't be initialized! Resources required, Gold: "
                + unit.unitCost[0].ToString() + " Lumber: " + unit.unitCost[1].ToString() + " Mana: " + unit.unitCost[2].ToString());
        }
    }

    public void SetMessageText(string text)
    {
        Debug.Log(text);
        if (messageText != null) messageText.text = text;
    }
}
