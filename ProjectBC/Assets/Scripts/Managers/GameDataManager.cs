using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static DB;
using static JsonHelper;
using Unity.VisualScripting;

public class GameDataManager : MonoSingleton<GameDataManager>
{
    [SerializeField] private string savePath;

    [SerializeField] private string _saveFilename = "savegame.dat";

    [SerializeField] private bool _debugValues;
    [SerializeField] private bool _resetGame;

    

    // private class
    [SerializeField] PlayerInfo _playerInfo;
    public PlayerInfo playerInfo { set => _playerInfo = value; get => _playerInfo; }
    public ItemCollection itemCollection;

    [SerializeField] public CharacterBaseData[] characterBaseData;

    [SerializeField] public ItemBaseData[] equipmentBaseData;
    [SerializeField] public ItemBaseData[] itemBaseData;

    [SerializeField] public EquipmentStatData[] equipmentStatData;

    public Transform noticeTransform;

    void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnValidate()
    {
        LoadDatas();
    }

    private void Awake()
    {
        savePath = Application.persistentDataPath;
        //InitializeHeroes();

        LoadGame();

        Init();

    }

    void Start()
    {
        
        
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
            _playerInfo = NewGame();
        }
        else if (FileManager.LoadFromFile(_saveFilename, out var jsonString))
        {
            _playerInfo.LoadJson(jsonString);
        }

        // 히어로 데이터 로드 (추가)
        //if (FileManager.LoadFromFile("heroes_" + _saveFilename, out var heroesJsonString))
        //{
        //    _playerInfo.LoadHeroesFromJson(heroesJsonString);

        //    if (_debugValues)
        //    {
        //        Debug.Log("SaveManager.LoadGame: heroes_" + _saveFilename + " json string: " + heroesJsonString);
        //    }
        //}

        // notify other game objects 
        if (_playerInfo != null)
        {
            //EventManager.instance.GameDataLoaded?.Invoke(_playerInfo);
            //HeroesUpdated?.Invoke(_playerInfo.heroes); // 추가
        }
    }
    public void SaveGame()
    {
        string jsonFile = _playerInfo.ToJson();
        //string heroesJson = _playerInfo.HeroesToJson(); // 추가

        // save to disk with FileDataHandler
        if (FileManager.WriteToFile(_saveFilename, jsonFile)/* &&
            FileManager.WriteToFile("heroes_" + _saveFilename, heroesJson)*/ && // 추가
            _debugValues)
        {

            //Debug.Log("SaveManager.SaveGame: heroes_" + _saveFilename + " json string: " + heroesJson); // 추가
        }
    }

    //void OnSettingsShown()
    //{
    //    // pass the GameData to the Settings Screen
    //    if (_playerInfo != null)
    //    {
    //        GameDataLoaded?.Invoke(_playerInfo);
    //    }
    //}

    //void OnSettingsUpdated(PlayerInfo playerInfo)
    //{
    //    _playerInfo = playerInfo;
    //    SaveGame();
    //}

    void Init()
    {
        UpdateFunds();
        //UpdateLevel();
        //UpdateBattlePoint();
        //UpdateAllInventorys();
    }

    public void UpdateFunds()
    {
        EventManager.instance.TriggerEvent(EventType.FundsUpdated, null);
    }

    //public void UpdateLevel()
    //{
    //    if (_playerInfo != null)
    //        LevelUpdated?.Invoke(_playerInfo);
    //}

    //public void UpdateBattlePoint()
    //{
    //    if (_playerInfo != null)
    //        BattlePointUpdated?.Invoke(_playerInfo);
    //}

    public void UpdateItem()
    {
        EventManager.instance.TriggerEvent(EventType.ItemUpdated, null);
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
        foreach (ItemBaseData data in equipmentBaseData)
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
            Index = data.Index,
            Name = data.Name,
            Id = data.Id,
            Level = data.Level,
            Rarity = data.Rarity,
            Type = data.Type,
            Class = data.Class,
            Tags = new List<ItemTag> { data.Tag1, data.Tag2, data.Tag3 },
            // Properties = new List<Property> { new Property(data.PropertyId1, data.PropertyValue1), new Property(data.PropertyId2, data.PropertyValue2) },
            Price = data.Price,

            IconId = data.IconId,
            SpriteId = data.SpriteId,
            Meta = data.Meta
        };
    }

    // 히어로 관련 메서드 (추가)
    //public List<HeroInfo> GetAllHeroes()
    //{
    //    if (_playerInfo == null || _playerInfo.heroes == null)
    //    {

    //        return new List<HeroInfo>();
    //    }
    //    return new List<HeroInfo>(_playerInfo.heroes);
    //}

    //public void AddHero(HeroInfo hero)
    //{
    //    _playerInfo.heroes.Add(hero);
    //    HeroesUpdated?.Invoke(_playerInfo.heroes);
    //}

    public void UpdateHero(HeroInfo hero)
    {
        int index = _playerInfo.heroes.FindIndex(h => h.id == hero.id);
        if (index != -1)
        {
            _playerInfo.heroes[index] = hero;
            EventManager.instance.TriggerEvent(EventType.HeroUpdated, new Dictionary<string, object> { { "heroes", _playerInfo.heroes } });
            //HeroesUpdated.Invoke(_playerInfo.heroes);
        }
    }

    //public HeroInfo GetHero(int id)
    //{
    //    return _playerInfo.heroes.Find(h => h.id == id);
    //}
    //public void UpdateHeroes(List<HeroInfo> heroes)
    //{
    //    _playerInfo.heroes = heroes;
    //    HeroesUpdated?.Invoke(_playerInfo.heroes);
    //}
    //public void RemoveHero(HeroInfo hero)
    //{
    //    _playerInfo.heroes.Remove(hero);
    //    HeroesUpdated?.Invoke(_playerInfo.heroes);
    //}

}