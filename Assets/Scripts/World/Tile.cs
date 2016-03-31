using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    private static int uniqueID; //Eventueel voor unieke identificatie van de tiles;
    private Renderer render;

    private Color defaultMaterialColor;
    public Color DefaultMaterialColor { get { return defaultMaterialColor; } }

    public Color highlightedColor = Color.yellow;

    public GameObject currentGameObject;
    public bool tilePressed;
    public bool highlighted;
    public int ID;

    public int x;
    public int y;

    public Transform shape;

    void Awake()
    {
        render = gameObject.GetComponent<Renderer>(); //Verkrijgt de renderer van dit object.
        defaultMaterialColor = render.material.color; //Verkrijgt de huidige kleur van dit object.
        tilePressed = false; //Kijkt of de tile ingedrukt is of niet.
        uniqueID = uniqueID + 1;
        ID = uniqueID;
    }

    void OnMouseDown()
    {
        render.material.color = Color.red;
        tilePressed = !tilePressed;
        GetGameManagerTile();
    }

    void OnMouseEnter()
    {
        render.material.color = Color.red;
        if (currentGameObject != null)
        {
            HighLightNearbyTiles(2, RangeType.Cross);
        }
    }

    void OnMouseExit()
    {
        if (!tilePressed)
        {
            if (highlighted)
            {
                render.material.color = highlightedColor;
            }
            else
            {
                render.material.color = defaultMaterialColor;
            }

            //Code hieronder is misschien overbodig.
            GameManager gm = GameManager.GetGameManager();
            if (gm.selectedTile != null)
            {
                Tile t = gm.selectedTile;
                if (t.ID == ID)
                {
                    gm.selectedTile = this;
                }
            }
        }
        else
        {
            if (currentGameObject != null)
            {
                UnlightNearbyTiles(2, RangeType.Cross);
            }
        }
    }

    /// <summary>
    /// Verkrijgt de huidig geselecteerde tile in de gamemanager.
    /// </summary>
    public void GetGameManagerTile()
    {
        GameManager gm = GameManager.GetGameManager();
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
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetCoords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Reset de tile naar zijn default color.
    /// </summary>
    public void ResetTile()
    {
        tilePressed = false;
        render.material.color = defaultMaterialColor;
    }

    public void UnlightNearbyTiles(float range, RangeType type)
    {
        if (type == RangeType.Cross)
        {
            RaycastHit[] hit = Physics.BoxCastAll(gameObject.transform.position, new Vector3(0.5f, 0.5f), new Vector3(1f, 0f, 0f), Quaternion.identity, range); //new Ray(gameObject.transform.position, new Vector3(1, 0, 1)), range 4 of 6de overload gebruiken (kan dus eventueel met layers.)
            Debug.DrawRay(gameObject.transform.position, new Vector3(1f, 0f, 0f), Color.yellow, 10);
            foreach (RaycastHit r in hit)
            {
                if (r.collider.gameObject.tag == "Tile")
                {
                    Tile t = r.collider.gameObject.GetComponent<Tile>();
                    t.render.material.color = t.DefaultMaterialColor;
                }
            }
        }
        else
        {

        }
    }

    public void HighLightNearbyTiles(float range, RangeType type)
    {
        /*GridGenerator gen = GridGenerator.GetGridGenerator();
        gen.til*/

        
        if (type == RangeType.Cross)
        {
            RaycastHit[] hit = Physics.BoxCastAll(gameObject.transform.position, new Vector3(0.5f, 0.5f), new Vector3(1f, 0f, 0f), Quaternion.identity, range); //new Ray(gameObject.transform.position, new Vector3(1, 0, 1)), range 4 of 6de overload gebruiken (kan dus eventueel met layers.)
            Debug.DrawRay(gameObject.transform.position, new Vector3(1f, 0f, 0f), Color.yellow, 10);
            foreach (RaycastHit r in hit)
            {
                if (r.collider.gameObject.tag == "Tile")
                {
                    Tile t = r.collider.gameObject.GetComponent<Tile>();
                    t.render.material.color = Color.yellow;
                }
            }
        }
        else
        {

        }
    }

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
            Debug.Log(currentGameObject);
            return currentGameObject;
        }
    }
}
