using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DB;

public class BattleItemManager : MonoBehaviour
{
    public ItemCollection itemCollection;
    [SerializeField] private List<Item> droppedItems = new List<Item>();

    public event Action ItemPickUp;

    private void Awake()
    {
       
    }
    private void OnEnable()
    {

    }
    public void OnDroppedEquipment(string id)
    {
        var item = new Item(id);
        item = RandomStat(item);

        droppedItems.Add(item);
    }

    public void OnPickupItem()
    {
        var inventory = GameDataManager.instance.playerInfo.items;
        foreach (Item item in droppedItems)
        {
            if(item.Params.Type == ItemType.Usable || item.Params.Type == ItemType.Material || item.Params.Type == ItemType.Crystal)
            {
                bool hasItem = false;
                foreach(Item _item in inventory)
                {
                    if (item.Params.Id == _item.Params.Id)
                    {
                        _item.Count++;
                        hasItem = true;
                        break;
                    } 
                }
                if(!hasItem)
                    inventory.Add(item);
            }
            else
            {
                inventory.Add(item);
            }
        }
        droppedItems.Clear();
        GameDataManager.instance.UpdateItem();
    }

       
    public Item RandomStat(Item item)
    {
        if(item.IsEquipment)
        {
            var statData = GameDataManager.instance.equipmentStatData[item.Params.Index];
            item.Stats = new List<Stat> { new Stat (statData.StatId1, UnityEngine.Random.Range(statData.StatValueMin1, statData.StatValueMax1)),
                                                    new Stat (statData.StatId2, UnityEngine.Random.Range(statData.StatValueMin2, statData.StatValueMax2)),
                                                    new Stat (statData.StatId3, UnityEngine.Random.Range(statData.StatValueMin3, statData.StatValueMax3)),
            };
        }
        return item;
    }
}
