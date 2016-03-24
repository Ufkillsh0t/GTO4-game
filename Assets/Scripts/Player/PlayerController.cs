using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public Player currentPlayer;
    public Player[] players;

    public int[] defaultResourceIncrease;
    private int amountOfResources;

    void Awake()
    {
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
            SetCurrentPlayerCamera();
        }
        else
        {
            IncreasePlayerResources();
            for (int i = 0; i < players.Length; i++)
            {
                if (currentPlayer == players[i])
                {
                    currentPlayer = players[(i + 1)];
                    SetCurrentPlayerCamera();
                    return;
                }
            }
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
        amountOfResources = System.Enum.GetNames(typeof(ResourceType)).Length;
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
