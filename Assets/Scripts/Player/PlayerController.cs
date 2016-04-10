using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public Player currentPlayer;
    public Player[] players;

    public int[] defaultResourceIncrease;
    private int amountOfResources;
    public int amountOfTurnsPerPlayer;
    public int currentTurn;
    private int turns;


    void Awake()
    {
        turns = amountOfTurnsPerPlayer;
        CheckResourceArrayLength();
    }

    public void AddPlayers(Player player)
    {
        player.cameraPosition = GameManager.GetGameManager().currentCamera.transform.position;
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

    public void SwitchPlayers()
    {
        SetCurrentPlayerCameraPosition();
        GameManager.GetGameManager().selectedTile.ResetTile();
        if (currentPlayer == null || currentPlayer == players[(players.Length - 1)] || players.Length == 1)
        {
            IncreasePlayerResources();
            currentPlayer = players[0];
            turns = amountOfTurnsPerPlayer;
            currentTurn = 1;
            SetCurrentPlayerCamera();
            ShowEnemyObjects();
            ShowCurrentPlayerObjects();
        }
        else
        {
            IncreasePlayerResources();
            for (int i = 0; i < players.Length; i++)
            {
                if (currentPlayer == players[i])
                {
                    currentPlayer = players[(i + 1)];
                    turns = amountOfTurnsPerPlayer;
                    currentTurn = 1;
                    SetCurrentPlayerCamera();
                    ShowEnemyObjects();
                    ShowCurrentPlayerObjects();
                    return;
                }
            }
        }
    }

    public void ShowCurrentPlayerObjects()
    {
        GridGenerator gen = GridGenerator.GetGridGenerator();
        foreach (Tile t in gen.terrain)
        {
            if (t.buildUnit != null && t.buildUnit.Player.ID == currentPlayer.ID)
            {
                t.buildUnit.ColorObject(BuildUnitColor.Default);
            }
        }
    }

    public void ShowEnemyObjects()
    {
        GridGenerator gen = GridGenerator.GetGridGenerator();
        foreach(Tile t in gen.terrain)
        {
            if(t.buildUnit != null && t.buildUnit.Player.ID != currentPlayer.ID)
            {
                t.buildUnit.Blocked();
            }
        }
    }

    public void ResetTurns()
    {
        turns = amountOfTurnsPerPlayer;
    }

    public void Turn()
    {
        if ((turns - 1) > 0)
        {
            turns--;
            currentTurn++;
        }
        else
        {
            SwitchPlayers();
        }
    }

    public void IncreasePlayerResources()
    {
        for (int i = 0; i < defaultResourceIncrease.Length; i++)
        {
            currentPlayer.AddResources((ResourceType)i, defaultResourceIncrease[i]);
        }
    }

    public void CheckResourceArrayLength()
    {
        amountOfResources = GameManager.amountOfResources;
        if (defaultResourceIncrease.Length < amountOfResources)
        {
            Debug.LogError("Array length needs to be:" + amountOfResources);
            defaultResourceIncrease = new int[amountOfResources];
        }
        else if (defaultResourceIncrease.Length > amountOfResources)
        {
            Debug.LogError("The array is a bit too long so not all values will be used!");
            defaultResourceIncrease = new int[amountOfResources];
        }
    }

    private void SetCurrentPlayerCamera()
    {
        GameManager gm = GameManager.GetGameManager();
        gm.currentCamera.transform.position = GetCurrentPlayerCameraPosition();
    }

    public void SetCurrentPlayerCameraPosition()
    {
        GameManager gm = GameManager.GetGameManager();
        currentPlayer.cameraPosition = gm.currentCamera.transform.position;
    }

    public Vector3 GetCurrentPlayerCameraPosition()
    {
        return currentPlayer.cameraPosition;
    }
}
