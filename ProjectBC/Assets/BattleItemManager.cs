using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DB;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

[Serializable]
public class DropItem
{
    public string id;
    public float dropRate;
    public int value;
    public GameObject droppedItemPrefab;

    public DropItem(string id, float dropRate)
    {
        this.id = id;
        this.dropRate = dropRate;
    }
}
public class BattleItemManager : MonoBehaviour
{

    public List<DropItem> DropItems = new List<DropItem>();

    public Sprite goldSprite;

    [SerializeField] private List<Item> droppedItems = new List<Item>();
    [SerializeField] private List<GameObject> droppedPrefabs = new List<GameObject>();
    [SerializeField] private int droppedGolds = 0;

    public event Action ItemPickUp;

    [SerializeField] private float totalDropRate = 0;

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
    }

    public void GetDroppedItem(Transform transform)
    {
        if(totalDropRate == 0)
        {
            foreach (DropItem i in DropItems)
            {
                totalDropRate += i.dropRate;
            }
        }
        float randomNumber = Random.value * totalDropRate;
        float cumulativeRate = 0f;
        foreach (DropItem i in DropItems)
        {
            cumulativeRate += i.dropRate;
            if(randomNumber <= cumulativeRate)
            {
                if (i.id == "none")
                    return;
                else if(i.id == "gold")
                {
                    droppedGolds += i.value;

                    SpriteRenderer renderer = i.droppedItemPrefab.GetComponent<SpriteRenderer>();
                    renderer.sprite = goldSprite;
                    // 추후 오브젝트 풇로 변경
                    droppedPrefabs.Add(Instantiate(i.droppedItemPrefab, transform.position, Quaternion.identity));
                }
                else
                {
                    var item = new Item(i.id);
                    item = RandomStat(item);

                    droppedItems.Add(item);

                    SpriteRenderer renderer = i.droppedItemPrefab.GetComponent<SpriteRenderer>();
                    renderer.sprite = ItemCollection.active.GetItemIcon(item).sprite;
                    // 추후 오브젝트 풇로 변경
                    droppedPrefabs.Add(Instantiate(i.droppedItemPrefab, transform.position, Quaternion.identity));
                }
                return;
            }
        }
        return;
    }

    public void OnDroppedEquipment(Item droppedItem)
    {
       
    }

    public void OnPickupItem()
    {
        GameDataManager.instance.playerInfo.gold += droppedGolds;

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

        foreach(GameObject go in droppedPrefabs)
        {
            Destroy(go);
        }

        droppedGolds = 0;
        droppedItems.Clear();
        droppedPrefabs.Clear();
        GameDataManager.instance.UpdateItem();
        GameDataManager.instance.UpdateFunds();
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
