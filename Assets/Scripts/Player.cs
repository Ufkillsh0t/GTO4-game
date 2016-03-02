using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int mana;
    public int gold;
    public int lumber;
    public string playerName;

    public Camera playerCamera;

    public void addPlayerName(string playerName)
    {
        this.playerName = playerName;
    }
	
    public void addResources(int mana, int gold, int lumber)
    {
        this.mana += mana;
        this.gold += gold;
        this.lumber += lumber;
    }

    public void removeResources(int mana, int gold, int lumber)
    {
        this.mana -= mana;
        this.gold -= gold;
        this.lumber -= lumber;
    }
}
