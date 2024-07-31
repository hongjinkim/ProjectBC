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
        selectedItems.Clear(); // ���� ����Ʈ�� ���� ����
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
    // �ʿ��� ��� selectedItems�� �����ϴ� �޼���
    public List<Item> GetSelectedItems()
    {
        return new List<Item>(selectedItems); // ���纻 ��ȯ
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
            disassemblyText.text = "�ϰ� ����";
            isPopup = false;
        }
    }

    public void ItemDisassembly()
    {
        // ���õ� �����۵��� ���纻�� ����ϴ�. 
        // �̴� ����Ʈ�� ��ȸ�ϸ鼭 �׸��� ������ �� �߻��� �� �ִ� ������ �����մϴ�.
        List<Item> itemsToRemove = new List<Item>(selectedItems);

        foreach (Item item in itemsToRemove)
        {
            // �κ��丮���� ������ ����
            GameDataManager.instance.playerInfo.items.Remove(item);

            // ���⿡ ������ ���ؿ� ���� �߰� ������ ������ �� �ֽ��ϴ�.
            // ��: ��� ȹ��, ����ġ ȹ�� ��
        }

        // selectedItems ����Ʈ ����
        selectedItems.Clear();

        // UI ������Ʈ
        UpdateUI();

        // GameDataManager�� ������ ������Ʈ�� �˸�
        GameDataManager.instance.UpdateItem();

        Debug.Log($"Disassembled {itemsToRemove.Count} items.");
    }
}