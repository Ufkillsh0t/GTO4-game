using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public Player currentPlayer;

    public Player[] players;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPlayers(Player player)
    {
        Player[] next = new Player[(players.Length + 1)];
        for (int i = 0; i < players.Length; i++)
        {
            next[i] = players[i];
            Debug.Log(next[i].playerName);
        }
        next[players.Length] = player;
        players = next;
        currentPlayer = players[0];
        Debug.Log("Added player");
    }

    public void SwitchPlayers() //Arrayoutofbound exception.... need to fix
    {
        if(currentPlayer == null || currentPlayer == players[(players.Length - 1)])
        {
            currentPlayer.addResources(10, 5, 5);
            currentPlayer = players[0];
        }
        else
        {
            currentPlayer.addResources(10, 5, 5);
            for (int i = 0; i < players.Length; i++)
            {
                if(currentPlayer == players[i])
                {
                    currentPlayer = players[(i + 1)];
                    return;
                }
            }
        }
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
