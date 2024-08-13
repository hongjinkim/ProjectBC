using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public Transform _SalesListTransform;
    public List<Prize> _goldPrizeList;
    public int price;

    void Start()
    {
        DisplayShopItems();
    }

    void DisplayShopItems()
    {
        foreach (Transform child in _SalesListTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (Prize i in _goldPrizeList)
        {
            GameObject GO = Instantiate(i.prize, _SalesListTransform);

            var item = new Item(i.id);

            price = i.price;

            SetItemDetails(GO, i, item);
        }
    }

    public void SetItemDetails(GameObject GO, Prize prize, Item item)
    {
        GO.transform.GetChild(0).GetComponent<Image>().sprite = ItemCollection.active.GetItemIcon(item).sprite;
        GO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = price.ToString();
        GO.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnBuyButtonClicked(prize, item));
    }
    
    void OnBuyButtonClicked(Prize prize, Item itemToPurchase)
    {
        PurchaseItem(prize, itemToPurchase);
    }

    public void PurchaseItem(Prize prize, Item itemToPurchase)
    {

        if (GameDataManager.instance.playerInfo.gold >= prize.price)
        {
            GameDataManager.instance.playerInfo.gold -= prize.price;

            GameDataManager.instance.AddItem(itemToPurchase);

            // 구매 이벤트 발생 (아이템 정보 전달)
            // EventManager.TriggerEvent(EventType.ItemUpdated, new Dictionary<string, object> 
            // { 
            //     { "item", itemToPurchase.Params.Name + "을(를) 구매했습니다." } 
            // });
            ToastMsg.instance.ShowMessage(itemToPurchase.Params.Name+"을(를) 구매했습니다.", 0.5f);

            EventManager.TriggerEvent(EventType.FundsUpdated, null);

            // 아이템 갱신 이벤트 발생
            // EventManager.TriggerEvent(EventType.ItemUpdated, new Dictionary<string, object> 
            // { 
            //     { "type", itemToPurchase.Params.Type } 
            // });
        }
        else
        {
            ToastMsg.instance.ShowMessage("골드가 부족합니다.", 0.5f);
            // // 자금 부족 메시지 출력
            // EventManager.TriggerEvent(EventType.ItemUpdated, new Dictionary<string, object> 
            // { 
            //     { "message", "골드가 부족합니다." } 
            // });
        }
    }

    // private IEnumerator PurchaseNotice(string message)
    // {
    //     UIManager.instance.ShowMessage(message);
    //     yield return new WaitForSeconds(2f);  // 2초 동안 메시지 표시
    //     UIManager.instance.HideMessage();
    // }


}
