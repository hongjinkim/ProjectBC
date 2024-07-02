//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ItemFactory : MonoBehaviour
//{
//    private Dictionary<int, /*클래스명*/> MakeItem = new Dictionary<int, string>();
//    private int index = 0;

//    private int maxTotalStats = 200;
//    private int curTotalStats;
//    private List<string> statName = new List<string>();
//    private List<int> stats = new List<int>();

//    private void CreateItem()
//    {
//        CreateItemStats();
//        Dictionary<string, int> itemStats = new Dictionary<string, int>();
//        for (int i = 0; i < statName.Count; i++)
//        {
//            itemStats.Add(statName[i], stats[i]);
//        }

//        MakeItem.Add(index, itemStats);


//    }

//    private void MakeIndex()
//    {
//        index += 1;
//    }

//    private void CreateItemStats()
//    {
//        int randomStatCount = Random.Range(1, 5);
//        for (int i = 0; i < randomStatCount; i++)
//        {
//            statName.Add("Stat" + i);
//            stats.Add(0);
//        }
//        for (int i = 0; i < stats.Count; i++)
//        {
//            int randBal = Random.Range(0, 11); // 맥스치 보정
//            int rand = Random.Range(0, maxTotalStats - randBal);
//            stats[i] = rand;
//            curTotalStats += stats[i];
//            maxTotalStats -= stats[i];
//        }
//    }
//}
