using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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

    public void Start()
    {
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


    public void OnHeroSelected(HeroInfo info, int idx)
    {
        _UIManager.ToggleMenuBar(false);
        _UIManager.TogglePlayerInfo(false);
        _idx = idx;
        _info = info;
        Initialize();
        transform.SetAsLastSibling();
    }
    public void Initialize()
    {
        
        characterImage.sprite = _info.Sprite;


        levlText.text = _info.level.ToString();
        //BattlePointText.text
        HealthText.text = _info.hp.ToString();
        AttackText.text = _info.attackDamage.ToString();
        DefenseText.text = _info.defense.ToString();
        ResistanceText.text = _info.magicResistance.ToString();
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
    }
}
