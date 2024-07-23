using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroPage : HeroScreen
{
    private HeroInfo _info;
    public HeroMenuManager heroMenuManager;

    [Header("Images")]
    public Image character;

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
        HeroSelected += OnHeroSelected;
    }
    private void OnDisable()
    {
        HeroSelected -= OnHeroSelected;
    }

    void OnHeroSelected(HeroInfo info)
    {
        _UIManager.ToggleMenuBar(false);
        _UIManager.TogglePlayerInfo(false);
        _info = info;
        Initialize();
        transform.SetAsLastSibling();
    }
    void Initialize()
    {
        character.sprite = _info.Sprite;

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


}
