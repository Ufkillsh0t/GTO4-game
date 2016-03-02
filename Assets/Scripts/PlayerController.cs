using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Player currentPlayer;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SwitchPlayers()
    {
        Debug.Log("Switch Players initiated");
    }

    public void setCurrentPlayerCamera(Camera camera)
    {
        currentPlayer.playerCamera = camera;
    }

    public Camera getCurrentPlayerCamera(Camera camera)
    {
        return currentPlayer.playerCamera;
    }
}
