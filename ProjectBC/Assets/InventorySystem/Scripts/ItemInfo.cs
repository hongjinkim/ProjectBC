using System.Collections.Generic;
using System.Linq;
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
    public TextMeshProUGUI luckyPoint;
    public List<TextMeshProUGUI> basicStats;
    public List<TextMeshProUGUI> magicStats;

    //[Header("Magic Stat")]

    //[Header("Rune")]

    //[Header("Buttons")]

    //public Button backButton;

    public Item Item { get; protected set; }

    //protected static readonly List<PropertyId> Sorting = new List<PropertyId>
    //    {
    //        PropertyId.Damage,
    //        PropertyId.StaminaMax,
    //        PropertyId.Blocking,
    //        PropertyId.Resistance
    //    };

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

    public virtual void Initialize(Item item)
    {
        Item = item;

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
            luckyPoint.text = "럭키포인트: " + item.LuckyPoint.ToString() + "(" +((int)(item.LuckyPoint*100 / item.MaxLuckyPoint)).ToString()+ "%)";

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
            // 장비 아이템이 아닐 경우 스탯 미표기 후 설명 표시
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
        
    }
    public void OnBackButtonClicked()
    {
        transform.SetSiblingIndex(0);
    }
}