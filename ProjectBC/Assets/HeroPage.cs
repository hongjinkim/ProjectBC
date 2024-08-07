using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroPage : HeroScreen
{

    public HeroMenuManager heroMenuManager;
    public ItemCollection itemCollection;

    [Header("current  hero info")]
    [SerializeField] private HeroInfo _info;
    private Hero _currentHero;
    public int _idx;

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
            heroPotion.UpdateCurrentHero(_info);  // HeroPotion 업데이트
        }
    }

    public void OnHeroSelected(HeroInfo info, int idx)
    {
        _UIManager.ToggleMenuBar(false);
        _UIManager.TogglePlayerInfo(false);
        _idx = idx;
        _info = info;
        Initialize();
        attributeUI.UpdateHeroAttributes(info);
        if (heroPotion != null)
        {
            heroPotion.UpdateCurrentHero(info);  // HeroPotion 업데이트
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
            heroPotion.UpdateCurrentHero(_info);  // HeroPotion 업데이트
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
        var ExpItem = GameDataManager.instance.playerInfo.items;

        for (int i = 0; i < ExpItem.Count; i++)
        {
            if (ExpItem[i].id == scrollType)
            {
                if (ExpItem[i].Count > 0)
                {
                    ExpItem[i].Count--;

                    GameDataManager.instance.UpdateItem();
                    GameDataManager.instance.UpdateFunds();

                    if (ExpScroll.Instance != null)
                    {
                        ExpScroll.Instance.UpdateExpScrollCount();
                    }

                    if (ExpItem[i].Count == 0)
                    {
                        ExpItem.RemoveAt(i);
                    }
                    return true;
                }
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
            Debug.Log($"{scrollType} 경험치 스크롤이 없습니다.");
        }

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
            heroPotion.UpdateCurrentHero(_info);  // HeroPotion 업데이트
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
    }
    private int CalculateBattlePoint(HeroInfo hero)
    {
        // 이 계산식은 게임 밸런스에 따라 조정해야 할 수 있습니다.
        return hero.hp / 10 + hero.attackDamage * 2 + hero.defense * 3 + hero.magicResistance * 3 + hero.level * 5;
    }
}
