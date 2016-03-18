using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int[] resources;
    public string playerName;
    public Vector3 cameraPosition;
    public Building[] buildings;

    void Awake()
    {
        resources = new int[System.Enum.GetNames(typeof(ResourceType)).Length];
    }

    public void addPlayerName(string playerName)
    {
        this.playerName = playerName;
    }
	
    public void addResources(ResourceType resource, int amount)
    {
        resources[(int)resource] += amount;
    }

    public void removeResources(ResourceType resource, int amount)
    {
        resources[(int)resource] -= amount;
    }
}
