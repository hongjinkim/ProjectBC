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
    public ExpScroll expScroll;

    [Header("current  hero info")]
    [SerializeField] private HeroInfo _info;
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

    public void Start()
    {
        transform.SetAsFirstSibling();
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

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

    //public int ExpScrollUse(int amount)
    //{
    //    for (int i = 0; i < GameDataManager.instance.playerInfo.items.Count; i++)
    //    {

    //    }
    //}
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
        // neededEXP만큼 경험치 책 사용
    }
}
