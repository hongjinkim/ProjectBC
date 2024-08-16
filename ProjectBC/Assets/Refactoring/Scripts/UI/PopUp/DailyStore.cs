using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyStore : PopUp
{
    [Header("Buttons")]
    public Button backButton;

    [Header("Texts")]
    public TextMeshProUGUI gold;

    private PlayerInfo playerInfo;

    private void OnEnable()
    {
        EventManager.StartListening(EventType.FundsUpdated, OnGoldUpdated);
    }
    private void OnDisable()
    {
        EventManager.StopListening(EventType.FundsUpdated, OnGoldUpdated);
    }

    public override void ShowScreen()
    {
        base.ShowScreen();
    }

    protected override void Start()
    {
        base.Start();
        playerInfo = GameDataManager.instance.playerInfo;
        OnGoldUpdated(null);
        backButton.onClick.AddListener(_UIManager.DailyStorePopUp.HideScreen);
    }

    private void OnGoldUpdated(Dictionary<string, object> message)
    {
        gold.text = playerInfo.gold.ToString();
    }
}
