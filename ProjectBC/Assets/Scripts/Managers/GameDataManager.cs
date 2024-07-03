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

    //playerInfo inventory Event
    public static event Action<PlayerInfo> EquipableInventoryUpdated;
    public static event Action<PlayerInfo> UsableInventoryUpdated;
    public static event Action<PlayerInfo> MaterialInventoryUpdated;
    public static event Action<PlayerInfo> CrystalInventoryUpdated;
    //characterData Event

    //itemData Event

    // private class
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private CharacterBaseData[] _characterBaseDatas;
    [SerializeField] private ItemBaseData[] _itemBaseDatas;

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
        UpdateAllInventorys();
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
    void UpdateAllInventorys()
    {
        UpdateEquipableInventory();
        UpdateUsableInventory();
        UpdateMaterialInventory();
        UpdateCrystalInventory();
    }

    void UpdateEquipableInventory()
    {
        if (_playerInfo != null)
            EquipableInventoryUpdated?.Invoke(_playerInfo);
    }
    void UpdateUsableInventory()
    {
        if (_playerInfo != null)
            UsableInventoryUpdated?.Invoke(_playerInfo);
    }
    void UpdateMaterialInventory()
    {
        if (_playerInfo != null)
            MaterialInventoryUpdated?.Invoke(_playerInfo);
    }
    void UpdateCrystalInventory()
    {
        if (_playerInfo != null)
            CrystalInventoryUpdated?.Invoke(_playerInfo);
    }

    private void LoadDatas()
    {
        _characterBaseDatas = LoadArrayJson<CharacterBaseData>("CharacterBaseData.json");
        _itemBaseDatas = LoadArrayJson<ItemBaseData>("ItemBaseData.json");
    }

}
