using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DB2
{

    // 클래스
    [System.Serializable]
    public class ItemDataList
    {
        public int idx;
        public string itemName;
        public string itemQuality;
        public string itemDescription;

    

        public ItemDataList(int idx, string itemName, string itemQuality, string itemDescription)
        {
            this.idx = idx;
            this.itemName = itemName;
            this.itemQuality = itemQuality;
            this.itemDescription = itemDescription;
        }
    }



    ////////////////////
    public static class ItemData
    {
        public static List<ItemDataList> itemDataList = new List<ItemDataList>()
        {
            new ItemDataList(0, "돌", "일반", "돌이다"),
            new ItemDataList(1, "빈 물병", "일반", "빈 물병이다"),
            new ItemDataList(2, "원숭이 영혼", "고급", "원숭이 영혼이다"),
            new ItemDataList(3, "코뿔소의 영혼", "고급", "코뿔소의 영혼이다"),
        };
    }
}
