using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

[Serializable]
public class ItemParams
{
    public string Id;
    public int Level;
    public ItemRarity Rarity;
    public ItemType Type;
    public ItemClass Class;
    public List<ItemTag> Tags = new List<ItemTag>();
    public List<Property> Properties = new List<Property>();
    public int Price;
    public int Weight;
    public ItemMaterial Material;
    public string IconId;
    public string SpriteId;
    public string Meta;
}
