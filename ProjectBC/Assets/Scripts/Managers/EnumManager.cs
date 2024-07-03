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

/// <summary>
/// Item Enums
/// </summary>
/// 
public enum ElementId
{
    Physic = 0,
    Magic = 1,
    Fire = 2,
    Ice = 3,
    Lightning = 4,
    Light = 5,
    Darkness = 6,
    Explosive = 7
}
public enum ItemClass
{
    Undefined,
    Dagger,
    Sword,
    Axe,
    Blunt,
    Lance,
    Wand,
    Bow,
    Light,
    Heavy,
    Ring,
    Necklace,
    Food,
    Potion,
    Scroll,
    Bomb,
    Pickaxe,
    Claw,
    Fang,
    Skin,
    Firearm,
    Talisman,
    Wood,
    Ore,
    Alloy,
    Wings,
    Shell,
    Bone,
    Leather,
    Tail,
    Gunpowder
}
public enum ItemMaterial
{
    Unknown,
    Wood,
    Leather,
    Metal,
    Fruit,
    Meat,
    Liquid,
    Soup,
    Gold
}
public enum ItemRarity
{
    Legacy = -2,
    Basic = -1,
    Common = 0,
    Rare = 1,
    Epic = 2,
    Legendary = 3
}
public enum ItemTag
{
    Undefined = 0,
    NotForSale = 1,
    Quest = 2,
    TwoHanded = 3,
    Light = 4,
    Heavy = 5,
    Short = 6,
    Long = 7,
    Christmas = 8,
    Farm = 9,
    NoFragments = 10
}
public enum ItemType
{
    Undefined,
    Currency,
    Loot,
    Material,
    Supply,
    Recipe,
    Weapon,
    Shield,
    Armor,
    Helmet,
    Pauldrons,
    Bracers,
    Gloves,
    Vest,
    Belt,
    Leggings,
    Boots,
    Jewelry,
    Backpack,
    Container,
    Booster,
    Coupon,
    Fragment
}
public enum ItemModifier
{
    None = 0,
    Reinforced = 1, // Increase damage/resistance and STR req.
    Refined = 2, // Increase damage/resistance and DEX req.
    Sharpened = 3, // Critical damage up.
    Lightweight = 4, // Reduce weight.
    Poison = 5, // Add poison damage/resistance.
    Bleeding = 6, // Add bleeding damage/resistance.
    Spread = 7, // Reduced damage in a column.
    Onslaught = 8, // Reduced damage in a line;
    Shieldbreaker = 9, // Ignore shield.
    Fire = 10, // Add fire damage/resistance.
    Ice = 11, // Add ice damage/resistance.
    Lightning = 12, // Add lightning damage/resistance.
    Light = 13, // Add healing ability and halved holy damage/resistance.
    Darkness = 14, // Add darkness damage/resistance.
    Vampiric = 15, // Restore HP after each hit.
    LevelDown = 16,
    LevelUp = 17,
    HealthUp = 18,
    HealthRecovery = 19,
    StaminaUp = 20,
    StaminaRecovery = 21,
    Speed = 22,
    Accuracy = 23,
    Reloading = 24
}
public enum PropertyId // TODO: Set indexes.
{
    Accuracy,
    Ammo,
    Antidote,
    Bandage,
    Blocking,
    BlockingFatigue,
    Capacity,
    ChargeSpeed,
    ChargeTimings,
    Craft,
    CriticalChance,
    CriticalDamage,
    CustomPrice,
    Damage,
    Duration,
    Effect,
    Exp,
    Fatigue,
    Gunpowder,
    HealthRecovery,
    HealthRestore,
    HealthMax,
    Immunity,
    Magazine,
    Materials,
    Mechanism,
    Radius,
    Range,
    Reloading,
    Resistance,
    ShopChance,
    SkillUp,
    Speed,
    StaminaRecovery,
    StaminaRestore,
    StaminaMax,
    Shock,
    Contains,
    DamageBonus,
    Multishot,
    Fragments,
    DropChance,
    ExpBonus,
    GoldBonus
}