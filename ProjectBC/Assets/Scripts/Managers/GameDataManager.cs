using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static DB;
using static JsonHelper;


[RequireComponent(typeof(SaveManager))]
public class GameDataManager : MonoBehaviour
{
    //playerInfo Event
    public static event Action<PlayerInfo> FundsUpdated;
    public static event Action<PlayerInfo> LevelUpdated;
    public static event Action<PlayerInfo> BattlePointUpdated;


    //characterData Event

    //itemData Event


    // private class
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private CharacterBaseData[] _characterBaseDatas;

    //public class
    public PlayerInfo playerInfo { set => _playerInfo = value; get => _playerInfo; }

    private static SaveManager _saveManager;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        _saveManager = GetComponent<SaveManager>();
    }

    void Start()
    {
        LoadDatas();
        //if saved data exists, load saved data
        _saveManager?.LoadGame();

        Init();
    }

    void Init()
    {
        UpdateFunds();
        UpdateLevel();
        UpdateBattlePoint();
        //UpdateAllInventorys();
    }

    void UpdateFunds()
    {
        if (_playerInfo != null)
            FundsUpdated?.Invoke(_playerInfo);
    }

    void UpdateLevel()
    {
        if (_playerInfo != null)
            LevelUpdated?.Invoke(_playerInfo);
    }

    void UpdateBattlePoint()
    {
        if (_playerInfo != null)
            BattlePointUpdated?.Invoke(_playerInfo);
    }



    private void LoadDatas()
    {
        _characterBaseDatas = LoadArrayJson<CharacterBaseData>("CharacterBaseData.json");
    }

}
