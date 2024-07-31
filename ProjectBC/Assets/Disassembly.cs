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

    [SerializeField]
    private List<Item> selectedItems = new List<Item>();

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
            Debug.LogError($"Toggle {toggle.name} is not mapped to any rarity.");
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
            if (selectedRarities.Contains(item.Params.Rarity))
            {
                selectedItems.Add(item);
            }
        }

        Debug.Log($"Updated selected items count: {selectedItems.Count}");

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
        // 선택된 아이템들의 복사본을 만듭니다. 
        // 이는 리스트를 순회하면서 항목을 제거할 때 발생할 수 있는 문제를 방지합니다.
        List<Item> itemsToRemove = new List<Item>(selectedItems);

        foreach (Item item in itemsToRemove)
        {
            // 인벤토리에서 아이템 제거
            GameDataManager.instance.playerInfo.items.Remove(item);

            // 여기에 아이템 분해에 대한 추가 로직을 구현할 수 있습니다.
            // 예: 재료 획득, 경험치 획득 등
        }

        // selectedItems 리스트 비우기
        selectedItems.Clear();

        // UI 업데이트
        UpdateUI();

        // GameDataManager에 아이템 업데이트를 알림
        GameDataManager.instance.UpdateItem();

        Debug.Log($"Disassembled {itemsToRemove.Count} items.");
    }
}