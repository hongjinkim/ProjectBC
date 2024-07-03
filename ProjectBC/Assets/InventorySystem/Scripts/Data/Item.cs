using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Item 
{
    public string id;
    public Modifier modifier;

    public int Count;

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

    [JsonIgnore] public ItemParams Params => ItemCollection.active.GetItemParams(this);
    [JsonIgnore] public ItemSprite Sprite => ItemCollection.active.GetItemSprite(this);
    [JsonIgnore] public ItemIcon Icon => ItemCollection.active.GetItemIcon(this);

    [JsonIgnore] public int Hash => $"{id}.{modifier?.id}.{modifier?.level}".GetHashCode();
    [JsonIgnore] public bool IsModified => modifier != null && modifier.id != ItemModifier.None;
    [JsonIgnore] public bool IsEquipment; //=> Params.Type == ItemType.Helmet || Params.Type == ItemType.Armor || Params.Type == ItemType.Vest || Params.Type == ItemType.Bracers || Params.Type == ItemType.Leggings || Params.Type == ItemType.Weapon || Params.Type == ItemType.Shield;
    //[JsonIgnore] public bool IsArmor => Params.Type == ItemType.Helmet || Params.Type == ItemType.Armor || Params.Type == ItemType.Vest || Params.Type == ItemType.Bracers || Params.Type == ItemType.Leggings;
    //[JsonIgnore] public bool IsWeapon => Params.Type == ItemType.Weapon;
    //[JsonIgnore] public bool IsShield => Params.Type == ItemType.Shield;
}
