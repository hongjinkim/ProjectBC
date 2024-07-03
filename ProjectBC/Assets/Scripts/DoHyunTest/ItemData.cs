using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public interface IItem
//{
//    int idx { get; set; }
//    string itemName { get; set; }
//    Rarity itemRarity { get; set; }
//    string itemDescription { get; set; }
//}
[System.Serializable]
public class Equipable 
{
    public int idx { get; set; }
    public string itemName { get; set; }
    public Rarity itemRarity { get; set; }
    public string itemDescription { get; set; }

    public Equipable(int idx, string itemName, Rarity itemRarity, string itemDescription)
    {
        this.idx = idx;
        this.itemName = itemName;
        this.itemRarity = itemRarity;
        this.itemDescription = itemDescription;
    }
}
[System.Serializable]
public class Usable 
{
    public int idx { get; set; }
    public string itemName { get; set; }
    public Rarity itemRarity { get; set; }
    public string itemDescription { get; set; }

    public Usable(int idx, string itemName, Rarity itemRarity, string itemDescription)
    {
        this.idx = idx;
        this.itemName = itemName;
        this.itemRarity = itemRarity;
        this.itemDescription = itemDescription;
    }
}
[System.Serializable]
public class Material 
{
    public int idx { get; set; }
    public string itemName { get; set; }
    public Rarity itemRarity { get; set; }
    public string itemDescription { get; set; }

    public Material(int idx, string itemName, Rarity itemRarity, string itemDescription)
    {
        this.idx = idx;
        this.itemName = itemName;
        this.itemRarity = itemRarity;
        this.itemDescription = itemDescription;
    }
}
[System.Serializable]
public class Crystal 
{
    public int idx { get; set; }
    public string itemName { get; set; }
    public Rarity itemRarity { get; set; }
    public string itemDescription { get; set; }

    public Crystal(int idx, string itemName, Rarity itemRarity, string itemDescription)
    {
        this.idx = idx;
        this.itemName = itemName;
        this.itemRarity = itemRarity;
        this.itemDescription = itemDescription;
    }
}
