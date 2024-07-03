using System.Collections;
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
    public List<Equipable> InventoryEquipable;
    public List<Usable> InventoryUsable;
    public List<Material> InventoryMaterial;
    public List<Crystal> InventoryCrystal;

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

        InitInventory();
    }

    // 디버깅 용 초기 인벤토리 상태 만들기
    private void InitInventory()
    {
        this.InventoryEquipable = new List<Equipable>()
        {
            new Equipable(0, "장비1", Rarity.Common, "테스트 장비 1 입니다."),
            new Equipable(1, "장비2", Rarity.Uncommon, "테스트 장비 2 입니다."),
            new Equipable(2, "장비3", Rarity.Rare, "테스트 장비 3 입니다.")
        };
        this.InventoryUsable = new List<Usable>()
        {
            new Usable(0, "소모품1", Rarity.Common, "테스트 소모품 1 입니다."),
            new Usable(1, "소모품2", Rarity.Uncommon, "테스트 소모품 2 입니다."),
            new Usable(2, "소모품3", Rarity.Rare, "테스트 소모품 3 입니다.")
        };
        this.InventoryMaterial = new List<Material>()
        {
            new Material(0, "재료1", Rarity.Common, "테스트 재료 1 입니다."),
            new Material(1, "재료2", Rarity.Uncommon, "테스트 재료 2 입니다."),
            new Material(2, "재료3", Rarity.Rare, "테스트 재료 3 입니다.")
        };
        this.InventoryCrystal = new List<Crystal>()
        {
            new Crystal(0, "크리스탈1", Rarity.Common, "테스트 크리스탈 1 입니다."),
            new Crystal(1, "크리스탈2", Rarity.Uncommon, "테스트 크리스탈 2 입니다."),
            new Crystal(2, "크리스탈3", Rarity.Rare, "테스트 크리스탈 3 입니다.")
        };
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


