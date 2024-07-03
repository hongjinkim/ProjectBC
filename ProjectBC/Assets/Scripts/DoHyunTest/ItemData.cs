using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public int idx { get; set; }
    public string itemName { get; set; }
    public Rarity itemRarity { get; set; }
    public string itemDescription { get; set; }
    public ItemType itemType { get; set; }
    public bool canStack { get; set; }
}
[System.Serializable]
public class Equipable : IItem
{
    public int idx { get; set; }
    public string itemName { get; set; }
    public Rarity itemRarity { get; set; }
    public string itemDescription { get; set; }
    public ItemType itemType { get; set; }
    public bool canStack { get; set; }
    public Equipable(int idx, string itemName, Rarity itemRarity, string itemDescription)
    {
        this.idx = idx;
        this.itemName = itemName;
        this.itemRarity = itemRarity;
        this.itemDescription = itemDescription;
        this.itemType = ItemType.Equipable;
        this.canStack = false;
    }
}
[System.Serializable]
public class Usable : IItem
{
    public int idx { get; set; }
    public string itemName { get; set; }
    public Rarity itemRarity { get; set; }
    public string itemDescription { get; set; }
    public ItemType itemType { get; set; }
    public bool canStack { get; set; }

    public Usable(int idx, string itemName, Rarity itemRarity, string itemDescription)
    {
        this.idx = idx;
        this.itemName = itemName;
        this.itemRarity = itemRarity;
        this.itemDescription = itemDescription;
        this.itemType = ItemType.Usable;
        this.canStack = false;
    }
}
[System.Serializable]
public class Material : IItem
{
    public int idx { get; set; }
    public string itemName { get; set; }
    public Rarity itemRarity { get; set; }
    public string itemDescription { get; set; }
    public ItemType itemType { get; set; }
    public bool canStack { get; set; }

    public Material(int idx, string itemName, Rarity itemRarity, string itemDescription)
    {
        this.idx = idx;
        this.itemName = itemName;
        this.itemRarity = itemRarity;
        this.itemDescription = itemDescription;
        this.itemType = ItemType.Material;
        this.canStack = false;
    }
}
[System.Serializable]
public class Crystal : IItem
{
    public int idx { get; set; }
    public string itemName { get; set; }
    public Rarity itemRarity { get; set; }
    public string itemDescription { get; set; }
    public ItemType itemType { get; set; }
    public bool canStack { get; set; }
    public Crystal(int idx, string itemName, Rarity itemRarity, string itemDescription)
    {
        this.idx = idx;
        this.itemName = itemName;
        this.itemRarity = itemRarity;
        this.itemDescription = itemDescription;
        this.itemType = ItemType.Crystal;
        this.canStack = false;
    }
}
