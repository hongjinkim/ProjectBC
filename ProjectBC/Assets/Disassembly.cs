using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Disassembly : MonoBehaviour
{
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
        int disassembledCount = 0;
        string rewardItemId = "Material_Iron"; // 지급할 아이템의 ID

        foreach (Item item in selectedItems)
        {
            totalGold += item.Params.Price;
            disassembledCount++;
        }

        // 골드 지급
        GameDataManager.instance.playerInfo.gold += totalGold;

        // 특정 아이템 지급 (던전에서의 아이템 획득 로직과 유사)
        var inventory = GameDataManager.instance.playerInfo.items;
        bool itemExists = false;

        foreach (Item item in inventory)
        {
            if (item.Params.Id == rewardItemId)
            {
                item.Count += disassembledCount;
                itemExists = true;
                break;
            }
        }

        if (!itemExists)
        {
            Item newItem = new Item(rewardItemId);
            newItem.Count = disassembledCount;
            inventory.Add(newItem);
        }

        Debug.Log($"Gained {totalGold} gold from disassembly");
        Debug.Log($"Gained {disassembledCount} of reward item (ID: {rewardItemId})");

        GameDataManager.instance.UpdateFunds();
        GameDataManager.instance.UpdateItem();

        inventoryBase.InitializeInventory();
    }
}