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

    private void Awake()
    {
        InitializeToggleRarityMap();
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener((isOn) => OnToggleValueChanged(toggles[index], isOn));
        }

        GameDataManager.ItemUpdated += UpdateSelectedItems;
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

        UpdateSelectedItems();
    }

    public void UpdateSelectedItems()
    {
        selectedItems.Clear(); // 기존 리스트를 비우고 재사용
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
    // 필요한 경우 selectedItems에 접근하는 메서드
    public List<Item> GetSelectedItems()
    {
        return new List<Item>(selectedItems); // 복사본 반환
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
            disassemblyText.text = "일괄 분해";
            isPopup = false;
        }
    }

    public void ItemDisassembly()
    {
        List<Item> itemsToRemove = new List<Item>(selectedItems);

        foreach (Item item in itemsToRemove)
        {
            GameDataManager.instance.playerInfo.items.Remove(item);
      
        }
        DisassemblyReward();

        selectedItems.Clear();
        UpdateUI();
        GameDataManager.instance.UpdateItem();

        inventoryBase.InitializeInventory();
    }

    public void DisassemblyReward()
    {
        int totalGold = 0;
        Dictionary<string, int> rewardItems = new Dictionary<string, int>();

        foreach (Item item in selectedItems)
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

        // 골드 지급
        GameDataManager.instance.playerInfo.gold += totalGold;

        // 아이템 지급
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

        GameDataManager.instance.UpdateFunds();
        GameDataManager.instance.UpdateItem();
        inventoryBase.InitializeInventory();
    }
    private int CalculateRewardAmount(RarityReward reward, int itemLevel)
    {
        int levelBonus = (itemLevel - 1) / reward.levelInterval;
        return reward.baseRewardAmount + (levelBonus * reward.rewardAmountPerLevel);
    }
}