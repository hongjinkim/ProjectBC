using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoBar : MenuScreen
{
    [Header("texts")]
    public TextMeshProUGUI level;
    public TextMeshProUGUI battlePoint;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI diamond;
    public TextMeshProUGUI gem;

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
}
