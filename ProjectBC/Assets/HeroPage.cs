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

    public void OnAddExpButtonClicked(int exp)
    {
        // 경험치 책 만큼 exp 획득
        // 아이템 개수 제거
        // UI 갱신
        // 아이템 있을 때만 사용 로직 필요
        
        Debug.Log(exp + "경험치 획득");
        _info.AddExp(exp);
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
