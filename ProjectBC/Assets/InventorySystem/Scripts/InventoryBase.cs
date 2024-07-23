using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


public class InventoryBase : ItemWorkspace
{

    [Header("inventories")]
    public ScrollInventory EquipmentInventory;
    public ScrollInventory UsableInventory;
    public ScrollInventory MaterialInventory;
    public ScrollInventory CrystalInventory;

    [Header("Sort Equipments by Type")]
    public ScrollInventory WeaponInventory;
    public ScrollInventory HelemtInventory;
    public ScrollInventory ArmorInventory;
    public ScrollInventory BootsInventory;

    public List<ScrollInventory> inventories;

    public bool InitializeExample;

    // These callbacks can be used outside;
    public Action<Item> OnRefresh;
    public Action<Item> OnEquip;
    public Func<Item, bool> CanEquip = i => true;


    void OnEnable()
    {
        ShowInventory(EquipmentInventory);
    }

    void OnApplicationQuit()
    {
        GameDataManager.ItemUpdated -= InitializeInventory;
    }

    


    void SetupInventories()
    {
        if (EquipmentInventory != null)
            inventories.Add(EquipmentInventory);
        if (UsableInventory != null)
            inventories.Add(UsableInventory);
        if (MaterialInventory != null)
            inventories.Add(MaterialInventory);
        if (CrystalInventory != null)
            inventories.Add(CrystalInventory);
    }

    void ShowInventory(ScrollInventory inventory)
    {
        foreach (ScrollInventory inven in inventories)
        {
            if (inven == inventory)
            {
                inven?.ShowContainer();
            }
            else
            {
                inven?.HideContainer();
            }
        }
    }
    public void Awake()
    {
        GameDataManager.ItemUpdated += InitializeInventory;
        ItemCollection.active = ItemCollection;
        SetupInventories();
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

        var equipment = new List<Item>();
        var usable = new List<Item>();
        var material = new List<Item>();
        var crystal = new List<Item>();

        var weapon = new List<Item>();
        var helmet = new List<Item>();
        var armor = new List<Item>();
        var boots = new List<Item>();

        foreach (Item item in inventory)
        {
            switch(item.Params.Type)
            {
                case ItemType.Container:
                case ItemType.Booster:
                case ItemType.Coupon:
                case ItemType.Usable:
                    usable.Add(item);
                    break;
                case ItemType.Material:
                    material.Add(item);
                    break;
                case ItemType.Crystal:
                    crystal.Add(item);
                    break;
                case ItemType.Weapon:
                    equipment.Add(item);
                    weapon.Add(item);
                    break;
                case ItemType.Helmet:
                    equipment.Add(item);
                    helmet.Add(item);
                    break;
                case ItemType.Armor:
                    equipment.Add(item);
                    armor.Add(item);
                    break;
                case ItemType.Boots:
                    equipment.Add(item);
                    boots.Add(item);
                    break;
                default:
                    equipment.Add(item);
                    break;
            }
        }

        Initialize(EquipmentInventory, ref equipment);
        Initialize(UsableInventory, ref usable);
        Initialize(MaterialInventory, ref material);
        Initialize(CrystalInventory, ref crystal);



    }

    public void Initialize(ScrollInventory container, ref List<Item> inventory/*, ref List<Item> equipped, int bagSize, Action onRefresh*/)
    {
        RegisterCallbacks();
        container.Initialize(ref inventory);
    }


    public void RegisterCallbacks()
    {
        InventoryItem.OnLeftClick = SelectItem;
        InventoryItem.OnRightClick = InventoryItem.OnDoubleClick = QuickAction;
    }

    private void QuickAction(Item item)
    {
        SelectItem(item);
    }

    public void SelectItem(Item item)
    {
        SelectedItem = item;
        ItemInfo.Initialize(SelectedItem);
        Refresh();
    }

    public void OnEquipmentButtonClicked()
    {
        ShowInventory(EquipmentInventory);
    }
    public void OnUsableButtonClicked()
    {
        ShowInventory(UsableInventory);
    }
    public void OnMaterialButtonClicked()
    {
        ShowInventory(MaterialInventory);
    }
    public void OnCrystalButtonClicked()
    {
        ShowInventory(CrystalInventory);
    }



    public Item Assemble()
    {
        if (SelectedItem != null && SelectedItem.Params.Type == ItemType.Fragment && SelectedItem.Count >= SelectedItem.Params.FindProperty(PropertyId.Fragments).valueInt)
        {
            SelectedItem.Count -= SelectedItem.Params.FindProperty(PropertyId.Fragments).valueInt;

            var crafted = new Item(SelectedItem.Params.FindProperty(PropertyId.Craft).value);
            var existed = EquipmentInventory.Items.SingleOrDefault(i => i.Hash == crafted.Hash);

            if (existed == null)
            {
                EquipmentInventory.Items.Add(crafted);
            }
            else
            {
                existed.Count++;
            }

            if (SelectedItem.Count == 0)
            {
                EquipmentInventory.Items.Remove(SelectedItem);
                SelectedItem = crafted;
            }

            EquipmentInventory.Refresh(SelectedItem);

            return crafted;
        }

        return null;
    }

    private List<Item> MaterialList => SelectedItem.Params.FindProperty(PropertyId.Materials).value.Split(',').Select(i => i.Split(':')).Select(i => new Item(i[0], int.Parse(i[1]))).ToList();


    private bool CanCraft(List<Item> materials)
    {
        return materials.All(i => EquipmentInventory.Items.Any(j => j.Hash == i.Hash && j.Count >= i.Count));
    }

    public override void Refresh()
    {
        if (SelectedItem == null)
        {
            ItemInfo.Reset();
        }


        OnRefresh?.Invoke(SelectedItem);
    }
}