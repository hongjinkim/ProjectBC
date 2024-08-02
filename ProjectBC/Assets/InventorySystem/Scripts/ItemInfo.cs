using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ItemInfo : MonoBehaviour
{
    [Header("Head")]
    public Image iconBackground;
    public Image icon;
    public Image iconFrame;
    public TextMeshProUGUI level;
    public TextMeshProUGUI battlePoint;
    public TextMeshProUGUI name;
    public TextMeshProUGUI rarity;
    public TextMeshProUGUI type;

    [Header("Basic Stat")]
    public TextMeshProUGUI stat1;
    public TextMeshProUGUI stat2;
    public TextMeshProUGUI stat3;

    [Header("Lock")]
    [SerializeField] private Button lockButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("View")]
    public Button nextViewBtn;
    public Button prevViewBtn;

    public bool isLocked = false;
    public Item Item { get; protected set; }
    public int currentIndex; // 새로 추가된 필드

    public GearBase gearBase;

    public void Awake()
    {
        lockButton.onClick.AddListener(ToggleItemLock);
        nextViewBtn.onClick.AddListener(gearBase.SelectNextItem);
        prevViewBtn.onClick.AddListener(gearBase.SelectPreviousItem);

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
        transform.SetAsFirstSibling();
    }

    public virtual void Initialize(Item item, int index)
    {
        Item = item;
        this.currentIndex = index;
        if (item == null)
        {
            Reset();
            return;
        }
        transform.SetAsLastSibling();
        icon.sprite = ItemCollection.active.GetItemIcon(item).sprite;
        iconBackground.sprite = ItemCollection.active.GetBackground(item) ?? ItemCollection.active.backgroundBrown;
        iconBackground.color = Color.white;
        iconFrame.raycastTarget = true;
        level.text = "Lv. " + item.Params.Level.ToString("D2");
        name.text = item.Params.Name;
        rarity.text = "품질 : " + item.Params.Rarity.ToString();
        type.text = item.Params.Type.ToString();
        if (item.IsEquipment)
        {
            stat1.text = item.Stats[0].value <= 0 ? null : item.Stats[0].id.ToString() + "  " + "+" + item.Stats[0].value.ToString();
            stat2.text = item.Stats[1].value <= 0 ? null : item.Stats[1].id.ToString() + "  " + "+" + item.Stats[1].value.ToString();
            stat3.text = item.Stats[2].value <= 0 ? null : item.Stats[2].id.ToString() + "  " + "+" + item.Stats[2].value.ToString();
        }
        else
        {
            // 장비 아이템이 아닐 경우 스탯 미표기 후 설명 표시
            stat1.text = null;
            stat2.text = null;
            stat3.text = null;
        }
    }

    public void OnBackButtonClicked()
    {
        transform.SetSiblingIndex(0);
    }

    private void ToggleItemLock()
    {
        isLocked = !isLocked;
        UpdateButtonUI();
        Debug.Log($"Item is now {(isLocked ? "locked" : "unlocked")}");
    }

    private void UpdateButtonUI()
    {
        buttonText.text = isLocked ? "Unlock" : "Lock";
    }
}