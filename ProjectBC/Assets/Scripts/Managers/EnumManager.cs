using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacteristicType
{
    MuscularStrength,
    Agility,
    Intellect
}
public enum JobType
{
    warrior,
    archer,
    wizard,
    priest
}
public enum Rarity
{
    Common = 1 ,
    Uncommon = 2,
    Rare = 3,
    Unique = 4 ,
    Epic = 5
}

public enum ItemType
{
    Equipable,
    Usable,
    Material,
    Crystal
}

public enum TraitType
{
    Concentration, // 집중
    Plunder,       // 약탈
    Magic,         // 요술
    Protection,    // 보호
    Life,          // 생명
    Explosion      // 폭발
}