using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


[Serializable]
public class Item 
{
    public string id;
    public Modifier modifier;

    public Stat Stat;
    public int LuckyPercent = 0;
    public int LuckyPoint = 0;

    public int BattlePoint => CalcItemBattlePoint();

    public int Count;
    public int index;


    public bool isLocked;
    public bool isSelected { get; set; }

    public Item()
    {

    }
    public Item(string id, int count = 1)
    {
        this.id = id;
        Count = count;
    }

    public Item(string id, Modifier modifier, int count = 1)
    {
        this.id = id;
        Count = count;
        this.modifier = modifier;
    }

    public Item Clone()
    {
        return new Item(id, modifier, Count);
    }

    private int CalcItemBattlePoint()
    {
        int result = 0;


        // 추후 스탯 종류 별로 다르게 적용 필요
        foreach(Basic basic in Stat.basic)
        {
            result += basic.value;
        }
        foreach (Magic magic in Stat.magic)
        {
            result += magic.value;
        }

        return result;
    }

    [JsonIgnore] public ItemParams Params => ItemCollection.active.GetItemParams(this);
    [JsonIgnore] public ItemSprite Sprite => ItemCollection.active.GetItemSprite(this);
    [JsonIgnore] public ItemIcon Icon => ItemCollection.active.GetItemIcon(this);

    [JsonIgnore] public int Hash => $"{id}.{modifier?.id}.{modifier?.level}".GetHashCode();
    [JsonIgnore] public bool IsModified => modifier != null;
    [JsonIgnore] public bool IsEquipment => Params.Type == ItemType.Helmet || Params.Type == ItemType.Armor || Params.Type == ItemType.Vest || Params.Type == ItemType.Bracers || Params.Type == ItemType.Leggings || Params.Type == ItemType.Weapon || Params.Type == ItemType.Shield;
    [JsonIgnore] public bool IsArmor => Params.Type == ItemType.Helmet || Params.Type == ItemType.Armor || Params.Type == ItemType.Vest || Params.Type == ItemType.Bracers || Params.Type == ItemType.Leggings;
    [JsonIgnore] public bool IsWeapon => Params.Type == ItemType.Weapon;
    [JsonIgnore] public bool IsShield => Params.Type == ItemType.Shield;
    [JsonIgnore] public bool IsDagger => Params.Class == ItemClass.Dagger;
    [JsonIgnore] public bool IsSword => Params.Class == ItemClass.Sword;
    [JsonIgnore] public bool IsAxe => Params.Class == ItemClass.Axe;
    [JsonIgnore] public bool IsPickaxe => Params.Class == ItemClass.Pickaxe;
    [JsonIgnore] public bool IsWand => Params.Class == ItemClass.Wand;
    [JsonIgnore] public bool IsBlunt => Params.Class == ItemClass.Blunt;
    [JsonIgnore] public bool IsLance => Params.Class == ItemClass.Lance;
    [JsonIgnore] public bool IsMelee => Params.Type == ItemType.Weapon && Params.Class != ItemClass.Bow && Params.Class != ItemClass.Firearm;
    [JsonIgnore] public bool IsBow => Params.Class == ItemClass.Bow;
    [JsonIgnore] public bool IsFirearm => Params.Class == ItemClass.Firearm;
    [JsonIgnore] public bool IsOneHanded => !IsTwoHanded;
    [JsonIgnore] public bool IsTwoHanded => Params.Class == ItemClass.Bow || Params.Tags.Contains(ItemTag.TwoHanded);
    [JsonIgnore] public bool IsCanStacked => Params.Type == ItemType.Usable || Params.Type == ItemType.Material || Params.Type == ItemType.Crystal || Params.Type == ItemType.Exp;
}
