using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class PlayerInfo
{

    [Header("Properties")]
    public int gold;
    public int diamond;
    public int gem;

    [Header("Inventory")]
    public List<Item> items;
    public int itemCapacity;

    [Header("Hero")]
    public List<HeroInfo> heroes;

    [Header("PlayerInfo")]
    public string username;
    public int battlePoint;
    public int level;

    [Header("Settings")]
    public float musicVolume;
    public float sfxVolume;


    public PlayerInfo()
    {
        Init();
        InitializeStartingHeroes();
    }
    private void Init()
    {
        this.gold = 0;
        this.diamond = 0;
        this.gem = 0;

        this.username = "GUEST_123456";
        this.battlePoint = 0;
        this.level = 1;

        this.musicVolume = 80f;
        this.sfxVolume = 80f;

        this.itemCapacity = 30;

        InitInventory();
    }

    public void InitializeStartingHeroes()
    {
        // 게임 시작 시 기본 히어로 생성
        heroes = new List<HeroInfo>()
        {
            new HeroInfo("Warrior", HeroClass.Knight, CharacteristicType.MuscularStrength, 1001, "Images/currency/Warrior"),
            new HeroInfo("Wizard", HeroClass.Wizard, CharacteristicType.Intellect, 1002, "Images/currency/Wizard"),
            new HeroInfo("Priest", HeroClass.Priest, CharacteristicType.Intellect, 1003, "Images/currency/Priest"),
            new HeroInfo("Archer", HeroClass.Archer, CharacteristicType.Agility, 1004, "Images/currency/Archer")
        };
    }
    //public string HeroesToJson()
    //{
    //    return JsonUtility.ToJson(new SerializableList<HeroInfo> { list = heroes });
    //}
    //public void LoadHeroesFromJson(string json)
    //{
    //    SerializableList<HeroInfo> loadedHeroes = JsonUtility.FromJson<SerializableList<HeroInfo>>(json);
    //    heroes = loadedHeroes.list;
    //}
    // 디버깅 용 초기 인벤토리 상태 만들기
    private void InitInventory()
    {
        this.items = new List<Item>();
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadJson(string jsonFilepath)
    {
        JsonUtility.FromJsonOverwrite(jsonFilepath, this);
    }
}