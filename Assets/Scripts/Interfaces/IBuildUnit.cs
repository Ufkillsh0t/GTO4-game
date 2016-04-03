using UnityEngine;
using System.Collections;

public interface IBuildUnit {
    Player Player { get; }

    /// <summary>
    /// Geeft de range van het IBuildUnit object.
    /// </summary>
    /// <returns>De range van dit IBuildUnit object</returns>
    int GetRange();

    /// <summary>
    /// Verkrijgt het rangeType van dit object.
    /// </summary>
    /// <returns>Het rangeType van dit object</returns>
    RangeType GetRangeType();

    /// <summary>
    /// Valt een andere IBuiltUnit object aan;
    /// </summary>
    /// <returns>Of de aanval is gelukt of niet</returns>
    bool Attack();

    /// <summary>
    /// Verdedigt zich tegen een aanval van een tegenstander.
    /// </summary>
    /// <returns>Of de verdediging is gelukt of niet</returns>
    bool Defend(int damage);

    /// <summary>
    /// Upgrade of verbeterd de huidige waarde van het IBuildUnit object.
    /// </summary>
    /// <returns>Of de upgrade is gelukt of niet</returns>
    bool Upgrade();

    /// <summary>
    /// Wordt uitgevoerd wanneer dit object is gespawnt, hierdoor wordt er een tile en speler meegegeven aan dit object.
    /// </summary>
    /// <param name="t">De tile waar dit object op is gespawnt</param>
    /// <param name="p">De speler van wie dit object is.</param>
    void OnSpawn(Tile t, Player p);

    /// <summary>
    /// Wordt uitgevoerd wanneer dit object is geselecteerd.
    /// </summary>
    void Select();

    /// <summary>
    /// Wordt uitgevoerd wanneer de muis op dit object staat.
    /// </summary>
    void Hover();

    /// <summary>
    /// Wordt uitgevoerd wanneer de speler een ander object selecteerd met de muis.
    /// </summary>
    void Exit();
}
