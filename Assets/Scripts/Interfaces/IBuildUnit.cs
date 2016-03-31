using UnityEngine;
using System.Collections;

public interface IBuildUnit {
    bool Attack();
    bool Defend();
    bool Ugrade();
    void OnSpawn(Tile t, Player p);
}
