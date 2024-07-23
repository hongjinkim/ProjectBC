using System.Collections.Generic;
using UnityEngine;

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
        // ���� ���� �� �⺻ ����� ����
        this.heroes = new List<HeroInfo>();

        this.heroes.Add(HeroInfo.CreateNewHero("Warrior", HeroClass.Knight, CharacteristicType.MuscularStrength));
        this.heroes.Add(HeroInfo.CreateNewHero("Wizard", HeroClass.Wizard, CharacteristicType.Intellect));
        this.heroes.Add(HeroInfo.CreateNewHero("Priest", HeroClass.Priest, CharacteristicType.Intellect));
        this.heroes.Add(HeroInfo.CreateNewHero("Archer", HeroClass.Archer, CharacteristicType.Agility));
        Debug.Log($"InitializeStartingHeroes: Added {this.heroes.Count} heroes");
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
    // ����� �� �ʱ� �κ��丮 ���� �����
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