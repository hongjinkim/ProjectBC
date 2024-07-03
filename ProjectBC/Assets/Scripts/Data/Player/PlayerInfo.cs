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
    public List<IItem> inventory;
    
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
        inventory = new List<IItem>();
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


