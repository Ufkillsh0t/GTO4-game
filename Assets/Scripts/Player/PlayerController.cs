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

    [Range(2, 4)]
    public float waitingTimeBetweenTurn = 2.0f;

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
                        GameOver();
                        return;
                    }
                }
            }
        }
        GameOver();
    }

    /// <summary>
    /// Wordt uitgevoerd om te controleren of de speler nog genoeg resources heeft.
    /// </summary>
    public void GameOver()
    {
        for (int i = 0; i < players.Length; i++)
        {
            bool canBuy = CanBuy(players[i]);
            bool hasBuildUnits = HasBuildUnits(players[i]);
            Debug.Log("Can Buy: " + canBuy + " Has BuildUnits: " + hasBuildUnits);
            if (!canBuy && !hasBuildUnits)
            {
                SetGameOver((i > 0) ? 0 : 1);
            }
        }
    }

    private bool HasBuildUnits(Player p)
    {
        int buildings = (p.buildings == null) ? 0 : p.buildings.Length;
        int units = (p.units == null) ? 0 : p.units.Length;
        return units > 0 || buildings > 0;
    }

    private bool CanBuy(Player p)
    {
        for (int i = 0; i < p.resources.Length; i++)
        {
            Building[] buildings = GameManager.GetGameManager().buildingClasses;
            Unit[] units = GameManager.GetGameManager().unitClasses;
            for (int j = 0; j < buildings.Length; j++)
            {
                if (p.EnoughResources(buildings[j].buildingCost))
                {
                    Debug.Log("Can buy: " + buildings[j]);
                    return true;
                }
            }
            for (int k = 0; k < units.Length; k++)
            {
                if (p.EnoughResources(units[k].unitCost))
                {
                    Debug.Log("Can buy: " + units[k]);
                    return true;
                }
            }
        }
        return false;
    }

    public void SetGameOver(int currentPlayer)
    {
        gameOver = true;
        GameManager.GetGameManager().GameOverMenu(players[currentPlayer]);
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
                if (!t.selected && !t.hover)
                {
                    t.ColorTile(TileColor.Spawn);
                }else if (t.hover)
                {
                    t.ColorTile(TileColor.Hover);
                }
            }
            else
            {
                t.ColorTile(TileColor.Default);
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
        gm.moveCamera.SetNewPosition(GetCurrentPlayerCameraPosition());
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

    private IEnumerator SwitchPlayerWait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SwitchPlayers();
    }
}
