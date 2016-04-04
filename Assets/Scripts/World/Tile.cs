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

    public Color attackColor = Color.magenta;
    public Color blockedColor = Color.red;
    public Color highlightColor = Color.yellow;
    public Color hoverColor = Color.green;
    public Color moveColor = Color.blue;
    public Color selectedColor = Color.green;

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

    /// <summary>
    /// Wordt uitgevoerd wanneer er een muis over een tile hovert.
    /// </summary>
    public void MouseHover()
    {
        if (selected == false)
        {
            HoverTile();
        }
    }

    /// <summary>
    /// Wordt uitgevoerd wanneer er op dit tile geklikt word.
    /// </summary>
    public void MouseClick()
    {
        SelectTile();
    }

    /// <summary>
    /// Wordt uitgevoerd wanneer een andere tile geselecteerd word.
    /// </summary>
    public void MouseExit()
    {
        if (!selected && hover)
        {
            ColorTile(TileColor.Default);
            if (buildUnit != null)
            {
                HighLightNearbyTiles(buildUnit.GetRange(), buildUnit.GetRangeType(), false);
                if (gm.selectedTile != null)
                {
                    gm.selectedTile.SelectTile();
                }
            }
        }
        if (highlighted)
        {
            if (buildUnit != null && buildUnit.Player.ID == gm.GetPlayerController.currentPlayer.ID)
            {
                ColorTile(TileColor.Blocked);
            }
            else
            {
                ColorTile(TileColor.Highlight);
            }
        }
        hover = false;
    }

    /// <summary>
    /// Wordt uitgevoerd wanneer de mouse op een tile zit.
    /// </summary>
    public void HoverTile()
    {
        hover = true;
        if (!highlighted && buildUnit != null)
        {
            if (buildUnit.Player.ID == gm.GetPlayerController.currentPlayer.ID)
            {
                ColorTile(TileColor.Hover);
                HighLightNearbyTiles(buildUnit.GetRange(), buildUnit.GetRangeType(), true);
            }
            else
            {
                ColorTile(TileColor.Blocked);
            }
        }
        else if (highlighted)
        {
            if (buildUnit != null)
            {
                if (buildUnit.Player.ID == gm.GetPlayerController.currentPlayer.ID)
                {
                    ColorTile(TileColor.Hover);
                }
                else
                {
                    ColorTile(TileColor.Attack);
                }
            }
            else
            {
                ColorTile(TileColor.Move);
            }
        }
        else
        {
            ColorTile(TileColor.Hover);
        }
    }

    /// <summary>
    /// Geeft de selecteer kleur aan de gegeven tile.
    /// </summary>
    public void SelectTile()
    {
        ResetGameManagerTile();
        selected = true;
        highlighted = false;
        if (buildUnit != null)
        {
            if (buildUnit.Player.ID == gm.GetPlayerController.currentPlayer.ID)
            {
                ColorTile(TileColor.Selected);
                HighLightNearbyTiles(buildUnit.GetRange(), buildUnit.GetRangeType(), true);
            }
            else
            {
                ColorTile(TileColor.Blocked);
            }
        }
        else
        {
            ColorTile(TileColor.Selected);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="range"></param>
    /// <param name="type"></param>
    /// <param name="highlight"></param>
    public void HighLightNearbyTiles(int range, RangeType type, bool highlight)
    {
        if (range > 0)
        {
            gen = GridGenerator.GetGridGenerator();

            int minX = xTile - range;
            int minY = yTile - range;
            int maxX = xTile + range;
            int maxY = yTile + range;
            int lxTile = xTile - 1;
            int hxTile = xTile + 1;
            int lyTile = yTile - 1;
            int hyTile = yTile + 1;
            if (minX < 0) minX = 0;
            if (minY < 0) minY = 0;
            if (maxX >= gen.terrainWidth) maxX = gen.terrainWidth - 1;
            if (maxY >= gen.terrainHeight) maxY = gen.terrainHeight - 1;

            if (type == RangeType.Cross)
            {
                HighLightTilesLeft(lxTile, minX, highlight);
                HighLightTilesRight(hxTile, maxX, highlight);
                HighLightTilesUp(hyTile, maxY, highlight);
                HighLightTilesDown(lyTile, minY, highlight);
            }
            else
            {
                //Formule voor sphere highlighting
            }
        }
        else
        {
            Debug.Log("Range is 0 of" + this + " " + ID);
        }
    }

    /// <summary>
    /// Highlights alle tiles aan de linkerkant van de huidige tile.
    /// </summary>
    /// <param name="lxTile">Begin tile die gehighlight moet worden</param>
    /// <param name="minX">Hoeveel tiles er gehighlight mogen worden</param>
    /// <param name="highlight">Of de bool gehighlight of geunlight moet worden</param>
    public void HighLightTilesLeft(int lxTile, int minX, bool highlight)
    {
        bool blocked = false;
        for (int x = lxTile; x >= minX; x--) //Todo: niet door drawen wanneer er iets in de weg zit en vanuit het object loopen, zodat je makkelijk uit de for loop kan gaan wanneer een object in de weg zit.
        {
            Tile t = gen.terrain[x, yTile];
            if (t.buildUnit == null && !blocked)
            {
                t.HighlightTile(highlight);
            }
            if (t.buildUnit != null && !blocked)
            {
                blocked = true;
                t.HighlightTile(highlight);
            }
        }
    }

    /// <summary>
    /// Highlight alle tiles aan de rechterkant van de huidige tile.
    /// </summary>
    /// <param name="hxTile">Begin tile die gehighlight moet worden</param>
    /// <param name="maxX">Hoeveel tiles er gehighlight mogen worden</param>
    /// <param name="highlight">Of de bool gehighlight of geunlight moet worden</param>
    public void HighLightTilesRight(int hxTile, int maxX, bool highlight)
    {
        bool blocked = false;
        for (int x = hxTile; x <= maxX; x++)
        {
            Tile t = gen.terrain[x, yTile];
            if (t.buildUnit == null && !blocked)
            {
                t.HighlightTile(highlight);
            }
            if (t.buildUnit != null && !blocked)
            {
                blocked = true;
                t.HighlightTile(highlight);
            }
        }
    }

    /// <summary>
    /// Highlight alle tiles aan de zuidelijke kant van de huidige tile.
    /// </summary>
    /// <param name="lyTile">Begin tile die gehighlight moet worden</param>
    /// <param name="minY">Hoeveel tiles er gehighlight mogen worden</param>
    /// <param name="highlight">Of de bool gehighlight of geunlight moet worden</param>
    public void HighLightTilesDown(int lyTile, int minY, bool highlight)
    {
        bool blocked = false;
        for (int y = lyTile; y >= minY; y--)
        {
            Tile t = gen.terrain[xTile, y];
            if (t.buildUnit == null && !blocked)
            {
                t.HighlightTile(highlight);
            }
            if (t.buildUnit != null && !blocked)
            {
                blocked = true;
                t.HighlightTile(highlight);
            }
        }
    }

    /// <summary>
    /// Highlight alle tiles aan de noordelijke kant van de huidige tile.
    /// </summary>
    /// <param name="hyTile">Begin tile die gehighlight moet worden</param>
    /// <param name="maxY">Hoeveel tiles er gehighlight mogen worden</param>
    /// <param name="highlight">Of de bool gehighlight of geunlight moet worden</param>
    public void HighLightTilesUp(int hyTile, int maxY, bool highlight)
    {
        bool blocked = false;
        for (int y = hyTile; y <= maxY; y++)
        {
            Tile t = gen.terrain[xTile, y];
            if (t.buildUnit == null && !blocked)
            {
                t.HighlightTile(highlight);
            }
            if (t.buildUnit != null && !blocked)
            {
                blocked = true;
                t.HighlightTile(highlight);
            }
        }
    }

    /// <summary>
    /// Highlights the current tile.
    /// </summary>
    /// <param name="highlight">If this object should be highlighted or not</param>
    public void HighlightTile(bool highlight)
    {
        if (!selected)
        {
            highlighted = highlight;
            if (hover && highlighted)
            {
                ColorTile(TileColor.Hover);
            }
            else if (highlighted)
            {
                if (buildUnit != null && buildUnit.Player.ID == gm.GetPlayerController.currentPlayer.ID)
                {
                    ColorTile(TileColor.Blocked);
                }
                else
                {
                    ColorTile(TileColor.Highlight);
                }
            }
            else
            {
                ColorTile(TileColor.Default);
            }
        }
    }

    /// <summary>
    /// Geeft de tile een van de 7 mogelijke kleuren.
    /// </summary>
    /// <param name="color">De kleur die de tile moet worden!</param>
    public void ColorTile(TileColor color)
    {
        switch (color)
        {
            case TileColor.Hover:
                render.material.color = hoverColor;
                break;
            case TileColor.Selected:
                render.material.color = selectedColor;
                break;
            case TileColor.Blocked:
                render.material.color = blockedColor;
                break;
            case TileColor.Highlight:
                render.material.color = highlightColor;
                break;
            case TileColor.Move:
                render.material.color = moveColor;
                break;
            case TileColor.Attack:
                render.material.color = attackColor;
                break;
            default:
            case TileColor.Default:
                render.material.color = defaultMaterialColor;
                break;
        }
    }

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
    /// Reset de tile naar zijn default color.
    /// </summary>
    public void ResetTile()
    {
        selected = false;
        ColorTile(TileColor.Default);
        if (buildUnit != null)
        {
            HighLightNearbyTiles(buildUnit.GetRange(), buildUnit.GetRangeType(), false);
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
    /// Spawnt een object op deze tile
    /// </summary>
    /// <param name="g">Het game object wat je wilt spawnen</param>
    /// <returns>Het gespawnde GameObject (Een clone van het meegegeven GameObject)</returns>
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
            if (currentGameObject != null)
            {
                buildUnit = currentGameObject.GetComponent<IBuildUnit>();
            }

            Debug.Log(currentGameObject);
            return currentGameObject;
        }
    }
}
