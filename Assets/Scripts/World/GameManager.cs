using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Tile selectedTile;
    public Tile hoverTile;

    //Buildings
    public GameObject[] buildings; //Misschien buildings van maken met GetComponent in de load?
    public GameObject[] units;

    //UI
    public Canvas canvas;
    public GameObject informationPanel;
    public GameObject textPreFab;
    public float textStartPosition = 20f;
    private float textPosition;
    private GameObject infoPanel;
    private GameObject[] panelText;


    [Range(1, 4)]
    public int amountOfPlayers;

    //Gold, Lumber, Mana
    public int[] startResources;
    public static int amountOfResources = System.Enum.GetNames(typeof(ResourceType)).Length;

    public Camera currentCamera;

    private PlayerController playerController;
    public PlayerController GetPlayerController { get { return playerController; } }

    void Awake()
    {
        instance = this;
        CheckResourceArrayLength();

        informationPanel = (GameObject)Resources.Load("Prefabs/UI/UIPanel");
        textPreFab = (GameObject)Resources.Load("Prefabs/UI/UIText");
        buildings = System.Array.ConvertAll(Resources.LoadAll("Prefabs/Models/Buildings"), item => (GameObject)item); //Laat alle gameObjecten van de folder Building in. Alle buildings moeten een building script hebben.
        units = System.Array.ConvertAll(Resources.LoadAll("Prefabs/Models/Units"), item => (GameObject)item); //zelfde als hierboven.
        //Debug.Log(buildings[0].GetComponent<Building>().ID);

        infoPanel = (GameObject)Instantiate(informationPanel);
        infoPanel.transform.SetParent(canvas.transform, false);

        textPosition = textStartPosition;

        panelText = new GameObject[amountOfResources + 1];

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

    // Use this for initialization
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

        currentCamera = FindObjectOfType<Camera>();

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
    }

    // Update is called once per frame
    void Update()
    {
        RayCastMouse();
        UiUpdate();
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
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            RayCastMouseHit(hit);
        }
    }

    /// <summary>
    /// Kijkt wat voor object de raycast heeft geraakt.
    /// </summary>
    /// <param name="hit">Het object dat de raycast heeft geraakt.</param>
    public void RayCastMouseHit(RaycastHit hit)
    {
        if (hit.collider.gameObject.tag == "Tile")
        {
            Tile t = hit.collider.gameObject.GetComponent<Tile>();
            TileSelection(t);
        }
        if (hit.collider.gameObject.tag == "TileObject")
        {
            IBuildUnit ibu = hit.collider.gameObject.GetComponent<IBuildUnit>();
            BuildUnitSelection(ibu);
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
        else if (Input.GetMouseButtonDown(1) && !t.selected && t.highlighted)
        {
            SelectTile(t);
        }
        if (hoverTile == null)
        {
            hoverTile = t;
            if (!hoverTile.selected)
            {
                t.MouseHover();
            }
        }
        if (hoverTile != null && hoverTile.ID != t.ID)
        {
            hoverTile.MouseExit();
            hoverTile = t;
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
                Debug.Log("Het huidige tile is geselecteerd in de gamemanger maar niet in zijn script zelf!");
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
            ibu.Select();
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

        if (building != null && playerController.currentPlayer.EnoughResources(building.buildingCost))
        {
            Building spawnedBuilding = selectedTile.SpawnObject(building.gameObject).GetComponent<Building>(); //test
            if (spawnedBuilding != null)
            {
                spawnedBuilding.OnSpawn(selectedTile, playerController.currentPlayer);
                playerController.currentPlayer.AddBuilding(spawnedBuilding);
                selectedTile.HighLightNearbyTiles(selectedTile.buildUnit.GetRange(), selectedTile.buildUnit.GetRangeType(), true);
            }
            else
            {
                Debug.Log("There is already a object on that tile");
            }
        }
        else
        {
            Debug.Log("Couldn't spawn building due too a low amount of resources or the building couldn't be initialized!");
        }
    }

    /// <summary>
    /// Voegt een Unit toe aan het speelveld en de huidige speler.
    /// </summary>
    /// <param name="unitType">Welk type Unit je wilt spawnen</param>
    public void AddUnit(int unitType)
    {
        Unit unit = units[unitType].GetComponent<Unit>();
        if (units != null && playerController.currentPlayer.EnoughResources(unit.unitCost))
        {
            Unit spawnedUnit = selectedTile.SpawnObject(unit.gameObject).GetComponent<Unit>();
            if (spawnedUnit != null)
            {
                spawnedUnit.OnSpawn(selectedTile, playerController.currentPlayer);
                playerController.currentPlayer.AddUnit(spawnedUnit);
                selectedTile.HighLightNearbyTiles(selectedTile.buildUnit.GetRange(), selectedTile.buildUnit.GetRangeType(), true);
            }
            else
            {
                Debug.Log("There is already a object on that tile");
            }
        }
    }
}
