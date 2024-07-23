using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


public class EquipmentBase : ItemWorkspace
{
    [Header("Sort Equipments by Type")]
    public ScrollInventory WeaponInventory;
    public ScrollInventory HelmetInventory;
    public ScrollInventory ArmorInventory;
    public ScrollInventory LeggingsInventory;

    // These callbacks can be used outside;
    public Action<Item> OnRefresh;
    public Action<Item> OnEquip;
    public Func<Item, bool> CanEquip = i => true;


    void OnEnable()
    {
        transform.SetAsFirstSibling();
    }

    void OnApplicationQuit()
    {
        GameDataManager.ItemUpdated -= InitializeInventory;
    }

    public void Awake()
    {
        GameDataManager.ItemUpdated += InitializeInventory;
        ItemCollection.active = ItemCollection;
    }

    public void Start()
    {
        InitializeInventory();
    }

    /// <summary>
    /// Initialize owned items (just for example).
    /// </summary>
    public void InitializeInventory()
    {
        var inventory = GameDataManager.instance.playerInfo.items; // inventory.Clear();

        var weapon = new List<Item>();
        var helmet = new List<Item>();
        var armor = new List<Item>();
        var boots = new List<Item>();

        foreach (Item item in inventory)
        {
            switch (item.Params.Type)
            {
                case ItemType.Weapon:
                    weapon.Add(item);
                    break;
                case ItemType.Helmet:
                    helmet.Add(item);
                    break;
                case ItemType.Armor:
                    armor.Add(item);
                    break;
                case ItemType.Boots:
                    boots.Add(item);
                    break;
                default:
                    break;
            }
        }


        Initialize(WeaponInventory, ref weapon);
        Initialize(HelmetInventory, ref helmet);
        Initialize(ArmorInventory, ref armor);
        Initialize(LeggingsInventory, ref boots);

    }

    public void Initialize(ScrollInventory container, ref List<Item> inventory/*, ref List<Item> equipped, int bagSize, Action onRefresh*/)
    {
        RegisterCallbacks();
        container.Initialize(ref inventory);
    }

    public void RegisterCallbacks()
    {
        InventoryItem.OnLeftClick = SelectItem;
    }

    public void SelectItem(Item item)
    {
        SelectedItem = item;
        ItemInfo.Initialize(SelectedItem);
        Refresh();
    }

    public override void Refresh()
    {
        if (SelectedItem == null)
        {
            ItemInfo.Reset();
        }


        OnRefresh?.Invoke(SelectedItem);
    }

    public void ShowWeaponInventory()
    {
        transform.SetAsLastSibling();

        WeaponInventory.transform.SetAsLastSibling();
    }
    public void ShowHelmetInventory()
    {
        transform.SetAsLastSibling();

        HelmetInventory.transform.SetAsLastSibling();
    }
    public void ShowArmorInventory()
    {
        transform.SetAsLastSibling();

        ArmorInventory.transform.SetAsLastSibling();
    }
    public void ShowLeggingsInventory()
    {
        transform.SetAsLastSibling();

        LeggingsInventory.transform.SetAsLastSibling();
    }
}