using System;
using System.Collections.Generic;
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
        EventManager.StartListening(EventType.FundsUpdated, OnFundsUpdated);

    }

    private void OnDisable()
    {

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
