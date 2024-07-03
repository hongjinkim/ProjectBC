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

    // ����� �� �ʱ� �κ��丮 ���� �����
    private void InitInventory()
    {
        this.InventoryEquipable = new List<Equipable>()
        {
            new Equipable(0, "���1", Rarity.Common, "�׽�Ʈ ��� 1 �Դϴ�."),
            new Equipable(1, "���2", Rarity.Uncommon, "�׽�Ʈ ��� 2 �Դϴ�."),
            new Equipable(2, "���3", Rarity.Rare, "�׽�Ʈ ��� 3 �Դϴ�.")
        };
        this.InventoryUsable = new List<Usable>()
        {
            new Usable(0, "�Ҹ�ǰ1", Rarity.Common, "�׽�Ʈ �Ҹ�ǰ 1 �Դϴ�."),
            new Usable(1, "�Ҹ�ǰ2", Rarity.Uncommon, "�׽�Ʈ �Ҹ�ǰ 2 �Դϴ�."),
            new Usable(2, "�Ҹ�ǰ3", Rarity.Rare, "�׽�Ʈ �Ҹ�ǰ 3 �Դϴ�.")
        };
        this.InventoryMaterial = new List<Material>()
        {
            new Material(0, "���1", Rarity.Common, "�׽�Ʈ ��� 1 �Դϴ�."),
            new Material(1, "���2", Rarity.Uncommon, "�׽�Ʈ ��� 2 �Դϴ�."),
            new Material(2, "���3", Rarity.Rare, "�׽�Ʈ ��� 3 �Դϴ�.")
        };
        this.InventoryCrystal = new List<Crystal>()
        {
            new Crystal(0, "ũ����Ż1", Rarity.Common, "�׽�Ʈ ũ����Ż 1 �Դϴ�."),
            new Crystal(1, "ũ����Ż2", Rarity.Uncommon, "�׽�Ʈ ũ����Ż 2 �Դϴ�."),
            new Crystal(2, "ũ����Ż3", Rarity.Rare, "�׽�Ʈ ũ����Ż 3 �Դϴ�.")
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


