using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static DB;
using static JsonHelper;

public class DBManager : MonoBehaviour
{
    [SerializeField] private CharacterBaseData[] _charaterBaseDatas;

    private void Start()
    {
        ReadJson();
        Inspector();
    }

    private void ReadJson()
    {
        GameData.characterBaseData = LoadArrayJson<CharacterBaseData>("CharacterBaseData.json");
    }
    private void Inspector()
    {
        _charaterBaseDatas = GameData.characterBaseData;
    }

    
   
}
