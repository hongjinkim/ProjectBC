using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DailyStore : PopUp
{
    [Header("Buttons")]
    public Button backButton;

    [Header("Texts")]
    public TextMeshProUGUI gold;
    public TextMeshProUGUI _ironMaterial;
    public TextMeshProUGUI _silverMaterial;
    public TextMeshProUGUI _goldMaterial;


    private PlayerInfo playerInfo;
    List<Item> Materials = new List<Item>();

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
        UpdateMaterialCount();
        backButton.onClick.AddListener(_UIManager.DailyStorePopUp.HideScreen);
    }

    private void OnGoldUpdated(Dictionary<string, object> message)
    {
        gold.text = playerInfo.gold.ToString();
        UpdateMaterialCount();
    }


    public void UpdateMaterialCount()
    {
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Material))
        {
            Materials = GameDataManager.instance.itemDictionary[ItemType.Material];
        }

        foreach (Item item in Materials)
        {
            int count;

            if (item.Params.Id == "Material_Iron")
            {
                count = item.count;
                _ironMaterial.text = count.ToString();
            }
            else if (item.Params.Id == "Material_Silver")
            {
                count = item.count;
                _silverMaterial.text = count.ToString();
            }
            else if (item.Params.Id == "Material_Gold")
            {
                count = item.count;
                _goldMaterial.text = count.ToString();
            }

        }
    }
}
