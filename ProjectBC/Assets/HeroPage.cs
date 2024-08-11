using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static UnityEditor.Progress;
using Unity.VisualScripting;

public class HeroPage : HeroScreen
{
    public TraitManager traitManager;
    public HeroMenuManager heroMenuManager;
    public ItemCollection itemCollection;

    [Header("current  hero info")]
    [SerializeField] private HeroInfo _info;
    private Hero _currentHero;
    public int _idx;

    [Header("ItemSlots")]
    public ItemSlot Weapon;
    public ItemSlot Helmet;
    public ItemSlot Armor;
    public ItemSlot Leggings;


    [Header("Images")]
    public Image characterImage;

    [Header("Texts")]
    public TextMeshProUGUI levlText;
    public TextMeshProUGUI BattlePointText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI AttackText;
    public TextMeshProUGUI DefenseText;
    public TextMeshProUGUI ResistanceText;

    [Header("ExpGaegu")]
    public Image gauge;

    [SerializeField] private AttributeUI attributeUI;
    [SerializeField] private HeroPotion heroPotion;


    [Header("Buttons")]
    public Button FastEquipButton;
    public Button UnEquipButton;

    public void Start()
    {
        if (attributeUI == null)
        {
            Debug.LogError("AttributeUI is not assigned in the inspector for HeroPage");
        }
        else
        {
            attributeUI.OnHeroInfoChanged += UpdateHeroInfo;
        }

        transform.SetAsFirstSibling();
    }

    private void OnEnable()
    {
        _info.OnExperienceChanged += GaugeBarUpdate;
        _info.OnLevelUp += GaugeBarUpdate;

        FastEquipButton.onClick.AddListener(FastEquip);
        UnEquipButton.onClick.AddListener(UnEquip);
    }

    private void OnDisable()
    {
        _info.OnExperienceChanged -= GaugeBarUpdate;
        _info.OnLevelUp -= GaugeBarUpdate;
    }
    private void UpdateHeroInfo(HeroInfo updatedInfo)
    {
        _info = updatedInfo;
        UpdateUITexts();
        if (heroPotion != null)
        {
            heroPotion.UpdateCurrentHero(_info);  // HeroPotion ������Ʈ
        }
    }

    public void OnHeroSelected(HeroInfo info, int idx)
    {
        _UIManager.ToggleMenuBar(false);
        _UIManager.TogglePlayerInfo(false);
        _idx = idx;
        _info = info;
        Initialize();
        traitManager.SetCurrentHero(_info);
        attributeUI.UpdateHeroAttributes(info);
        if (heroPotion != null)
        {
            heroPotion.UpdateCurrentHero(info);  // HeroPotion ������Ʈ
        }
        transform.SetAsLastSibling();
    }
    public void Initialize()
    {
        if (_info == null)
        {
            Debug.LogError("_info is null in Initialize");
            return;
        }

        characterImage.sprite = _info.Sprite;

        UpdateUITexts();

        if (attributeUI != null)
        {
            attributeUI.UpdateHeroAttributes(_info);
        }
        else
        {
            Debug.LogError("attributeUI is null in Initialize");
        }

        if (heroPotion != null)
        {
            heroPotion.UpdateCurrentHero(_info);  // HeroPotion ������Ʈ
        }
        else
        {
            Debug.LogError("heroPotion is null in Initialize");
        }
    }
    public void OnBackButtonClicked()
    {
        transform.SetAsFirstSibling();
        _UIManager.ToggleMenuBar(true);
        _UIManager.TogglePlayerInfo(true);
    }


    public bool UseExpScroll(string scrollType)
    {
        var ExpItem = GameDataManager.instance.itemDictionary[ItemType.Exp];

        foreach (Item item in ExpItem)
        {
            
            if (item.count > 0)
            {
                item.count--;

                //GameDataManager.instance.UpdateItem();
                //GameDataManager.instance.UpdateFunds();
                EventManager.TriggerEvent(EventType.FundsUpdated, null);
                EventManager.TriggerEvent(EventType.ItemUpdated, new Dictionary<string, object> { {"type", ItemType.Exp } });

                if (ExpScroll.Instance != null)
                {
                    ExpScroll.Instance.UpdateExpScrollCount();
                }

                if (item.count == 0)
                {
                    GameDataManager.instance.RemoveItem(item);
                }
                return true;
            }
            
        }
        return false;
    }

    public void OnAddExpButtonClicked(int buttonType)
    {
        string scrollType = "";
        int exp = 0;

        switch (buttonType)
        {
            case 0: scrollType = "Exp_Basic"; exp = 1; break;
            case 1: scrollType = "Exp_Common"; exp = 2; break;
            case 2: scrollType = "Exp_Rare"; exp = 3; break;
            case 3: scrollType = "Exp_Epic"; exp = 4; break;
            case 4: scrollType = "Exp_Legendary"; exp = 5; break;
        }

        if (UseExpScroll(scrollType))
        {
            _info.AddExp(exp);
        }
        else
        {
            Debug.Log($"{scrollType} ����ġ ��ũ���� �����ϴ�.");
        }

    }
    public void OnTalentButtonClicked()
    {
        traitManager.ShowTraitPanel(_info);
    }
    public void OnLevelupButtonClicked()
    {
        if (_info.level >= 40 && _info.currentExp == _info.neededExp)
        {
            _info.LevelUp();
        }
    }

    public void OnBookUseLevelupButtonClicked()
    {
        float neededExp = _info.neededExp - _info.currentExp;
        if (neededExp <= 0) return;

        string[] scrollTypes = { "Exp_Legendary", "Exp_Epic", "Exp_Rare", "Exp_Common", "Exp_Basic" };
        int[] expValues = { 5, 4, 3, 2, 1 };

        float totalExpGained = 0;

        for (int i = 0; i < scrollTypes.Length; i++)
        {
            while (neededExp > 0)
            {
                if (UseExpScroll(scrollTypes[i]))
                {
                    totalExpGained += expValues[i];
                    neededExp -= expValues[i];
                }
                else
                {
                    break;
                }
            }

            if (neededExp <= 0) break;
        }

        if (totalExpGained > 0)
        {
            _info.AddExp(totalExpGained);
        }
    }

    public void GaugeBarUpdate()
    {
        gauge.fillAmount = Mathf.Clamp01(_info.currentExp / _info.neededExp);

        UpdateUITexts();
        if (heroPotion != null)
        {
            heroPotion.UpdateCurrentHero(_info);  // HeroPotion ������Ʈ
        }
    }

    private void UpdateUITexts()
    {
        levlText.text = _info.level.ToString();
        HealthText.text = _info.hp.ToString();
        AttackText.text = _info.attackDamage.ToString();
        DefenseText.text = _info.defense.ToString();
        ResistanceText.text = _info.magicResistance.ToString();

        int battlePoint = CalculateBattlePoint(_info);
        BattlePointText.text = battlePoint.ToString();

        UpdateEquipment();

    }

    private void UpdateEquipment()
    {
        UpdateWeapon();
        UpdateHelmet();
        UpdateArmor();
        UpdateLeggings();
    }

    private void UpdateWeapon()
    {
        if(_info.Weapon.id != "")
        {
            Weapon.icon.enabled = true;
            Weapon.icon.sprite = _info.Weapon.id == "" ? Weapon.BaseSprite : ItemCollection.active.GetItemIcon(_info.Weapon)?.sprite;
            Weapon.background.sprite = ItemCollection.active.GetBackground(_info.Weapon) ?? ItemCollection.active.backgroundBrown;
            Weapon.background.color = Color.white;
        }
        else
        {
            Weapon.icon.enabled = false;
            Weapon.icon.sprite = Weapon.BaseSprite;
            Weapon.background.sprite = ItemCollection.active.backgroundBrown;
        }
    }
    private void UpdateHelmet()
    {
        if (_info.Helmet.id != "")
        {
            Helmet.icon.enabled = true;
            Helmet.icon.sprite = _info.Helmet.id == "" ? Helmet.BaseSprite : ItemCollection.active.GetItemIcon(_info.Helmet)?.sprite;
            Helmet.background.sprite = ItemCollection.active.GetBackground(_info.Helmet) ?? ItemCollection.active.backgroundBrown;
            Helmet.background.color = Color.white;
        }
        else
        {
            Helmet.icon.enabled = false;
            Helmet.icon.sprite = Helmet.BaseSprite;
            Helmet.background.sprite = ItemCollection.active.backgroundBrown;
        }
    }
    private void UpdateArmor()
    {
        if (_info.Armor.id != "")
        {
            Armor.icon.enabled = true;
            Armor.icon.sprite = _info.Armor.id == "" ? Armor.BaseSprite : ItemCollection.active.GetItemIcon(_info.Armor)?.sprite;
            Armor.background.sprite = ItemCollection.active.GetBackground(_info.Armor) ?? ItemCollection.active.backgroundBrown;
            Armor.background.color = Color.white;
        }
        else
        {
            Armor.icon.enabled = false;
            Armor.icon.sprite = Armor.BaseSprite;
            Armor.background.sprite = ItemCollection.active.backgroundBrown;
        }
    }
    private void UpdateLeggings()
    {
        if (_info.Leggings.id != "")
        {
            Leggings.icon.enabled = true;
            Leggings.icon.sprite = _info.Leggings.id == "" ? Leggings.BaseSprite : ItemCollection.active.GetItemIcon(_info.Leggings)?.sprite;
            Leggings.background.sprite = ItemCollection.active.GetBackground(_info.Leggings) ?? ItemCollection.active.backgroundBrown;
            Leggings.background.color = Color.white;
        }
        else
        {
            Leggings.icon.enabled = false;
            Leggings.icon.sprite = Leggings.BaseSprite;
            Leggings.background.sprite = ItemCollection.active.backgroundBrown;
        }
    }

    private void FastEquip()
    {
        if(GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Weapon))
        {
            var item = FindBestItem(GameDataManager.instance.itemDictionary[ItemType.Weapon]);
            if (_info.Weapon.id == "")
            {
                _info.Weapon = item.Clone();
                GameDataManager.instance.RemoveItem(item);
            }
            else
            {
                if(_info.Weapon.battlePoint < item.battlePoint)
                {
                    GameDataManager.instance.AddItem(_info.Weapon);
                    _info.Weapon = item.Clone();
                    GameDataManager.instance.RemoveItem(item);
                }
            }
        }
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Armor))
        {
            var item = FindBestItem(GameDataManager.instance.itemDictionary[ItemType.Armor]);
            if (_info.Armor.id == "")
            {
                _info.Armor = item.Clone();
                GameDataManager.instance.RemoveItem(item);
            }
            else
            {
                if (_info.Armor.battlePoint < item.battlePoint)
                {
                    GameDataManager.instance.AddItem(_info.Armor);
                    _info.Armor = item.Clone();
                    GameDataManager.instance.RemoveItem(item);
                }
            }
        }
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Helmet))
        {
            var item = FindBestItem(GameDataManager.instance.itemDictionary[ItemType.Helmet]);
            if (_info.Helmet.id == "")
            {
                _info.Helmet = item.Clone();
                GameDataManager.instance.RemoveItem(item);
            }
            else
            {
                if (_info.Helmet.battlePoint < item.battlePoint)
                {
                    GameDataManager.instance.AddItem(_info.Helmet);
                    _info.Helmet = item.Clone();
                    GameDataManager.instance.RemoveItem(item);
                }
            }
        }
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Leggings))
        {
            var item = FindBestItem(GameDataManager.instance.itemDictionary[ItemType.Leggings]);
            if (_info.Leggings.id == "")
            {
                _info.Leggings = item.Clone();
                GameDataManager.instance.RemoveItem(item);
            }
            else
            {
                if (_info.Leggings.battlePoint < item.battlePoint)
                {
                    GameDataManager.instance.AddItem(_info.Leggings);
                    _info.Leggings = item.Clone();
                    GameDataManager.instance.RemoveItem(item);
                }
            }
        }


        UpdateEquipment();
    }

    private void UnEquip()
    {
        if (_info.Weapon.id != "")
        {
            GameDataManager.instance.AddItem(_info.Weapon.Clone());
            _info.Weapon = null;
        }
        if (_info.Helmet.id != "")
        {
            GameDataManager.instance.AddItem(_info.Helmet.Clone());
            _info.Helmet = null;
        }
        if (_info.Armor.id != "")
        {
            GameDataManager.instance.AddItem(_info.Armor.Clone());
            _info.Armor = null;
        }
        if (_info.Leggings.id != "")
        {
            GameDataManager.instance.AddItem(_info.Leggings.Clone());
            _info.Leggings = null;
        }

        UpdateEquipment();
    }

    private Item FindBestItem(List<Item> items)
    {
        int max = 0;
        Item result = items[0];
        foreach(Item item in items)
        {
            if(max < item.battlePoint)
            {
                max = item.battlePoint;
                result = item;
            }
        }
        return result;
    }

    private int CalculateBattlePoint(HeroInfo hero)
    {
        // �� ������ ���� �뷱���� ���� �����ؾ� �� �� �ֽ��ϴ�.
        return hero.hp / 10 + hero.attackDamage * 2 + hero.defense * 3 + hero.magicResistance * 3 + hero.level * 5;
    }
}
