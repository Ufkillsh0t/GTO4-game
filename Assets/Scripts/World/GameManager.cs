using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Tile selectedTile;

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
        UiUpdate();
    }

    public static GameManager GetGameManager()
    {
        return instance;
    }

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

    public void AddBuilding(int buildingType)
    {
        //Controleer voor genoeg resources.
        Building building = buildings[buildingType].GetComponent<Building>();

        if (building != null && playerController.currentPlayer.EnoughResources(building.buildingCost))
        {
            if (selectedTile.SpawnObject(building.gameObject))
            {
                building.OnSpawn(selectedTile, playerController.currentPlayer);
                playerController.currentPlayer.AddBuilding(building);
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

    public void AddUnit(int unitType)
    {
        Unit unit = units[unitType].GetComponent<Unit>();
        if(units != null && playerController.currentPlayer.EnoughResources(unit.unitCost))
        {
            if (selectedTile.SpawnObject(unit.gameObject)) {
                unit.OnSpawn(selectedTile, playerController.currentPlayer);
                playerController.currentPlayer.AddUnit(unit);
            }
            else
            {
                Debug.Log("There is already a object on that tile");
            }
        }
    }
}
