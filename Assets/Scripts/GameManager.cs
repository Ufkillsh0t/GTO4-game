using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public GameManager instance;

    public int amountOfPlayers;
    public int startMana;
    public int startGold;
    public int startLumber;

    public Camera currentCamera;
    public Text playerNameText; 
    public Text playerManaText; 
    public Text playerGoldText; 
    public Text playerLumberText;

    private PlayerController playerController;

    void Awake()
    {
        instance = this;
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
            p.addResources(startMana, startGold, startLumber);
            playerController.AddPlayers(p);
        }
    }

    // Update is called once per frame
    void Update()
    {
        uiUpdate();
    }

    public void uiUpdate()
    {
        playerNameText.text = "Name: " + playerController.currentPlayer.playerName;
        playerManaText.text = "Mana: " + playerController.currentPlayer.mana.ToString();
        playerGoldText.text = "Gold: " + playerController.currentPlayer.gold.ToString();
        playerLumberText.text = "Lumber: " + playerController.currentPlayer.lumber.ToString();
    }
}
