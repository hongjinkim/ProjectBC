using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static DB;
using static JsonHelper;
using Unity.VisualScripting;


public class GameDataManager : MonoBehaviour
{
    [SerializeField] private string savePath;

    private static GameDataManager _instance;
    public static GameDataManager instance
    {
        get { return _instance; }
    }

    [SerializeField] private string _saveFilename = "savegame.dat";


    [SerializeField] private bool _debugValues;
    [SerializeField] private bool _resetGame;

    //playerInfo Event
    public static event Action<PlayerInfo> FundsUpdated;
    public static event Action<PlayerInfo> LevelUpdated;
    public static event Action<PlayerInfo> BattlePointUpdated;

    public static event Action<PlayerInfo> GameDataLoaded;


    //characterData Event



    //itemData Event
    public static event Action ItemUpdated;

    // private class
    [SerializeField] PlayerInfo _playerInfo;
    public PlayerInfo playerInfo { set => _playerInfo = value; get => _playerInfo; }
    public ItemCollection itemCollection;


    [SerializeField] public CharacterBaseData[] characterBaseData;

    [SerializeField] public ItemBaseData[] equipmentBaseData;
    [SerializeField] public ItemBaseData[] itemBaseData;


    [SerializeField] public EquipmentStatData[] equipmentStatData;


    void OnApplicationQuit()
    {
        SaveGame();
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        savePath = Application.persistentDataPath;
    }

    void Start()
    {
        LoadDatas();
        //if saved data exists, load saved data

        LoadGame();

        Init();
    }

    public PlayerInfo NewGame()
    {
        return new PlayerInfo();
    }

    public void LoadGame()
    {
        // load saved data from FileDataHandler

        if (_playerInfo == null || _resetGame)
        {
            if (_debugValues)
            {
                Debug.Log("GAME DATA MANAGER LoadGame: Initializing game data.");
            }

            _playerInfo = NewGame();
        }
        else if (FileManager.LoadFromFile(_saveFilename, out var jsonString))
        {
            _playerInfo.LoadJson(jsonString);

            if (_debugValues)
            {
                Debug.Log("SaveManager.LoadGame: " + _saveFilename + " json string: " + jsonString);
            }
        }

        // notify other game objects 
        if (_playerInfo != null)
        {
            GameDataLoaded?.Invoke(_playerInfo);
        }
    }

    public void SaveGame()
    {
        string jsonFile = _playerInfo.ToJson();

        // save to disk with FileDataHandler
        if (FileManager.WriteToFile(_saveFilename, jsonFile) && _debugValues)
        {
            Debug.Log("SaveManager.SaveGame: " + _saveFilename + " json string: " + jsonFile);
        }
    }

    void OnSettingsShown()
    {
        // pass the GameData to the Settings Screen
        if (_playerInfo != null)
        {
            GameDataLoaded?.Invoke(_playerInfo);
        }
    }

    void OnSettingsUpdated(PlayerInfo playerInfo)
    {
        _playerInfo = playerInfo;
        SaveGame();
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

    public void UpdateItem()
    {
        if (_playerInfo != null)
            ItemUpdated?.Invoke();
    }


    private void LoadDatas()
    {
        characterBaseData = LoadArrayJson<CharacterBaseData>("CharacterBaseData.json");

        equipmentBaseData = LoadArrayJson<ItemBaseData>("EquipmentBaseData.json");
        itemBaseData = LoadArrayJson<ItemBaseData>("ItemBaseData.json");

        equipmentStatData = LoadArrayJson<EquipmentStatData>("EquipmentStatData.json");

        MakeItemCollection();
    }

    private void MakeItemCollection()
    {
        itemCollection.items.Clear();
        foreach(ItemBaseData data in equipmentBaseData)
        {
            itemCollection.items.Add(MakeItem(data));
        }
        foreach (ItemBaseData data in itemBaseData)
        {
            itemCollection.items.Add(MakeItem(data));
        }

    }

    private ItemParams MakeItem(ItemBaseData data)
    {
        return new ItemParams
        {

            Id = data.Id,
            Level = data.Level,
            Rarity = data.Rarity,
            Type = data.Type,
            Class = data.Class,
            Tags = new List<ItemTag> { data.Tag1, data.Tag2, data.Tag3 },
            Properties = new List<Property> { new Property(data.PropertyId1, data.PropertyValue1), new Property(data.PropertyId2, data.PropertyValue2) },
            Price = data.Price,
            Weight = data.Weight,
            Material = data.Material,
            IconId = data.IconId,
            SpriteId = data.SpriteId,
            Meta = data.Meta
        };
    }

}
