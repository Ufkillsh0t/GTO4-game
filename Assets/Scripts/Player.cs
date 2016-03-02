using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private int mana;
    private int gold;
    private int lumber;

    public Camera playerCamera;

    public Player(int mana, int gold, int lumber)
    {
        this.mana = mana;
        this.gold = gold;
        this.lumber = lumber;
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
