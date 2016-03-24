using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    private static int uniqueID; //Eventueel voor unieke identificatie van de tiles;
    private Renderer render;
    private Color defaultMaterialColor;

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
                Tile t = gm.selectedTile.GetComponent<Tile>();
                if (t.ID == ID)
                {
                    gm.selectedTile = gameObject;
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
            Tile t = gm.selectedTile.GetComponent<Tile>();
            if (t != null && t.ID != ID)
            {
                t.ResetTile();
                gm.selectedTile = gameObject;
            }
        }
        else
        {
            gm.selectedTile = gameObject;
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
}
