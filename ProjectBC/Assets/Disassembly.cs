using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Disassembly : MonoBehaviour
{
    [Serializable]
    public class RarityReward
    {
        public ItemRarity rarity;
        public List<string> rewardItemIds;
        public int baseRewardAmount = 1;
        public int rewardAmountPerLevel = 1;
        public int levelInterval = 10;
    }

    [SerializeField]
    private List<RarityReward> rarityRewards = new List<RarityReward>
    {
        new RarityReward { rarity = ItemRarity.Common, rewardItemIds = new List<string> { "Material_Iron" }, baseRewardAmount = 1, rewardAmountPerLevel = 1, levelInterval = 10 },
        new RarityReward { rarity = ItemRarity.Rare, rewardItemIds = new List<string> { "Material_Silver" }, baseRewardAmount = 1, rewardAmountPerLevel = 1, levelInterval = 10 },
        new RarityReward { rarity = ItemRarity.Epic, rewardItemIds = new List<string> { "Material_Gold" }, baseRewardAmount = 1, rewardAmountPerLevel = 1, levelInterval = 10 },
        new RarityReward { rarity = ItemRarity.Legendary, rewardItemIds = new List<string> { "Material_Iron", "Material_Silver", "Material_Gold" }, baseRewardAmount = 1, rewardAmountPerLevel = 1, levelInterval = 10 }
    };

    public Button disassemblyButton;
    public TextMeshProUGUI disassemblyText;
    public InventoryBase inventoryBase;
    public bool isPopup;

    public Toggle[] toggles;
    private Dictionary<Toggle, ItemRarity> toggleRarityMap;
    private List<ItemRarity> selectedRarities = new List<ItemRarity>();

    [SerializeField] private List<Item> selectedItems = new List<Item>();
    [SerializeField] private List<ItemType> allowedTypes = new List<ItemType> { ItemType.Weapon, ItemType.Armor, ItemType.Helmet, ItemType.Leggings };

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EventType.ItemUpdated, UpdateSelectedItems);
    }

    private void Awake()
    {
        InitializeToggleRarityMap();
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener((isOn) => OnToggleValueChanged(toggles[index], isOn));
        }

        EventManager.StartListening(EventType.ItemUpdated, UpdateSelectedItems);
        disassemblyButton.onClick.AddListener(ItemAllDisassemblyButton);
    }

    private void InitializeToggleRarityMap()
    {
        toggleRarityMap = new Dictionary<Toggle, ItemRarity>();
        ItemRarity[] rarities = { ItemRarity.Basic, ItemRarity.Common, ItemRarity.Rare, ItemRarity.Epic, ItemRarity.Legendary };

        for (int i = 0; i < Mathf.Min(toggles.Length, rarities.Length); i++)
        {
            toggleRarityMap[toggles[i]] = rarities[i];
        }
    }

    private void OnToggleValueChanged(Toggle toggle, bool isOn)
    {
        if (!toggleRarityMap.TryGetValue(toggle, out ItemRarity rarity))
        {
            return;
        }

        if (isOn)
        {
            if (!selectedRarities.Contains(rarity))
                selectedRarities.Add(rarity);
        }
        else
        {
            selectedRarities.Remove(rarity);
        }

        UpdateSelectedItems(null);
    }

    public void UpdateSelectedItems(Dictionary<string, object> message)
    {
        selectedItems.Clear();
        var items = GameDataManager.instance.playerInfo.items;

        foreach (var item in items)
        {
            if (selectedRarities.Contains(item.Params.Rarity) && allowedTypes.Contains(item.Params.Type))
            {
                selectedItems.Add(item);
            }
        }

        if (gameObject.activeInHierarchy)
        {
            UpdateUI();
        }
    }
    private void UpdateUI()
    {
        if (GetComponent<Text>() != null)
        {
            GetComponent<Text>().text = $"Selected items: {selectedItems.Count}";
        }
    }
    // ï¿½Ê¿ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿?selectedItemsï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ï´ï¿½ ï¿½Þ¼ï¿½ï¿½ï¿½
    public List<Item> GetSelectedItems()
    {
        return new List<Item>(selectedItems); // ï¿½ï¿½ï¿½çº» ï¿½ï¿½È¯
    }

    public void ItemAllDisassemblyButton()
    {
        if (isPopup == false)
        {
            isPopup = true;
            return;
        }

        if (isPopup)
        {
            ItemDisassembly();

            inventoryBase.DisassemblyPopup.SetActive(false);
            disassemblyText.text = "ï¿½Ï°ï¿½ ï¿½ï¿½ï¿½ï¿½";
            isPopup = false;
        }
    }

    //public void ItemDisassembly()
    //{
    //    List<Item> itemsToRemove = new List<Item>(selectedItems);

    //    foreach (Item item in itemsToRemove)
    //    {
    //        GameDataManager.instance.playerInfo.items.Remove(item);

    //    }
    //    DisassemblyReward();

    //    selectedItems.Clear();
    //    UpdateUI();
    //    GameDataManager.instance.UpdateItem();

    //    inventoryBase.InitializeInventory();
    //}
    public void ItemDisassembly()
    {
        List<Item> itemsToDisassemble = new List<Item>();
        int excludedCount = 0;

        foreach (Item item in selectedItems)
        {
            if (!item.isLocked)
            {
                itemsToDisassemble.Add(item);
            }
            else
            {
                excludedCount++;
            }
        }

        foreach (Item item in itemsToDisassemble)
        {
            GameDataManager.instance.playerInfo.items.Remove(item);
        }

        DisassemblyReward(itemsToDisassemble);  // itemsToDisassemble ¸®½ºÆ® Àü´Þ

        Debug.Log($"Disassembled {itemsToDisassemble.Count} items. {excludedCount} locked items were excluded.");

        selectedItems.Clear();
        UpdateUI();
        //GameDataManager.instance.UpdateItem();
        EventManager.TriggerEvent(EventType.ItemUpdated, null);

        inventoryBase.InitializeInventory(null);
    }

    public void DisassemblyReward(List<Item> disassembledItems)
    {
        int totalGold = 0;
        Dictionary<string, int> rewardItems = new Dictionary<string, int>();

        foreach (Item item in disassembledItems)  // selectedItems ´ë½Å disassembledItems »ç¿ë
        {
            totalGold += item.Params.Price;

            RarityReward reward = rarityRewards.Find(r => r.rarity == item.Params.Rarity);
            if (reward != null)
            {
                int rewardAmount = CalculateRewardAmount(reward, item.Params.Level);
                foreach (string rewardItemId in reward.rewardItemIds)
                {
                    if (!rewardItems.ContainsKey(rewardItemId))
                        rewardItems[rewardItemId] = 0;
                    rewardItems[rewardItemId] += rewardAmount;
                }
            }
        }

        // ï¿½ï¿½ï¿?ï¿½ï¿½ï¿½ï¿½
        GameDataManager.instance.playerInfo.gold += totalGold;

        // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        var inventory = GameDataManager.instance.playerInfo.items;
        foreach (var rewardItem in rewardItems)
        {
            bool itemExists = false;
            foreach (Item item in inventory)
            {
                if (item.Params.Id == rewardItem.Key)
                {
                    item.Count += rewardItem.Value;
                    itemExists = true;
                    break;
                }
            }

            if (!itemExists)
            {
                Item newItem = new Item(rewardItem.Key);
                newItem.Count = rewardItem.Value;
                inventory.Add(newItem);
            }

            Debug.Log($"Gained {rewardItem.Value} of reward item (ID: {rewardItem.Key})");
        }

        Debug.Log($"Gained {totalGold} gold from disassembly");

        //GameDataManager.instance.UpdateFunds();
        //GameDataManager.instance.UpdateItem();
        EventManager.TriggerEvent(EventType.FundsUpdated, null);
        EventManager.TriggerEvent(EventType.ItemUpdated, null);

        inventoryBase.InitializeInventory(null);
    }
    private int CalculateRewardAmount(RarityReward reward, int itemLevel)
    {
        int levelBonus = (itemLevel - 1) / reward.levelInterval;
        return reward.baseRewardAmount + (levelBonus * reward.rewardAmountPerLevel);
    }

    public void SelectedItemDisassembly(Item item)
    {
        if (item.isSelected && !item.isLocked)
        {
            Debug.Log($"ºÐÇØ ½ÃÀÛ: {item.Params.Name}");

            // ¾ÆÀÌÅÛ ºÐÇØ ¹× º¸»ó °è»ê
            int goldReward = item.Params.Price;
            RarityReward reward = rarityRewards.Find(r => r.rarity == item.Params.Rarity);

            if (reward != null)
            {
                int rewardAmount = CalculateRewardAmount(reward, item.Params.Level);

                // °ñµå Áö±Þ
                GameDataManager.instance.playerInfo.gold += goldReward;

                // Àç·á ¾ÆÀÌÅÛ Áö±Þ
                foreach (string rewardItemId in reward.rewardItemIds)
                {
                    AddRewardItem(rewardItemId, rewardAmount);
                }

                // ¾ÆÀÌÅÛ Á¦°Å
                GameDataManager.instance.playerInfo.items.Remove(item);

                Debug.Log($"ºÐÇØ ¿Ï·á: {item.Params.Name}");
                Debug.Log($"È¹µæÇÑ °ñµå: {goldReward}");
                Debug.Log($"È¹µæÇÑ Àç·á: {string.Join(", ", reward.rewardItemIds)} (°¢ {rewardAmount}°³)");

                // ÀÎº¥Åä¸® ¹× UI ¾÷µ¥ÀÌÆ®
                EventManager.TriggerEvent(EventType.FundsUpdated, new Dictionary<string, object> { });
                EventManager.TriggerEvent(EventType.ItemUpdated, new Dictionary<string, object> { });
                inventoryBase.InitializeInventory(null);
                UpdateUI();
            }
            else
            {
                Debug.LogWarning($"ÇØ´ç µî±Þ({item.Params.Rarity})ÀÇ º¸»ó Á¤º¸¸¦ Ã£À» ¼ö ¾ø½À´Ï´Ù.");
            }
        }
        else if (item.isLocked)
        {
            Debug.Log($"{item.Params.Name}Àº(´Â) Àá°ÜÀÖ¾î ºÐÇØÇÒ ¼ö ¾ø½À´Ï´Ù.");
        }
        else
        {
            Debug.Log($"{item.Params.Name}Àº(´Â) ¼±ÅÃµÇÁö ¾Ê¾Ò½À´Ï´Ù.");
        }
    }

    private void AddRewardItem(string itemId, int amount)
    {
        var inventory = GameDataManager.instance.playerInfo.items;
        var existingItem = inventory.Find(i => i.Params.Id == itemId);

        if (existingItem != null)
        {
            existingItem.Count += amount;
        }
        else
        {
            Item newItem = new Item(itemId);
            newItem.Count = amount;
            inventory.Add(newItem);
        }
    }
}