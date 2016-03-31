using UnityEngine;
using System.Collections;

public interface IBuildUnit {
    Player Player { get; }
    bool Attack();
    bool Defend();
    bool Ugrade();
    void OnSpawn(Tile t, Player p);
}
