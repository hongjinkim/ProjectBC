using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public List<Basic> basic;
    public List<Magic> magic;
    

    public Stat()
    {
    }

    public Stat(List<Basic> basic)
    {
        this.basic = basic;
        this.magic = new List<Magic>();
    }

}
[Serializable]
public struct Basic
{
    public BasicStat id;
    public string name
    {
        get
        {
            switch(id)
            {
                case BasicStat.Agillity:
                    return "민첩";
                case BasicStat.AttackPower:
                    return "공격력";
                case BasicStat.Defense:
                    return "방어력";
                case BasicStat.Health:
                    return "체력";
                case BasicStat.Intelligence:
                    return "지능";
                case BasicStat.Strength:
                    return "힘";
                case BasicStat.MagicResistance:
                    return "마법저항력";
                default:
                    return "";
            }
        }
    }
    public int value;

    public int minValue;
    public int maxValue;
}
[Serializable]
public struct Magic
{
    public MagicStat id;
    public string name
    {
        get
        {
            switch (id)
            {
                case MagicStat.AttackPower:
                    return "공격력";
                case MagicStat.Strength:
                    return "힘";
                case MagicStat.Agillity:
                    return "민첩";
                case MagicStat.Intelligence:
                    return "지능";
                case MagicStat.Defense:
                    return "방어력";
                case MagicStat.MagicResistance:
                    return "마법저항력";
                case MagicStat.Health:
                    return "체력";
                case MagicStat.DamageBlock:
                    return "데미지 방어";
                case MagicStat.HealthRegeneration:
                    return "체력 재생";
                case MagicStat.EnergyRegeneration:
                    return "에너지 재생";
                case MagicStat.AttackSpeed:
                    return "공격속도";
                case MagicStat.TrueDamage:
                    return "고정데미지";
                default:
                    return "";
            }
        }
    }
    public int value;

    //public int minValue;
    //public int maxValue;
}