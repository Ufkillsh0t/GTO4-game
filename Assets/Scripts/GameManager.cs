using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public GameObject selectedTile;

    public Canvas canvas;
    public GameObject informationPanel;

    [Range(1, 4)]
    public int amountOfPlayers;
    private int amountOfResources;

    //Gold, Lumber, Mana
    public int[] startResources;

    public Camera currentCamera;
    public Text playerNameText;
    public Text playerManaText;
    public Text playerGoldText;
    public Text playerLumberText;

    private PlayerController playerController;

    void Awake()
    {
        instance = this;
        CheckResourceArrayLength();
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
            p.addPlayerName(playerNameString);
            for (int j = 0; j < amountOfResources; j++)
            {
                p.addResources((ResourceType)j, startResources[j]);
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
        amountOfResources = System.Enum.GetNames(typeof(ResourceType)).Length;
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
        playerNameText.text = "Name: " + playerController.currentPlayer.playerName;
        playerManaText.text = "Mana: " + playerController.currentPlayer.resources[(int)ResourceType.Mana].ToString();
        playerGoldText.text = "Gold: " + playerController.currentPlayer.resources[(int)ResourceType.Gold].ToString();
        playerLumberText.text = "Lumber: " + playerController.currentPlayer.resources[(int)ResourceType.Lumber].ToString();
    }
}
