using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameManager instance;

    public int amountOfPlayers;
    public int startMana;
    public int startGold;
    public int startLumber;

    private PlayerController playerController;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        playerController.SwitchPlayers();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
