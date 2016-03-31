using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    private static int uniqueID; //Eventueel voor unieke identificatie van de tiles;
    private Renderer render;
    private GameManager gm;
    private GridGenerator gen;

    private Color defaultMaterialColor;
    public Color DefaultMaterialColor { get { return defaultMaterialColor; } }

    public Color highlightedColor = Color.yellow;
    public Color selectedColor = Color.green;
    public Color hoverColor = Color.green;
    public Color blockedTileColor = Color.red;

    public GameObject currentGameObject;
    public IBuildUnit buildUnit;
    public bool selected;
    public bool hover;
    public bool highlighted;
    public int ID;

    public int xTile;
    public int yTile;

    public Transform shape;

    void Awake()
    {
        render = gameObject.GetComponent<Renderer>(); //Verkrijgt de renderer van dit object.
        defaultMaterialColor = render.material.color; //Verkrijgt de huidige kleur van dit object.
        selected = false; //Kijkt of de tile ingedrukt is of niet.
        uniqueID = uniqueID + 1;
        ID = uniqueID;
    }

    void OnMouseDown()
    {
        ResetGameManagerTile();
        render.material.color = selectedColor;
        selected = !selected;
        highlighted = false;
    }

    void OnMouseEnter()
    {
        render.material.color = selectedColor;
        hover = true;
        if (buildUnit != null)
        {
            HighLightNearbyTiles(2, RangeType.Cross, true);
        }
    }

    void OnMouseExit()
    {
        hover = false;
        if (!selected)
        {
            if (buildUnit != null)
            {
                HighLightNearbyTiles(2, RangeType.Cross, false);
            }
            else if (highlighted)
            {
                render.material.color = highlightedColor;
            }
            else
            {
                render.material.color = defaultMaterialColor;
            }
        }
    }

    //Code hieronder is misschien overbodig.
    //gm = GameManager.GetGameManager();
    //if (gm.selectedTile != null)
    //{
    //    Tile t = gm.selectedTile;
    //    if (t.ID == ID)
    //    {
    //        gm.selectedTile = this;
    //    }
    //}

    /// <summary>
    /// Reset de huidig geselecteerde tile in de gamemanager.
    /// </summary>
    public void ResetGameManagerTile()
    {
        gm = GameManager.GetGameManager();
        if (gm.selectedTile != null)
        {
            Tile t = gm.selectedTile;
            if (t != null && t.ID != ID)
            {
                t.ResetTile();
                gm.selectedTile = this;
            }
        }
        else
        {
            gm.selectedTile = this;
        }
    }

    /// <summary>
    /// Zet de coördinaten van tile vast in paramaters.
    /// </summary>
    /// <param name="x">x coörd van de tile</param>
    /// <param name="y">y coörd van de tile</param>
    public void SetCoords(int x, int y)
    {
        xTile = x;
        yTile = y;
    }

    /// <summary>
    /// Dit zorgt er voor dat een tile gehighlight wordt of geunlight.
    /// </summary>
    /// <param name="highlight">Of de tile gehighlight moet worden of niet.</param>
    public void HighlightTile(bool highlight)
    {
        if (highlight && buildUnit == null)
        {
            highlighted = true;
            render.material.color = highlightedColor;
        }
        else if (selected && highlight)
        {
            render.material.color = selectedColor;
        }
        else if (buildUnit != null && hover)
        {
            if (buildUnit.Player.GetPlayerID == gm.GetPlayerController.currentPlayer.GetPlayerID)
            {
                render.material.color = hoverColor;
            }
            else
            {
                render.material.color = blockedTileColor;
            }
        }
        else
        {
            highlighted = false;
            render.material.color = defaultMaterialColor;
        }
    }

    /// <summary>
    /// Reset de tile naar zijn default color.
    /// </summary>
    public void ResetTile()
    {
        selected = false;
        render.material.color = defaultMaterialColor;
        if (buildUnit != null)
        {
            HighLightNearbyTiles(2, RangeType.Cross, false);
        }
    }

    //RaycastHit[] hit = Physics.BoxCastAll(gameObject.transform.position, new Vector3(0.5f, 0.5f), new Vector3(1f, 0f, 0f), Quaternion.identity, range); //new Ray(gameObject.transform.position, new Vector3(1, 0, 1)), range 4 of 6de overload gebruiken (kan dus eventueel met layers.)
    //Debug.DrawRay(gameObject.transform.position, new Vector3(1f, 0f, 0f), Color.yellow, 10);
    //foreach (RaycastHit r in hit)
    //{
    //    if (r.collider.gameObject.tag == "Tile")
    //    {
    //        Tile t = r.collider.gameObject.GetComponent<Tile>();
    //        t.render.material.color = t.DefaultMaterialColor;
    //    }
    //}

    /// <summary>
    /// Highlight de tiles rondom deze tile binnen een bepaalde range afhankelijk van het range type.
    /// </summary>
    /// <param name="range"></param>
    /// <param name="type"></param>
    /// <param name="highlight"></param>
    public void HighLightNearbyTiles(int range, RangeType type, bool highlight)
    {
        if (buildUnit != null)
        {
            if (buildUnit.Player.GetPlayerID == gm.GetPlayerController.currentPlayer.GetPlayerID)
            {
                gen = GridGenerator.GetGridGenerator();

                int minX = xTile - range;
                int minY = yTile - range;
                int maxX = xTile + range;
                int maxY = yTile + range;
                if (minX < 0) minX = 0;
                if (minY < 0) minY = 0;
                if (maxX >= gen.terrainWidth) maxX = gen.terrainWidth - 1;
                if (maxY >= gen.terrainHeight) maxY = gen.terrainHeight - 1;

                if (type == RangeType.Cross)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        gen.terrain[x, yTile].HighlightTile(highlight);
                    }
                    for (int y = minY; y <= maxY; y++)
                    {
                        gen.terrain[xTile, y].HighlightTile(highlight);
                    }
                }
                else
                {
                    //Formule voor sphere highlighting
                }
            }
            else
            {
                HighlightTile(false);
            }
        }
        else
        {
            Debug.Log("Er is geen gebouw dat gehighlight kan worden!");
        }
    }

    //RaycastHit[] hit = Physics.BoxCastAll(gameObject.transform.position, new Vector3(0.5f, 0.5f), new Vector3(1f, 0f, 0f), Quaternion.identity, range); //new Ray(gameObject.transform.position, new Vector3(1, 0, 1)), range 4 of 6de overload gebruiken (kan dus eventueel met layers.)
    //Debug.DrawRay(gameObject.transform.position, new Vector3(1f, 0f, 0f), Color.yellow, 10);
    //foreach (RaycastHit r in hit)
    //{
    //    if (r.collider.gameObject.tag == "Tile")
    //    {
    //        Tile t = r.collider.gameObject.GetComponent<Tile>();
    //        t.render.material.color = Color.yellow;
    //    }
    //}

    public GameObject SpawnObject(GameObject g)
    {
        if (currentGameObject != null)
        {
            Debug.Log("There already is an object on this tile!");
            return currentGameObject;
        }
        else
        {
            currentGameObject = Instantiate(g);
            currentGameObject.transform.parent = transform;
            currentGameObject.transform.position = transform.parent.position;
            currentGameObject.transform.localPosition = new Vector3(0f, 0f, (currentGameObject.GetComponent<Renderer>().bounds.size.y / 2));
            currentGameObject.transform.rotation = Quaternion.identity;
            //current.transform.localRotation = Quaternion.identity;
            if (currentGameObject != null)
            {
                buildUnit = currentGameObject.GetComponent<IBuildUnit>();
                //HighLightNearbyTiles(2, RangeType.Cross, true);
            }

            Debug.Log(currentGameObject);
            return currentGameObject;
        }
    }
}
