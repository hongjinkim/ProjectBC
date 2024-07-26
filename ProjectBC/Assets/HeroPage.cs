using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroPage : HeroScreen
{

    public HeroMenuManager heroMenuManager;

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

    public void OnLevelupButtonClicked()
    {
        if (_info.level >= 40 && _info.currentExp == _info.neededExp)
        {
            _info.LevelUp();
        }
    }
}
