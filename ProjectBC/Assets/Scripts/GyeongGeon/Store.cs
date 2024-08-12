using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Prize
{
    public string id;
    public float price;
    public Sprite Image;
}

public class Store : MonoBehaviour
{
    public Transform SalesListTransform;
    public List<GameObject> goldPrizePrefabList;
    public List<GameObject> dailyItems;

    public TextMeshProUGUI itemNameText;   // 아이템 이름을 표시하는 텍스트
    public TextMeshProUGUI priceText;      // 아이템 가격을 표시하는 텍스트
    public Image itemIconImage; // 아이템 아이콘을 표시하는 이미지
    public Button buyButton;    // 구매 버튼

    void Start()
    {
        DisplayShopItems();
    }

    void DisplayShopItems()
    {
        // 기존에 표시된 아이템 UI 제거
        foreach (Transform child in SalesListTransform)
        {
            Destroy(child.gameObject);
        }

        // 아이템 프리팹 리스트의 각 프리팹을 UI로 표시
        for (int i = 0; i < goldPrizePrefabList.Count; i++)
        {
            // 프리팹을 가져와서 부모 객체에 추가
            GameObject goods = Instantiate(goldPrizePrefabList[i], SalesListTransform);

            dailyItems.Add(goods);

            // 생성된 UI에 아이템 세부 정보를 설정
            //Item item = dailyItems[i];
            //SetItemDetails(item);

            // 구매 버튼에 클릭 이벤트 연결
            //buyButton.onClick.AddListener(() => OnBuyButtonClicked(item));
        }
    }

    // 아이템 세부 정보를 설정하는 메서드
    public void SetItemDetails(Item item)
    {
        SetName(item.Params.Name);
        SetIcon(item.Icon.sprite);
        SetPrice(item.Params.Price);
    }

    // 아이템 이름을 설정하는 메서드
    public void SetName(string name)
    {
        itemNameText.text = name;
    }

    // 아이템 아이콘을 설정하는 메서드
    public void SetIcon(Sprite icon)
    {
        itemIconImage.sprite = icon;
    }

    // 아이템 가격을 설정하는 메서드
    public void SetPrice(int price)
    {
        priceText.text = price.ToString();
    }
    

    void OnBuyButtonClicked(Item itemToPurchase)
    {
        PurchaseItem(itemToPurchase);
    }

    public void PurchaseItem(Item itemToPurchase)
    {
        // 플레이어의 현재 자금을 확인하여 구매할 수 있는지 체크
        int itemPrice = itemToPurchase.Params.Price;
        if (GameDataManager.instance.playerInfo.gold >= itemPrice)
        {
            // 플레이어의 자금에서 아이템 가격을 차감
            GameDataManager.instance.playerInfo.gold -= itemPrice;

            // 아이템을 인벤토리에 추가
            var inventory = GameDataManager.instance.playerInfo.items;

            // 인벤토리에 이미 같은 아이템이 있는지 확인
            bool hasItem = false;

            foreach (Item _item in inventory)
            {
                if (itemToPurchase.Params.Id == _item.Params.Id)
                {
                    _item.Count += itemToPurchase.Count;
                    hasItem = true;
                    break;
                }
            }

            // 인벤토리에 같은 아이템이 없으면 새로 추가
            if (!hasItem)
            {
                //StartCoroutine(PurchaseNotice(itemToPurchase.Params.Name + "을(를) 구매했습니다."));
                inventory.Add(itemToPurchase);
            }

            // 구매 완료 후 UI 업데이트
            GameDataManager.instance.UpdateItem();
            GameDataManager.instance.UpdateFunds();
        }
        else
        {
            // 자금 부족 메시지 출력
            //StartCoroutine(PurchaseNotice("골드가 부족합니다."));
        }
    }

    // private IEnumerator PurchaseNotice(string message)
    // {
    //     UIManager.instance.ShowMessage(message);
    //     yield return new WaitForSeconds(2f);  // 2초 동안 메시지 표시
    //     UIManager.instance.HideMessage();
    // }


}
