using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DB;

public class BattleItemManager : MonoBehaviour
{
    public Item droppedItem;
    private int idAdjust;

    public event Action<Item> ItemPickUp;

    private void Awake()
    {
        ItemCollection.items = new List<ItemParams>();
        idAdjust = 0;
    }

    public void OnDroppedEquipment(int idx)
    {
        ItemCollection.items.Add(MakeEquipment(GameDataManager.instance._equipmentBaseData[idx]));
    }

    public void OnPickupItem()
    {
        
    }


       
    public ItemParams MakeEquipment(EquipmentBaseData data)
    {
        return new ItemParams
        {
            Id = data.Id + idAdjust++.ToString(),
            Level = data.Level,
            Rarity = data.Rarity,
            Type = data.Type,
            Class = data.Class,
            Tags = new List<ItemTag> { data.Tag1, data.Tag2, data.Tag3 },
            Properties = new List<Property> { new Property(data.PropertyId1, data.PropertyValue1), new Property(data.PropertyId2, data.PropertyValue2) },
            Price = data.Price,
            Weight = data.Weight,
            Material = data.Material,
            IconId = data.IconId,
            SpriteId = data.SpriteId,
            Meta = data.Meta
        };
    }

    public override void Refresh()
    {
        if (Items == null) return;

        List<Item> items;

        if (AutoSorting && SortingFunc != null)
        {
            items = new List<Item>();

            var groups = Items.OrderBy(SortingFunc).ToList().GroupBy(i => i.Params.Type);

            foreach (var group in groups)
            {
                items.AddRange(group.OrderBy(i => i.Params.Class).ThenBy(i => i.Params.Price));
            }
        }
        else
        {
            items = Items.ToList();
        }

        if (FilterFunc != null)
        {
            items.RemoveAll(i => !FilterFunc(i));
        }

        foreach (var instance in _itemInstances)
        {
            instance.Reset();
            instance.SetActive(false);
        }

        var toggleGroup = GetComponentInParent<ToggleGroup>(includeInactive: true);

        for (var i = 0; i < items.Count; i++)
        {
            var instance = GetItemInstance();

            instance.transform.SetSiblingIndex(i);
            instance.Initialize(items[i], toggleGroup);
            instance.count.SetActive(Stacked);

            if (AutoSelect) instance.Select(items[i] == selected);
        }

        var columns = 0;
        var rows = 0;

        switch (Grid.constraint)
        {
            case GridLayoutGroup.Constraint.FixedColumnCount:
                {
                    var height = Mathf.FloorToInt((ScrollRect.GetComponent<RectTransform>().rect.height + Grid.spacing.y) / (Grid.cellSize.y + Grid.spacing.y));

                    columns = Grid.constraintCount;
                    itemCapacity = GameDataManager.instance.playerInfo.itemCapacity;
                    rows = itemCapacity / columns;//Mathf.Max(height, Mathf.FloorToInt((float)items.Count / columns));

                    if (Extend) rows++;

                    break;
                }
            case GridLayoutGroup.Constraint.FixedRowCount:
                {
                    var width = Mathf.FloorToInt((ScrollRect.GetComponent<RectTransform>().rect.width + Grid.spacing.x) / (Grid.cellSize.x + Grid.spacing.x));

                    rows = Grid.constraintCount;
                    columns = Mathf.Max(width, Mathf.FloorToInt((float)items.Count / rows));

                    if (Extend) columns++;

                    break;
                }
        }

        for (var i = items.Count; i < columns * rows; i++)
        {
            var instance = GetItemInstance();

            instance.Initialize(null);
        }

        OnRefresh?.Invoke();
    }
}
