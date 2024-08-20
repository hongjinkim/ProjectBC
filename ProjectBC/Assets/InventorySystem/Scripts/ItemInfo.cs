using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : PopUp
{
    [Header("Head")]
    public Image iconBackground;
    public Image icon;
    public Image iconFrame;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI rarity;
    public TextMeshProUGUI type;
    public TextMeshProUGUI description;

    protected Item currentItem;
    protected List<Item> currentItems;

    [Header("View")]
    public Button nextViewBtn;
    public Button prevViewBtn;
    public Button backButton;

    public Item Item { get; protected set; }
    public int currentIndex; // 새로 추가된 필드

    protected override void Start()
    {
        base.Start();
        Init();
    }

    protected virtual void Init()
    {
        nextViewBtn.onClick.AddListener(SelectNextItem);
        prevViewBtn.onClick.AddListener(SelectPreviousItem);
        backButton.onClick.AddListener(OnBackButtonClicked);
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
        
        itemName.text = item.Params.Name;

        switch (item.Params.Rarity)
        {
            case ItemRarity.Basic:
                rarity.text = "품질 : 일반" ;
                break;
            case ItemRarity.Common:
                rarity.text = "품질 : 커먼";
                break;
            case ItemRarity.Rare:
                rarity.text = "품질 : 레어";
                break;
            case ItemRarity.Epic:
                rarity.text = "품질 : 에픽";
                break;
            case ItemRarity.Legendary:
                rarity.text = "품질 : 전설";
                break;
        }

        switch (item.Params.Type)
        {
            case ItemType.Usable:
                type.text = "소모품";
                break;
            case ItemType.Exp:
                type.text = "경험치";
                break;
            case ItemType.Material:
                type.text = "재료";
                break;
            case ItemType.Crystal:
                type.text = "크리스탈";
                break;
            case ItemType.Weapon:
                type.text = "무기";
                break;
            case ItemType.Helmet:
                type.text = "헬멧";
                break;
            case ItemType.Armor:
                type.text = "갑옷";
                break;
            case ItemType.Leggings:
                type.text = "각반";
                break;
        }
        

        if(description != null)
        {
            description.text = item.Params.Description;
        }

    }

    public void OnBackButtonClicked()
    {
        HideScreen();
        Item.isSelected = false;
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
            // 현재 선택된 아이템이 없거나 현재 인벤토리에 없는 경우, 마지막 아이템 선택
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
            // 현재 선택된 아이템이 없거나 현재 인벤토리에 없는 경우, 첫 번째 아이템 선택
            Initialize(currentItems[0], currentItems);
        }
        else
        {
            int nextIndex = (currentIndex + 1) % currentItems.Count;
            Initialize(currentItems[nextIndex], currentItems);
        }
    }
}