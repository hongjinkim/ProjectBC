using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DB2
{

    // Ŭ����
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
            new ItemDataList(0, "��", "�Ϲ�", "���̴�"),
            new ItemDataList(1, "�� ����", "�Ϲ�", "�� �����̴�"),
            new ItemDataList(2, "������ ��ȥ", "���", "������ ��ȥ�̴�"),
            new ItemDataList(3, "�ڻԼ��� ��ȥ", "���", "�ڻԼ��� ��ȥ�̴�"),
        };
    }
}
