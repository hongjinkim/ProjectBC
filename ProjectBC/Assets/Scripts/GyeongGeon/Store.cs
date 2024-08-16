using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public Transform _SalesListTransform;
    public List<Prize> _goldPrizeList;
    public List<Prize> _materialPrizeList;
    public List<Prize> _currentPrizeList;

    public Button _goldPageBtn;
    public Button _materialPageBtn;

    public DailyStore _dailyStore;

    void Start()
    {
        _currentPrizeList = _goldPrizeList;
        DisplayShopItems();

        _goldPageBtn.onClick.AddListener(() => OnGoldPageButtonClicked());
        _materialPageBtn.onClick.AddListener(() => OnMaterialPageButtonClicked());
    }

    void DisplayShopItems()
    {
        foreach (Transform child in _SalesListTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (Prize i in _currentPrizeList)
        {
            GameObject GO = Instantiate(i.prize, _SalesListTransform);

            var item = new Item(i.id);

            SetItemDetails(GO, i, item);
        }
    }

    public void SetItemDetails(GameObject GO, Prize prize, Item item)
    {
        GO.transform.GetChild(0).GetComponent<Image>().sprite = ItemCollection.active.GetItemIcon(item).sprite;
        GO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = prize.price.ToString();
        GO.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = ItemCollection.active.GetItemIcon(prize.needmaterial).sprite;
        GO.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnBuyButtonClicked(prize, item));
    }
    
    void OnBuyButtonClicked(Prize prize, Item itemToPurchase)
    {
        if (_currentPrizeList == _goldPrizeList)
        {
            PurchaseItemWithGold(prize, itemToPurchase);
        }
        else if (_currentPrizeList == _materialPrizeList)
        {
            PurchaseItemWithMaterial(prize, itemToPurchase);
        }
    }

    public void PurchaseItemWithGold(Prize prize, Item itemToPurchase)
    {
        if (GameDataManager.instance.playerInfo.gold >= prize.price)
        {
            GameDataManager.instance.playerInfo.gold -= prize.price;
            GameDataManager.instance.AddItem(itemToPurchase);

            ToastMsg.instance.ShowMessage(itemToPurchase.Params.Name + "을(를) 구매했습니다.", 0.5f);
            EventManager.TriggerEvent(EventType.FundsUpdated, null);
        }
        else
        {
            ToastMsg.instance.ShowMessage("골드가 부족합니다.", 0.5f);
        }
    }

    public void PurchaseItemWithMaterial(Prize prize, Item itemToPurchase)
    {
        int currentQuantity = GameDataManager.instance.GetItemQuantity(prize.needmaterial);

        if (currentQuantity >= prize.price)
        {
            GameDataManager.instance.RemoveItem(prize.needmaterial, prize.price);
            GameDataManager.instance.AddItem(itemToPurchase);

            ToastMsg.instance.ShowMessage(itemToPurchase.Params.Name + "을(를) 구매했습니다.", 0.5f);
        }
        else
        {
            ToastMsg.instance.ShowMessage("재료가 부족합니다.", 0.5f);
        }
    }

    void OnGoldPageButtonClicked()
    {
        _currentPrizeList = _goldPrizeList;
        DisplayShopItems();
    }

    void OnMaterialPageButtonClicked()
    {
        _currentPrizeList = _materialPrizeList;
        DisplayShopItems();
    }

}
