using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public uint gold;
    public string username;
    public float musicVolume;
    public float sfxVolume;

    public PlayerInfo()
    {
        // player stats/data
        this.gold = 500;

        // ĳ����
        //public List<Character> characterList;
        // ������
        //public List<Item> itemList;
        //���
        //public List<Equipment> equipmentList
        // settings
        this.musicVolume = 80f;
        this.sfxVolume = 80f;

        this.username = "GUEST_123456";
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
