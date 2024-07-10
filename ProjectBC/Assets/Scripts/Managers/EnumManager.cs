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
    Fragment,
    Crystal
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
    Accuracy = 0,
    Ammo = 1,
    Antidote = 2,
    Bandage = 3,
    Blocking = 4,
    BlockingFatigue = 5,
    Capacity = 6,
    ChargeSpeed = 7,
    ChargeTimings = 8,
    Craft = 9,
    CriticalChance = 10,
    CriticalDamage = 11,
    CustomPrice = 12,
    Damage = 13,
    Duration = 14,
    Effect = 15,
    Exp = 16,
    Fatigue = 17,
    Gunpowder = 18,
    HealthRecovery = 19,
    HealthRestore = 20,
    HealthMax = 21,
    Immunity = 22,
    Magazine = 23,
    Materials = 24,
    Mechanism = 25,
    Radius = 26,
    Range = 27,
    Reloading = 28,
    Resistance = 29,
    ShopChance = 30,
    SkillUp = 31,
    Speed = 32,
    StaminaRecovery = 33,
    StaminaRestore = 34,
    StaminaMax = 35,
    Shock = 36,
    Contains = 37,
    DamageBonus = 38,
    Multishot = 39,
    Fragments = 40,
    DropChance = 41,
    ExpBonus = 42,
    GoldBonus = 43
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

public enum CharacterState
{
    Idle = 0,
    Ready = 1,
    Walk = 2,
    Run = 3,
    Jump = 4,
    Climb = 5,
    Death = 9,
    ShieldBlock = 10,
    WeaponBlock = 11,
    Evasion = 12,
    Dance = 13
}

public enum WeaponType
{
    Melee1H = 0,
    Melee2H = 1,
    Paired = 2,
    Bow = 3,
    Crossbow = 4,
    Firearm1H = 5,
    Firearm2H = 6,
    Throwable = 7
}

public enum BodyPart
{
    Body,
    Head,
    Hair,
    Ears,
    Eyebrows,
    Eyes,
    Mouth,
    Beard,
    Makeup
}

public enum EquipmentPart
{
    Armor,
    Helmet,
    Vest,
    Bracers,
    Leggings,
    MeleeWeapon1H,
    MeleeWeapon2H,
    Bow,
    Crossbow,
    SecondaryMelee1H,
    SecondaryFirearm1H,
    Shield,
    Earrings,
    Cape,
    Quiver,
    Back,
    Mask,
    Firearm1H,
    Firearm2H,
    Wings
}
