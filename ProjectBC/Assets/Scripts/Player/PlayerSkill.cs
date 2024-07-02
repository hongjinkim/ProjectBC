using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{


    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Level { get; private set; }
    public int MaxLevel { get; private set; }
    public int[] BaseDamage { get; private set; }
    public float[] Coefficients { get; private set; }

    public PlayerSkill(string name, string description, int maxLevel, int[] baseDamage, float[] coefficients)
    {
        Name = name;
        Description = description;
        MaxLevel = maxLevel;
        BaseDamage = baseDamage;
        Coefficients = coefficients;
        Level = 1; // 기본 레벨은 1
    }

    public void LevelUp()
    {
        if (Level < MaxLevel)
        {
            Level++;
        }
        else
        {
            Debug.Log("이미 최대 레벨입니다.");
        }
    }

    public int GetDamage(int attackDamage)
    {
        if (Level > 0 && Level <= MaxLevel)
        {
            return BaseDamage[Level - 1] + (int)(attackDamage * Coefficients[Level - 1]);
        }
        return 0;
    }
}

