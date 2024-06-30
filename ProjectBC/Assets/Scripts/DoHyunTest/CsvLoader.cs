using System.Collections.Generic;
using UnityEngine;

public class CsvLoader : MonoBehaviour
{
    private static CsvLoader instance;
    public static CsvLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CsvLoader").AddComponent<CsvLoader>();
                instance.LoadCSVData();
            }
            return instance;
        }
    }

    public class ItemData
    {
        public int index;
        public string itemId;
        public string itemName;
        public string itemQuality;
        public string itemDescription;
    }

    public List<ItemData> itemDataList = new List<ItemData>();


    private void LoadCSVData()
    {
        string csvFilePath = "CSVs/MaterialItemData";
        TextAsset csvFile = Resources.Load<TextAsset>(csvFilePath);
        if (csvFile != null)
        {
            string csvData = csvFile.text;
            string[] rows = csvData.Split('\n');
            for (int i = 1; i < rows.Length; i++) // 첫 번째 행은 헤더이므로 건너뜁니다.
            {
                string[] columns = rows[i].Split(',');
                if (columns.Length == 5)
                {
                    itemDataList.Add(new ItemData
                    {
                        index = int.Parse(columns[0]),
                        itemId = columns[1],
                        itemName = columns[2],
                        itemQuality = columns[3],
                        itemDescription = columns[4]
                    });
                }
            }
        }
    }
}