using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ItemInfo : PopUp
{
    [Header("Head")]
    public Image iconBackground;
    public Image icon;
    public Image iconFrame;
    public TextMeshProUGUI level;
    public TextMeshProUGUI battlePoint;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI rarity;
    public TextMeshProUGUI type;

    [Header("Basic Stat")]
    public TextMeshProUGUI luckyPoint;
    public List<TextMeshProUGUI> basicStats;
    public List<TextMeshProUGUI> magicStats;


    [Header("Lock")]
    [SerializeField] private Button lockButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    private Item currentItem;
    private List<Item> currentItems;

    [Header("View")]
    public Button nextViewBtn;
    public Button prevViewBtn;

    [Header("Function")]
    public Button disassemblyBtn;
    public Button equipBtn;

    public bool isLocked = false;
    public Item Item { get; protected set; }
    public int currentIndex; // ���� �߰��� �ʵ�

    //public GearBase gearBase;
    public Disassembly disassembly;

    protected override void Start()
    {
        base.Start();
        lockButton.onClick.AddListener(ToggleItemLock);
        nextViewBtn.onClick.AddListener(SelectNextItem);
        prevViewBtn.onClick.AddListener(SelectPreviousItem);
        disassemblyBtn.onClick.AddListener(SelectedDisassemblyButtonClikced);
        equipBtn.onClick.AddListener(EquipButtonClicked);

        UpdateButtonUI();
    }

    public void OnEnable()
    {
        if (Item == null)
        {
            Reset();
        }
    }

    public void Reset()
    {

    }

    public virtual void Initialize(Item item, List<Item> items)
    {
        currentItem = item;
        currentItems = items;

        Item = item;
        currentIndex = items.IndexOf(currentItem);

        ShowScreen();

        icon.sprite = ItemCollection.active.GetItemIcon(item).sprite;
        iconBackground.sprite = ItemCollection.active.GetBackground(item) ?? ItemCollection.active.backgroundBrown;
        iconBackground.color = Color.white;
        iconFrame.raycastTarget = true;
        level.text = "Lv. " + item.Params.Level.ToString("D2");
        itemName.text = item.Params.Name;
        rarity.text = "ǰ�� : " + item.Params.Rarity.ToString();
        type.text = item.Params.Type.ToString();
        battlePoint.text = item.BattlePoint.ToString();
        if (item.IsEquipment)
        {
            luckyPoint.text = "��Ű����Ʈ: " + item.LuckyPoint.ToString() + "(" +item.LuckyPercent.ToString()+ "%)";

            for(int i = 0; i < item.Stat.basic.Count; i++)
            {
                basicStats[i].text = item.Stat.basic[i].value <= 0 ? null : item.Stat.basic[i].id.ToString() + "  " + "+" + item.Stat.basic[i].value.ToString() + "    (+" + (item.Stat.basic[i].value - item.Stat.basic[i].minValue).ToString() + ")";
            }
            for (int i = 0; i < item.Stat.magic.Count; i++)
            {
                magicStats[i].text = item.Stat.magic[i].value <= 0 ? null : item.Stat.magic[i].id.ToString() + "  " + "+" + item.Stat.magic[i].value.ToString();
            }

        }
        else
        {
            // ��� �������� �ƴ� ��� ���� ��ǥ�� �� ���� ǥ��
            luckyPoint.text = null;
            foreach(TextMeshProUGUI text in basicStats)
            {
                text.text = null;
            }
            foreach (TextMeshProUGUI text in magicStats)
            {
                text.text = null;
            }
        }

        UpdateButtonUI();
    }

    public void OnBackButtonClicked()
    {
        HideScreen();
        Item.isSelected = false;
    }

    private void ToggleItemLock()
    {
        if (currentItem != null)
        {
            currentItem.isLocked = !currentItem.isLocked;
            UpdateButtonUI();
            Debug.Log($"Item {currentItem.Params.Name} is now {(currentItem.isLocked ? "locked" : "unlocked")}");
        }
    }

    private void UpdateButtonUI()
    {
        if (currentItem != null)
        {
            buttonText.text = currentItem.isLocked ? "Unlock" : "Lock";
        }
    }

    private void SelectedDisassemblyButtonClikced()
    {
        disassembly.SelectedItemDisassembly(Item);
        OnBackButtonClicked();
    }

    private void EquipButtonClicked()
    {
        Debug.Log("����");
    }

    public void SelectPreviousItem()
    {
        if (currentItems.Count == 0)
        {
            Debug.Log("Current inventory is empty.");
            return;
        }
        if (currentIndex == -1)
        {
            // ���� ���õ� �������� ���ų� ���� �κ��丮�� ���� ���, ������ ������ ����
            Initialize(currentItems[currentItems.Count - 1], currentItems);
        }
        else
        {
            int previousIndex = (currentIndex - 1 + currentItems.Count) % currentItems.Count;
            Initialize(currentItems[previousIndex], currentItems);
        }
    }
    public void SelectNextItem()
    {
        if (currentItems.Count == 0)
        {
            Debug.Log("Current inventory is empty.");
            return;
        }
        if (currentIndex == -1)
        {
            // ���� ���õ� �������� ���ų� ���� �κ��丮�� ���� ���, ù ��° ������ ����
            Initialize(currentItems[0], currentItems);
        }
        else
        {
            int nextIndex = (currentIndex + 1) % currentItems.Count;
            Initialize(currentItems[nextIndex], currentItems);
        }
    }
}