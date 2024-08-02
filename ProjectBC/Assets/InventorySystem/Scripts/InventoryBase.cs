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

    [Header("Disassembly")]
    public GameObject DisassemblyPopup;
    public Disassembly disassembly;
    //[Header("Sort Equipments by Type")]
    //public ScrollInventory WeaponInventory;
    //public ScrollInventory HelemtInventory;
    //public ScrollInventory ArmorInventory;
    //public ScrollInventory BootsInventory;

    public List<ScrollInventory> inventories;

    public enum InventoryType { Equipment, Usable, Material, Crystal }
    public InventoryType currentInventoryType = InventoryType.Equipment;
    public Dictionary<InventoryType, List<Item>> inventoryItems;

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
        EventManager.StopListening(EventType.ItemUpdated, InitializeInventory);
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
        EventManager.StartListening(EventType.ItemUpdated, InitializeInventory);
        //GameDataManager.ItemUpdated += InitializeInventory;
        ItemCollection.active = ItemCollection;
        SetupInventories();
        inventoryItems = new Dictionary<InventoryType, List<Item>>();
        foreach (InventoryType type in Enum.GetValues(typeof(InventoryType)))
        {
            inventoryItems[type] = new List<Item>();
        }
    }

    public void Start()
    {
       InitializeInventory(null);
    }

    /// <summary>
    /// Initialize owned items (just for example).
    /// </summary>
    public void InitializeInventory(Dictionary<string, object> message)
    {
        var allItems = GameDataManager.instance.playerInfo.items;

        // 기존 아이템 리스트 초기화
        foreach (var list in inventoryItems.Values)
        {
            list.Clear();
        }

        foreach (Item item in allItems)
        {
            switch (item.Params.Type)
            {
                case ItemType.Usable:
                case ItemType.Exp:
                    inventoryItems[InventoryType.Usable].Add(item);
                    break;
                case ItemType.Material:
                    inventoryItems[InventoryType.Material].Add(item);
                    break;
                case ItemType.Crystal:
                    inventoryItems[InventoryType.Crystal].Add(item);
                    break;
                default:
                    inventoryItems[InventoryType.Equipment].Add(item);
                    break;
            }
        }

        Initialize(EquipmentInventory, inventoryItems[InventoryType.Equipment]);
        Initialize(UsableInventory, inventoryItems[InventoryType.Usable]);
        Initialize(MaterialInventory, inventoryItems[InventoryType.Material]);
        Initialize(CrystalInventory, inventoryItems[InventoryType.Crystal]);
    }

    public void Initialize(ScrollInventory container, List<Item> inventory)
    {
        RegisterCallbacks();
        container.Initialize(ref inventory);
    }
    private void SwitchInventoryType(InventoryType newType)
    {
        currentInventoryType = newType;
        ShowInventory(GetInventoryByType(newType));
        SelectedItem = null;
        if (ItemInfo != null)
        {
            ItemInfo.Reset();
        }
        Refresh();
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
        if (item != null)
        {
            int index = GameDataManager.instance.playerInfo.items.IndexOf(item);
            ItemInfo.Initialize(SelectedItem, index);
        }
        Refresh();
    }

    public void OnEquipmentButtonClicked()
    {
        SwitchInventoryType(InventoryType.Equipment);
    }

    public void OnUsableButtonClicked()
    {
        SwitchInventoryType(InventoryType.Usable);
    }

    public void OnMaterialButtonClicked()
    {
        SwitchInventoryType(InventoryType.Material);
    }

    public void OnCrystalButtonClicked()
    {
        SwitchInventoryType(InventoryType.Crystal);
    }

    public void OnDisassemblyButtonClicked()
    {
        if (DisassemblyPopup != null && disassembly.isPopup == false)
        {
            DisassemblyPopup.SetActive(true);

            disassembly.disassemblyText.text = "분해하기";
        }
    }



    public Item Assemble()
    {
        //if (SelectedItem != null && SelectedItem.Params.Type == ItemType.Fragment && SelectedItem.Count >= SelectedItem.Params.FindProperty(BasicStat.Fragments).valueInt)
        //{
        //    SelectedItem.Count -= SelectedItem.Params.FindProperty(BasicStat.Fragments).valueInt;

        //    var crafted = new Item(SelectedItem.Params.FindProperty(BasicStat.Craft).value);
        //    var existed = EquipmentInventory.Items.SingleOrDefault(i => i.Hash == crafted.Hash);

        //    if (existed == null)
        //    {
        //        EquipmentInventory.Items.Add(crafted);
        //    }
        //    else
        //    {
        //        existed.Count++;
        //    }

        //    if (SelectedItem.Count == 0)
        //    {
        //        EquipmentInventory.Items.Remove(SelectedItem);
        //        SelectedItem = crafted;
        //    }

        //    EquipmentInventory.Refresh(SelectedItem);

        //    return crafted;
        //}

        return null;
    }

    //private List<Item> MaterialList => SelectedItem.Params.FindProperty(BasicStat.Materials).value.Split(',').Select(i => i.Split(':')).Select(i => new Item(i[0], int.Parse(i[1]))).ToList();


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

    private ScrollInventory GetInventoryByType(InventoryType type)
    {
        switch (type)
        {
            case InventoryType.Equipment:
                return EquipmentInventory;
            case InventoryType.Usable:
                return UsableInventory;
            case InventoryType.Material:
                return MaterialInventory;
            case InventoryType.Crystal:
                return CrystalInventory;
            default:
                return null;
        }
    }
}