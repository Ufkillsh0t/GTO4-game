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
    public Color SelectedHighlightColor = Color.blue;

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

    }

    /// <summary>
    /// Wordt uitgevoerd wanneer er op dit tile geklikt word.
    /// </summary>
    public void MouseClick()
    {
        ResetGameManagerTile();
        selected = !selected;
        Debug.Log("Test click");
    }

    /// <summary>
    /// Wordt uitgevoerd wanneer een andere tile geselecteerd word.
    /// </summary>
    public void MouseExit()
    {
        Debug.Log("Test exit");
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
        render.material.color = defaultMaterialColor;
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
