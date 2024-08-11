using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static DB;
using static JsonHelper;
using Unity.VisualScripting;

public class GameDataManager : MonoSingleton<GameDataManager>
{
    [SerializeField] private static string savePath => Application.persistentDataPath;

    [SerializeField] private string _saveFilename = "savegame.json";

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

    public Dictionary<ItemType, List<Item>> itemDictionary;

    void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnValidate()
    {
        LoadDatas();
    }

    public override void Init()
    {
        ItemCollection.active = itemCollection;
        //InitializeHeroes();

        LoadGame();

        
        
    }

    void Start()
    {
        Initialize();
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
        MakeItemDictionary();
      
    }
    public void SaveGame()
    {
        string jsonFile = _playerInfo.ToJson();
        FileManager.WriteToFile(_saveFilename, jsonFile);
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

    void Initialize()
    {
        UpdateFunds();
        //UpdateLevel();
        //UpdateBattlePoint();
        //UpdateAllInventorys();
    }

    public void UpdateFunds()
    {
        EventManager.TriggerEvent(EventType.FundsUpdated, null);
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
            EventManager.TriggerEvent(EventType.HeroUpdated, new Dictionary<string, object> { { "heroes", _playerInfo.heroes } });
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

    private void MakeItemDictionary()
    {
        itemDictionary = new Dictionary<ItemType, List<Item>>();
        foreach(Item item in _playerInfo.items)
        {
            if (!itemDictionary.ContainsKey(item.Params.Type))
            {
                itemDictionary[item.Params.Type] = new List<Item>();
            }

            itemDictionary[item.Params.Type].Add(item);
        }
    }

    public void AddItem(Item item, int amount = 1)
    {
        _playerInfo.items.Add(item);

        if (!itemDictionary.ContainsKey(item.Params.Type))
        {
            itemDictionary[item.Params.Type] = new List<Item>();
        }
        if (item.IsCanStacked)
        {
            bool hasItem = false;
            foreach (Item _item in itemDictionary[item.Params.Type])
            {
                if (item.Params.Id == _item.Params.Id)
                {
                    _item.count+=amount;
                    hasItem = true;
                    break;
                }
            }
            if (!hasItem)
            {
                itemDictionary[item.Params.Type].Add(item);
            }
        }
        else
        {
            itemDictionary[item.Params.Type].Add(item);
        }

        EventManager.TriggerEvent(EventType.ItemUpdated, new Dictionary<string, object> { { "type", item.Params.Type } });
    }

    public void RemoveItem(Item item, int amount = 1)
    {
        _playerInfo.items.Remove(item);

        if (item.IsCanStacked)
        {
            foreach (Item _item in itemDictionary[item.Params.Type])
            {
                if (item.Params.Id == _item.Params.Id)
                {
                    if(amount < _item.count)
                        _item.count-=amount;
                    else
                    {
                        Debug.Log("제거하려는 아이템의 수가 가지고 있는 수 보다 많습니다.");
                    }
                    if (_item.count <=0)
                    {
                        itemDictionary[item.Params.Type].Remove(item);
                    }
                    break;
                }
            }
        }
        else
        {
            itemDictionary[item.Params.Type].Remove(item);
        }

        if (itemDictionary[item.Params.Type].Count == 0)
        {
            itemDictionary.Remove(item.Params.Type);
        }

        EventManager.TriggerEvent(EventType.ItemUpdated, new Dictionary<string, object> { { "type", item.Params.Type } });
    }

    

}