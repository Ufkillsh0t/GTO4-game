using UnityEngine;
using System.Collections;

public class GridGenerator : MonoBehaviour
{

    public Transform tilePrefab;

    public float pointX = 0;

    public float pointY = 0;

    [Range(1, 128)]
    public int terrainWidth = 5;

    [Range(1, 128)]
    public int terrainHeight = 5;

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

    public void Refresh()
    {
        foreach(Transform child in terrain)
        {
            Destroy(child.gameObject);
        }
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        terrain = new Transform[terrainWidth, terrainHeight];

        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                Transform tile = Instantiate(tilePrefab, new Vector3(pointX + (x * tilesize), 0, pointY + (y * tilesize)), tilePrefab.rotation) as Transform;

                tile.parent = transform;

                terrain[x, y] = tile;
            }
        }
    }

    public Transform GetTileTerrain(int x, int y)
    {
        if (x < 0 || y < 0 || x > terrainWidth || y > terrainHeight)
        {
            Debug.LogWarning("The inserted x and y are OutOfBound!");
            return null;
        }
        return terrain[x, y];
    }

    public Transform GetTileWorldSpace(float x, float y)
    {
        if(x < pointX || y < pointY || x > (pointX + terrainWidth) || y > (pointY + terrainHeight))
        {
            Debug.LogWarning("The inserted x and y are OutOfBound!");
            return null;
        }
        return terrain[(int)(x - pointX), (int)(y - pointY)];
    }
}
