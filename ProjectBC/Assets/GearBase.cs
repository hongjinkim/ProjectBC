using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class GearBase : ItemWorkspace
{

    public Equipment Equipment;
    //public ScrollInventory PlayerInventory;

    public bool InitializeExample;
    // These callbacks can be used outside;
    public Action<Item> OnRefresh;
    public Action<Item> OnEquip;
    public Func<Item, bool> CanEquip = i => true;
    public void Awake()
    {

    }
    public void Start()
    {
    }
    /// <summary>
    /// Initialize owned items (just for example).
    /// </summary>
    /// 
    public void InitializeInventory()
    {
        var inventory = GameDataManager.instance.playerInfo.items; // inventory.Clear();

    }
    public void Initialize(ScrollEquipment container, ref List<Item> inventory)
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
        if (item != null)
        {
            int index = GameDataManager.instance.playerInfo.items.IndexOf(item);
            ItemInfo.Initialize(item, index);
        }
        Refresh();
    }
    public void Equip()
    {
        if (!CanEquip(SelectedItem)) return;
        var equipped = SelectedItem.IsFirearm
            ? Equipment.Items.Where(i => i.IsFirearm).ToList()
            : Equipment.Items.Where(i => i.Params.Type == SelectedItem.Params.Type && !i.IsFirearm).ToList();
        //if (equipped.Any())
        //{
        //    AutoRemove(equipped, Equipment.slots.Count(i => i.Supports(SelectedItem)));
        //}
        //if (SelectedItem.IsTwoHanded) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
        //if (SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());
        //if (SelectedItem.IsFirearm) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
        //if (SelectedItem.IsFirearm) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());
        //if (SelectedItem.IsTwoHanded || SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsFirearm).ToList());
        OnEquip?.Invoke(SelectedItem);
    }
    public void Remove()
    {
        SelectItem(SelectedItem);
    }
    public override void Refresh()
    {
        OnRefresh?.Invoke(SelectedItem);
    }

    public InventoryBase inventoryBase;
    public void SelectPreviousItem()
    {
        List<Item> currentItems = inventoryBase.inventoryItems[inventoryBase.currentInventoryType];
        if (currentItems.Count == 0)
        {
            Debug.Log("Current inventory is empty.");
            return;
        }

        int currentIndex = SelectedItem != null ? currentItems.IndexOf(SelectedItem) : -1;
        if (currentIndex == -1)
        {
            // 현재 선택된 아이템이 없거나 현재 인벤토리에 없는 경우, 마지막 아이템 선택
            SelectItem(currentItems[currentItems.Count - 1]);
        }
        else
        {
            int previousIndex = (currentIndex - 1 + currentItems.Count) % currentItems.Count;
            SelectItem(currentItems[previousIndex]);
        }
    }
    public void SelectNextItem()
    {
        List<Item> currentItems = inventoryBase.inventoryItems[inventoryBase.currentInventoryType];
        if (currentItems.Count == 0)
        {
            Debug.Log("Current inventory is empty.");
            return;
        }

        int currentIndex = SelectedItem != null ? currentItems.IndexOf(SelectedItem) : -1;
        if (currentIndex == -1)
        {
            // 현재 선택된 아이템이 없거나 현재 인벤토리에 없는 경우, 첫 번째 아이템 선택
            SelectItem(currentItems[0]);
        }
        else
        {
            int nextIndex = (currentIndex + 1) % currentItems.Count;
            SelectItem(currentItems[nextIndex]);
        }
    }
}