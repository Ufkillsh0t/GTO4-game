using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    private static int uniqueID; //Eventueel voor unieke identificatie van de tiles;
    private Renderer render;
    private Color defaultMaterialColor;

    public GameObject currentGameObject;
    public bool tilePressed;
    public int ID;

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
    }

    void OnMouseExit()
    {
        if (!tilePressed)
        {
            render.material.color = defaultMaterialColor;
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
    /// Reset de tile naar zijn default color.
    /// </summary>
    public void ResetTile()
    {
        tilePressed = false;
        render.material.color = defaultMaterialColor;
    }

    public GameObject SpawnObject(GameObject g)
    {
        if(currentGameObject != null)
        {
            Debug.Log("There already is an object on this tile!");
            return null;
        }
        else
        {
            GameObject current = Instantiate(g);
            current.transform.parent = transform;
            current.transform.position = transform.parent.position;
            current.transform.localPosition = new Vector3(0f, 0f, -0.5f);
            current.transform.rotation = Quaternion.identity;           
            //current.transform.localRotation = Quaternion.identity;
            Debug.Log(current);
            return current;
        }
    }
}
