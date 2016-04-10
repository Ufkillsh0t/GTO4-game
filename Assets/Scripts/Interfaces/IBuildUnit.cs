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
    bool Attack(Tile t);

    /// <summary>
    /// Verdedigt zich tegen een aanval van een tegenstander.
    /// </summary>
    /// <returns>Of de verdediging is gelukt of niet</returns>
    bool Defend(int damage);

    /// <summary>
    /// Moves an object to another tile.
    /// </summary>
    /// <param name="t">Moves the object to the given tile.</param>
    /// <returns>If the object has been moved or not</returns>
    bool Move(Tile t);

    /// <summary>
    /// Checks if this object can move or not;
    /// </summary>
    /// <returns>Wheter the current object can move or not</returns>
    bool CanMove();

    /// <summary>
    /// Checks wether the unit is moving or not.
    /// </summary>
    /// <returns>If the unit is moving</returns>
    bool IsMoving();

    /// <summary>
    /// Checks if this object can attack another object or not.
    /// </summary>
    /// <returns>Wheter the current object can attack or not</returns>
    bool CanAttack();

    /// <summary>
    /// Upgrade of verbeterd de huidige waarde van het IBuildUnit object.
    /// </summary>
    /// <returns>Of de upgrade is gelukt of niet</returns>
    bool Upgrade();

    /// <summary>
    /// Returns if the tile on which this buildunit is standing is hightlighted.
    /// </summary>
    /// <returns></returns>
    bool TileHighlighted();

    /// <summary>
    /// Gets the tile of the buildunit;
    /// </summary>
    /// <returns></returns>
    Tile getTile();

    /// <summary>
    /// Wordt uitgevoerd wanneer dit object is gespawnt, hierdoor wordt er een tile en speler meegegeven aan dit object.
    /// </summary>
    /// <param name="t">De tile waar dit object op is gespawnt</param>
    /// <param name="p">De speler van wie dit object is.</param>
    void OnSpawn(Tile t, Player p);

    /// <summary>
    /// Geeft de building een kleur;
    /// </summary>
    /// <param name="col"></param>
    void ColorObject(BuildUnitColor col);

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

    /// <summary>
    /// Wordt uitgevoerd indien deze buildunit niet van de speler is.
    /// </summary>
    void Blocked();
}
