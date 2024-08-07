using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBar : BaseScreen
{
    [Header("Texts")]
    public TextMeshProUGUI level;
    public TextMeshProUGUI battlePoint;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI diamond;
    public TextMeshProUGUI gem;

    [Header("Images")]
    public Image profile;

    private void OnEnable()
    {
        EventManager.instance.StartListening(EventType.FundsUpdated, OnFundsUpdated);

    }

    private void OnDisable()
    {
        EventManager.instance.StopListening(EventType.FundsUpdated, OnFundsUpdated);
    }

    void Start()
    {

    }

    void OnFundsUpdated(Dictionary<string, object> message)
    {
        gold.text = GameDataManager.instance.playerInfo.gold.ToString();
        diamond.text = GameDataManager.instance.playerInfo.diamond.ToString();
        gem.text = GameDataManager.instance.playerInfo.gem.ToString();
    }

    void OnLevelUpdated(PlayerInfo info)
    {
        level.text = "Lv. " + info.level.ToString("D2");
    }
    void OnBattlePointUpdated(PlayerInfo info)
    {
        battlePoint.text = info.battlePoint.ToString();
    }

    public void ShowOnlyFunds()
    {
        //Color color = panel.color;
        //color.a = 0f;
        //panel.color = color;

        profile.gameObject.SetActive(false);
        level.SetActive(false);
        battlePoint.SetActive(false);
    }

    public void ShowPlayerInfo()
    {
        profile.gameObject.SetActive(true);
        level.SetActive(true);
        battlePoint.SetActive(true);
    }
}
