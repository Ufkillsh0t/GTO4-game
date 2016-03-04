using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    private Renderer render;
    public bool tilePressed;


    void Awake()
    {
        render = gameObject.GetComponent<Renderer>();
        tilePressed = false;
    }

	void OnMouseDown()
    {
        render.material.color = Color.red;
        tilePressed = !tilePressed;
    }

    void OnMouseEnter()
    {
        render.material.color = Color.red;
    }

    void OnMouseExit()
    {
        if (!tilePressed)
        {
            render.material.color = Color.white;
        }
    }
}
