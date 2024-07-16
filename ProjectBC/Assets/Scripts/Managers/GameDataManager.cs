using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static DB;
using static JsonHelper;
using Unity.VisualScripting;
using System.IO;

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

    // Hero Event (추가)
    public static event Action<List<HeroInfo>> HeroesUpdated;

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
        if (_playerInfo.heroes == null || _playerInfo.heroes.Count == 0)
        {
            Debug.Log("No heroes data found. Initializing with default heroes.");
            InitializeDefaultHeroes();
        }
        // 히어로 데이터 로드 (추가)
        if (FileManager.LoadFromFile("heroes_" + _saveFilename, out var heroesJsonString))
        {
            _playerInfo.LoadHeroesFromJson(heroesJsonString);

            if (_debugValues)
            {
                Debug.Log("SaveManager.LoadGame: heroes_" + _saveFilename + " json string: " + heroesJsonString);
            }
        }
        if (_playerInfo.heroes == null || _playerInfo.heroes.Count == 0)
        {
            Debug.Log("No heroes data found. Initializing with default heroes.");
            InitializeDefaultHeroes();
        }
        else
        {
            Debug.Log($"Loaded {_playerInfo.heroes.Count} heroes from saved data.");
        }
        // notify other game objects 
        if (_playerInfo != null)
        {
            GameDataLoaded?.Invoke(_playerInfo);
            HeroesUpdated?.Invoke(_playerInfo.heroes); // 추가
        }
    }
    private void InitializeDefaultHeroes()
    {
        _playerInfo.heroes = new List<HeroInfo>
    {
        new HeroInfo("Warrior", HeroClass.Knight, CharacteristicType.MuscularStrength) { id = 1 },
        new HeroInfo("Archer", HeroClass.Archer, CharacteristicType.Agility) { id = 2 },
        // 추가 기본 히어로...
    };
    }
    public void SaveGame()
    {
        string jsonFile = _playerInfo.ToJson();
        string heroesJson = _playerInfo.HeroesToJson(); // 추가

        // save to disk with FileDataHandler
        if (FileManager.WriteToFile(_saveFilename, jsonFile) &&
            FileManager.WriteToFile("heroes_" + _saveFilename, heroesJson) && // 추가
            _debugValues)
        {
            Debug.Log("SaveManager.SaveGame: " + _saveFilename + " json string: " + jsonFile);
            Debug.Log("SaveManager.SaveGame: heroes_" + _saveFilename + " json string: " + heroesJson); // 추가
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

    // 히어로 관련 메서드 (추가)
    public List<HeroInfo> GetAllHeroes()
    {
        return _playerInfo.heroes;
    }

    public void AddHero(HeroInfo hero)
    {
        _playerInfo.heroes.Add(hero);
        SaveGame();
        HeroesUpdated?.Invoke(_playerInfo.heroes);
    }

    public void UpdateHero(HeroInfo hero)
    {
        int index = _playerInfo.heroes.FindIndex(h => h.id == hero.id);
        if (index != -1)
        {
            _playerInfo.heroes[index] = hero;
            SaveGame();
            HeroesUpdated?.Invoke(_playerInfo.heroes);
        }
    }

    public HeroInfo GetHero(int id)
    {
        return _playerInfo.heroes.Find(h => h.id == id);
    }
    public void InitializeAllHeroes()
    {
        _playerInfo.heroes.Clear();
        _playerInfo.heroes.Add(new HeroInfo("Warrior", HeroClass.Knight, CharacteristicType.MuscularStrength) { id = 1 });
        _playerInfo.heroes.Add(new HeroInfo("Archer", HeroClass.Archer, CharacteristicType.Agility) { id = 2 });
        _playerInfo.heroes.Add(new HeroInfo("Wizard", HeroClass.Wizard, CharacteristicType.Intellect) { id = 3 });
        _playerInfo.heroes.Add(new HeroInfo("Priest", HeroClass.Priest, CharacteristicType.Intellect) { id = 4 });
    }
    public void SaveHeroesToGameData()
    {
        SaveGame();
    }
    public void ResetHeroData()
    {
        _playerInfo.heroes.Clear();
        string fullPath = Path.Combine(Application.persistentDataPath, "heroes_" + _saveFilename);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        InitializeAllHeroes();  // InitializeDefaultHeroes() 대신
        SaveHeroesToGameData();
        HeroesUpdated?.Invoke(_playerInfo.heroes);
    }
}