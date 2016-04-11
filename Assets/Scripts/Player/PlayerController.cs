using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public Player currentPlayer;
    public Player[] players;
    private GridGenerator gen;

    public int[] defaultResourceIncrease;
    private int amountOfResources;
    public int amountOfTurnsPerPlayer;
    public int currentTurn;
    private int turns;

    private bool gameOver;

    void Awake()
    {
        gameOver = false;
        turns = amountOfTurnsPerPlayer;
        CheckResourceArrayLength();
        gen = GridGenerator.GetGridGenerator();
    }

    /// <summary>
    /// Voegt een speler toe aan de array met spelers.
    /// </summary>
    /// <param name="player">De speler die je aan der array met spelers wilt toevoegen.</param>
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

    /// <summary>
    /// Wisselt tussen de spelers.
    /// </summary>
    public void SwitchPlayers()
    {
        SetCurrentPlayerCameraPosition();
        if (GameManager.GetGameManager().selectedTile != null)
        {
            GameManager.GetGameManager().selectedTile.ResetTile();
        }
        GameOver();
        if (!gameOver)
        {
            if (currentPlayer == null || currentPlayer == players[(players.Length - 1)] || players.Length == 1)
            {
                IncreasePlayerResources();
                currentPlayer = players[0];
                turns = amountOfTurnsPerPlayer;
                currentTurn = 1;
                SetCurrentPlayerCamera();
                ShowEnemyObjects();
                ShowSpawnableTiles();
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
                        ShowSpawnableTiles();
                        ShowCurrentPlayerObjects();
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Wordt uitgevoerd om te controleren of de speler nog genoeg resources heeft.
    /// </summary>
    public void GameOver()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].buildings == null || players[i].buildings.Length == 0)
            {
                int lowResource = 0;
                for (int j = 0; j < players[i].resources.Length; j++)
                {
                    if (players[i].resources[j] <= 50)
                    {
                        lowResource++;
                        if ((currentPlayer.buildings == null && currentPlayer.units == null))
                        {
                            SetGameOver();
                        }
                        else if (currentPlayer.buildings != null && currentPlayer.units != null && currentPlayer.units.Length == 0 && currentPlayer.buildings.Length == 0)
                        {
                            SetGameOver();
                        }
                        else if (currentPlayer.buildings != null && currentPlayer.units == null && currentPlayer.buildings.Length == 0)
                        {
                            SetGameOver();
                        }
                        else if (currentPlayer.units != null && currentPlayer.buildings == null && currentPlayer.units.Length == 0)
                        {
                            SetGameOver();
                        }
                    }
                }
            }
        }
    }

    public void SetGameOver()
    {
        if (players[0].ID == currentPlayer.ID)
        {
            gameOver = true;
            GameManager.GetGameManager().GameOverMenu(players[0]);
        }
        else
        {
            gameOver = true;
            GameManager.GetGameManager().GameOverMenu(players[1]);
        }
    }

    /// <summary>
    /// Laat alle objecten zien van de huidige speler.
    /// </summary>
    public void ShowCurrentPlayerObjects()
    {
        gen = GridGenerator.GetGridGenerator();
        foreach (Tile t in gen.terrain)
        {
            if (t.buildUnit != null && t.buildUnit.Player.ID == currentPlayer.ID)
            {
                t.buildUnit.ColorObject(BuildUnitColor.Default);
            }
        }
    }

    /// <summary>
    /// Laat alle objecten van de vijanden zien.
    /// </summary>
    public void ShowEnemyObjects()
    {
        gen = GridGenerator.GetGridGenerator();
        foreach (Tile t in gen.terrain)
        {
            if (t.buildUnit != null && t.buildUnit.Player.ID != currentPlayer.ID)
            {
                t.buildUnit.Blocked();
            }
        }
    }

    /// <summary>
    /// Laat alle spawnbare tiles zien.
    /// </summary>
    public void ShowSpawnableTiles()
    {
        foreach (Tile t in gen.terrain)
        {
            if (t.PlayerID == currentPlayer.ID && t.buildUnit == null)
            {
                t.ColorTile(TileColor.Spawn);
            }
            else
            {
                if (!t.selected && !t.hover)
                {
                    t.ColorTile(TileColor.Default);
                }
            }
        }
    }

    /// <summary>
    /// Reset het aantal turns van de speler.
    /// </summary>
    public void ResetTurns()
    {
        turns = amountOfTurnsPerPlayer;
    }

    /// <summary>
    /// Voert een turn uit en switcht de speler wanneer al zijn turns voorbij zijn.
    /// </summary>
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

    /// <summary>
    /// Verhoogt de resources van de speler.
    /// </summary>
    public void IncreasePlayerResources()
    {
        for (int i = 0; i < defaultResourceIncrease.Length; i++)
        {
            currentPlayer.AddResources((ResourceType)i, defaultResourceIncrease[i]);
        }

        if (currentPlayer.buildings != null)
        {
            foreach (Building b in currentPlayer.buildings)
            {
                if (b.buildingType == BuildingType.Harvester)
                {
                    for (int i = 0; i < b.resourcesIncrease.Length; i++)
                    {
                        currentPlayer.AddResources((ResourceType)i, b.resourcesIncrease[i]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Kijkt naar de lengte van de resource array en controleert of die groot genoeg is voor alle resources in het spel.
    /// </summary>
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

    /// <summary>
    /// Zet op dit moment de spawnpoints van 2 spelers klaar.
    /// </summary>
    public void SetPlayerSpawnPoints()
    {
        if (players.Length == 2)
        {
            int terX = gen.terrain.GetLength(0);
            int terY = gen.terrain.GetLength(1);
            int maxX = (int)(terX / 3.5);

            //spawnable player1
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < terY; y++)
                {
                    gen.terrain[x, y].PlayerID = players[0].ID;
                }
            }

            //spawnable player2
            for (int x = (terX - maxX); x < terX; x++)
            {
                for (int y = 0; y < terY; y++)
                {
                    gen.terrain[x, y].PlayerID = players[1].ID;
                }
            }
        }
    }

    /// <summary>
    /// Zet de huidige speler camera.
    /// </summary>
    private void SetCurrentPlayerCamera()
    {
        GameManager gm = GameManager.GetGameManager();
        gm.currentCamera.transform.position = GetCurrentPlayerCameraPosition();
    }

    /// <summary>
    /// Slaat de camera positie van de speler op.
    /// </summary>
    public void SetCurrentPlayerCameraPosition()
    {
        GameManager gm = GameManager.GetGameManager();
        currentPlayer.cameraPosition = gm.currentCamera.transform.position;
    }

    /// <summary>
    /// Verkrijgt de camera positie van de huidige speler.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCurrentPlayerCameraPosition()
    {
        return currentPlayer.cameraPosition;
    }
}
