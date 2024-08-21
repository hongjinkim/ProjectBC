using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EquipmentInfo : ItemInfo
{
    [Header("Head")]
    public TextMeshProUGUI level;
    public TextMeshProUGUI battlePoint;

    [Header("Basic Stat")]
    public TextMeshProUGUI luckyPoint;
    public List<TextMeshProUGUI> basicStats;
    public List<TextMeshProUGUI> magicStats;


    [Header("Lock")]
    [SerializeField] private Button lockButton;
    [SerializeField] private TextMeshProUGUI buttonText;


    [Header("Function")]
    public Button disassemblyBtn;
    public Button equipBtn;

    public bool isLocked = false;

    public Disassembly disassembly;

    private string luckyColor;
    private string statColor;
    private string id;

    protected override void Init()
    {
        base.Init();
        lockButton.onClick.AddListener(ToggleItemLock);
        disassemblyBtn.onClick.AddListener(SelectedDisassemblyButtonClikced);
        equipBtn.onClick.AddListener(EquipButtonClicked);
        UpdateButtonUI();
    }
    
    
    public override void Initialize(Item item, List<Item> items)
    {
        base.Initialize(item, items);
        level.text = "Lv. " + item.Params.Level.ToString("D2");
        battlePoint.text = item.battlePoint.ToString();

        luckyColor = SetLuckyPointColor(item.luckyPercent);
        luckyPoint.text = $"<color=green>럭키포인트 :</color> <color={luckyColor}>{item.luckyPoint}({item.luckyPercent}%)</color>";
       
        for (int i = 0; i < basicStats.Count; i++)
        {
            if (item.stat.basic.Count > i)
            {
                statColor = SetAdditionalStatColor(item.stat.basic[i].value, item.stat.basic[i].maxValue);
                basicStats[i].text = item.stat.basic[i].value <= 0 ? null : item.stat.basic[i].name.ToString() + "  " + "+" + item.stat.basic[i].value.ToString() + $"    <color={statColor}>(+" + (item.stat.basic[i].value - item.stat.basic[i].minValue).ToString() + ")</color>";
            }
            else
                basicStats[i].text = null;
        }
        for (int i = 0; i < magicStats.Count; i++)
        {
            if (item.stat.magic.Count > i)
            {
                magicStats[i].text = item.stat.magic[i].value <= 0 ? null : item.stat.magic[i].name.ToString() + "  " + "+" + item.stat.magic[i].value.ToString();
            }   
            else
                magicStats[i].text = null;
        }

        UpdateButtonUI();
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
        Debug.Log("장착");
    }

    private string SetLuckyPointColor(int luck)
    {
        if(luck >=80)
        {
            return "red";
        }
        else if(luck >= 60)
        {
            return "purple";
        }
        else if (luck >= 40)
        {
            return "#00698C";// blue
        }
        else if (luck >= 20)
        {
            return "green";
        }
        else
        {
            return "grey";
        }
    }
    private string SetAdditionalStatColor(int stat, int max)
    {
        if (stat == 0)
            return "grey";

        float percentage = (float)stat / (float)max;
        if (percentage >= 0.8)
        {
            return "red";
        }
        else if (percentage >= 0.6)
        {
            return "purple";
        }
        else if (percentage >= 0.4)
        {
            return "#00698C";// blue
        }
        else if (percentage >= 0.2)
        {
            return "green";
        }
        else
        {
            return "grey";
        }
    }
}