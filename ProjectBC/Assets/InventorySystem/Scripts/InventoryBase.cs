using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class InventoryBase : ItemWorkspace
{
    public ScrollInventory PlayerInventory;
    public bool InitializeExample;

    // These callbacks can be used outside;
    public Action<Item> OnRefresh;
    public Action<Item> OnEquip;
    public Func<Item, bool> CanEquip = i => true;

    public void Awake()
    {
        ItemCollection.active = ItemCollection;
    }

    public void Start()
    {
        if (InitializeExample)
        {
            TestInitialize();
        }
    }

    /// <summary>
    /// Initialize owned items (just for example).
    /// </summary>
    public void TestInitialize()
    {
        var inventory = ItemCollection.active.items.Select(i => new Item(i.Id)).ToList(); // inventory.Clear();
        var equipped = new List<Item>();

        Initialize(ref inventory, ref equipped, 6, null);
    }

    public void Initialize(ref List<Item> inventory, ref List<Item> equipped, int bagSize, Action onRefresh)
    {
        RegisterCallbacks();
        PlayerInventory.Initialize(ref inventory);

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
        ItemInfo.Initialize(SelectedItem, SelectedItem.Params.Price);
        Refresh();
    }




    public Item Assemble()
    {
        if (SelectedItem != null && SelectedItem.Params.Type == ItemType.Fragment && SelectedItem.Count >= SelectedItem.Params.FindProperty(PropertyId.Fragments).valueInt)
        {
            SelectedItem.Count -= SelectedItem.Params.FindProperty(PropertyId.Fragments).valueInt;

            var crafted = new Item(SelectedItem.Params.FindProperty(PropertyId.Craft).value);
            var existed = PlayerInventory.Items.SingleOrDefault(i => i.Hash == crafted.Hash);

            if (existed == null)
            {
                PlayerInventory.Items.Add(crafted);
            }
            else
            {
                existed.Count++;
            }

            if (SelectedItem.Count == 0)
            {
                PlayerInventory.Items.Remove(SelectedItem);
                SelectedItem = crafted;
            }

            PlayerInventory.Refresh(SelectedItem);

            return crafted;
        }

        return null;
    }

    private List<Item> MaterialList => SelectedItem.Params.FindProperty(PropertyId.Materials).value.Split(',').Select(i => i.Split(':')).Select(i => new Item(i[0], int.Parse(i[1]))).ToList();

    private bool CanUse()
    {
        switch (SelectedItem.Params.Type)
        {
            case ItemType.Container:
            case ItemType.Booster:
            case ItemType.Coupon:
                return true;
            default:
                return false;
        }
    }

    private bool CanCraft(List<Item> materials)
    {
        return materials.All(i => PlayerInventory.Items.Any(j => j.Hash == i.Hash && j.Count >= i.Count));
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