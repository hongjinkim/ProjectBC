using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{ 
    [SerializeField] private List<DB.CharacterBaseData> charaterBaseData = DB.GameData.characterBaseDatas;
    [SerializeField] private List<DB2.ItemDataList> itemDataLists = DB2.ItemData.itemDataList;
}
