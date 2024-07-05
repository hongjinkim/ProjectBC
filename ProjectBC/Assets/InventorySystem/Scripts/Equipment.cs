using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : ItemContainer
{
    public Action onRefresh;

    /// <summary>
    /// Defines what kinds of items can be equipped.
    /// </summary>
    public List<ItemSlot> slots;

    /// <summary>
    /// Equipped items will be instantiated in front of equipment slots.
    /// </summary>
    public GameObject itemPrefab;

    /// <summary>
    /// Character preview.
    /// </summary>
    public CharacterBase preview;

    public Transform scheme;
    public int bagSize;

    public readonly List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public void OnValidate()
    {
        if (Application.isPlaying) return;

        slots = GetComponentsInChildren<ItemSlot>(true).ToList();

        //if (Character == null)
        //{
        //    Character = FindObjectOfType<Character>();
        //}
    }

    public void Start()
    {
        //Character.Animator.SetBool("Ready", false);
    }

    public void SetBagSize(int size)
    {
        bagSize = size;

        var supplies = GetComponentsInChildren<ItemSlot>(true).Where(i => i.types.Contains(ItemType.Supply)).ToList();

        for (var i = 0; i < supplies.Count; i++)
        {
            supplies[i].Locked = i >= size;
        }
    }

    public bool SelectAny()
    {
        if (inventoryItems.Count > 0)
        {
            inventoryItems[0].Select(true);

            return true;
        }

        return false;
    }

    public override void Refresh(Item selected)
    {
        var items = slots.Select(FindItem).Where(i => i != null).ToList();
        var toggleGroup = GetComponentInParent<ToggleGroup>(includeInactive: true);

        Reset();

        foreach (var slot in slots)
        {
            var item = FindItem(slot);

            slot.gameObject.SetActive(item == null);

            if (item == null) continue;

            var inventoryItem = Instantiate(itemPrefab, slot.transform.parent).GetComponent<InventoryItem>();

            inventoryItem.Initialize(item, toggleGroup);
            inventoryItem.count.text = null;
            inventoryItem.transform.position = slot.transform.position;
            inventoryItem.transform.SetSiblingIndex(slot.transform.GetSiblingIndex());

            if (AutoSelect) inventoryItem.Select(item == selected);

            inventoryItems.Add(inventoryItem);
        }

        if (preview)
        {
            CharacterInventorySetup.Setup(preview, items);
            preview.Initialize();
        }

        onRefresh?.Invoke();
    }

    private void Reset()
    {
        foreach (var inventoryItem in inventoryItems)
        {
            Destroy(inventoryItem.gameObject);
        }

        inventoryItems.Clear();
    }

    private Item FindItem(ItemSlot slot)
    {
        if (slot.types.Contains(ItemType.Shield))
        {
            var copy = Items.SingleOrDefault(i => i.Params.Type == ItemType.Weapon && (i.IsTwoHanded || i.IsFirearm));

            if (copy != null)
            {
                return copy;
            }
        }

        var index = slots.Where(i => i.types.SequenceEqual(slot.types)).ToList().IndexOf(slot);
        var items = Items.Where(slot.Supports).ToList();

        return index < items.Count ? items[index] : null;
    }
}
