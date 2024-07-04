using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour
{
    public List<Item> Items { get; protected set; } = new List<Item>();

    [Header("Settings")]
    [Tooltip("Stack identical inventory items to a single UI element.")]
    public bool Stacked = true;
    public bool AutoSelect = true;

    public abstract void Refresh(Item selected);

    public void Initialize(ref List<Item> items, Item selected = null)
    {
        Items = items;
        Refresh(selected);
    }
}
