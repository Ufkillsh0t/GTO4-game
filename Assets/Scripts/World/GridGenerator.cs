using UnityEngine;
using System.Collections;

public class GridGenerator : MonoBehaviour
{

    public Transform tilePrefab;

    public float pointX = -5;

    public float pointZ = -10;

    [Range(1, 128)]
    public int terrainWidth = 20;

    [Range(1, 128)]
    public int terrainHeight = 20;

    [Range(1, 128)] //Can be used for offsets or objects which are larger than 1 x 1.
    public float tilesize = 1f;

    public Transform[,] terrain;

    // Use this for initialization
    void OnEnable()
    {
        if (tilePrefab == null)
        {
            tilePrefab = Resources.Load<Transform>("Prefabs/Tile/Tile");
        }

        if (!tilePrefab)
        {
            Debug.LogWarning("Couldn't load the tileprefab!");
        }

        GenerateTerrain();
    }

    /// <summary>
    /// Dit zorgt er voor dat alle opnieuw gegenereert word indien je in engine iets aan de x en y as veranderd.
    /// </summary>
    public void Refresh()
    {
        foreach(Transform child in terrain)
        {
            Destroy(child.gameObject);
        }
        GenerateTerrain();
    }

    /// <summary>
    /// Genereert een array met tiles afhankelijk van de x en y die je gegeven hebt.
    /// </summary>
    public void GenerateTerrain()
    {
        terrain = new Transform[terrainWidth, terrainHeight];

        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                Transform tile = Instantiate(tilePrefab, new Vector3(pointX + (x * tilesize), 0, pointZ + (y * tilesize)), tilePrefab.rotation) as Transform;

                tile.parent = transform;

                terrain[x, y] = tile;
            }
        }
    }

    /// <summary>
    /// Verkrijgt een tile in de terrain array op basis van zijn x en y positie in die array;
    /// </summary>
    /// <param name="x">X positie in de array</param>
    /// <param name="y">Y positie in de array</param>
    /// <returns></returns>
    public Transform GetTileTerrain(int x, int y)
    {
        if (x < 0 || y < 0 || x > terrainWidth || y > terrainHeight)
        {
            Debug.LogWarning("The inserted x and y are OutOfBound!");
            return null;
        }
        return terrain[x, y];
    }

    /// <summary>
    /// Verkrijgt de array op basis van de wereld.
    /// </summary>
    /// <param name="x">De x float in de wereld</param>
    /// <param name="y">De y float in de wereld</param>
    /// <returns></returns>
    public Transform GetTileWorldSpace(float x, float y)
    {
        if(x < pointX || y < pointZ || x > (pointX + (terrainWidth * tilesize)) || y > (pointZ + (terrainHeight * tilesize)))
        {
            Debug.LogWarning("The inserted x and y are OutOfBound!");
            return null;
        }
        return terrain[(int)(x - pointX), (int)(y - pointZ)];
    }
}
