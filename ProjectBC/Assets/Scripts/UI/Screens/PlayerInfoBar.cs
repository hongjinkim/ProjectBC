using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBar : MenuScreen
{
    [Header("texts")]
    public TextMeshProUGUI level;
    public TextMeshProUGUI battlePoint;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI diamond;
    public TextMeshProUGUI gem;

    [Header("panel")]
    public Image panel;
    public Image profile;

    private void OnEnable()
    {
        GameDataManager.FundsUpdated += OnFundsUpdated;
        GameDataManager.LevelUpdated += OnLevelUpdated;
        GameDataManager.BattlePointUpdated += OnBattlePointUpdated;
    }

    private void OnDisable()
    {
        GameDataManager.FundsUpdated -= OnFundsUpdated;
        GameDataManager.FundsUpdated -= OnLevelUpdated;
        GameDataManager.FundsUpdated -= OnBattlePointUpdated;
    }

    void Start()
    {

    }

    void OnFundsUpdated(PlayerInfo info)
    {
        gold.text = info.gold.ToString();
        diamond.text = info.diamond.ToString();
        gem.text = info.gem.ToString();
    }

    void OnLevelUpdated(PlayerInfo info)
    {
        level.text = "Lv. " + info.level.ToString("D2");
    }
    void OnBattlePointUpdated(PlayerInfo info)
    {
        battlePoint.text = info.battlePoint.ToString();
    }

    public void HideMenu()
    {
        //Color color = panel.color;
        //color.a = 0f;
        //panel.color = color;

        profile.gameObject.SetActive(false);
        level.SetActive(false);
        battlePoint.SetActive(false);
    }

    public void ShowMenu()
    {
        profile.gameObject.SetActive(true);
        level.SetActive(true);
        battlePoint.SetActive(true);
    }
}
