using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerStat : MonoBehaviour
{
    public CharacteristicType CharacteristicType;

    public float healthRegen = 0f;
    public float hp = 0f;
    public float attackDamage = 0f;
    public float attackSpeed = 0f;
    public float defense = 0f;
    public float energyRegen = 0f;
    public float magicResistance = 0f;

    public float strength = 0f;
    public float agility = 0f;
    public float intelligence = 0f;
    public float stamina = 0f;

    public void IncreaseStrength(float amount)
    {
        strength += amount;
        healthRegen += 0.1f * amount;
        hp += 1f * amount;

        if (CharacteristicType == CharacteristicType.MuscularStrength)
        {
            attackDamage += 0.7f*amount;
        }
    }

    public void IncreaseAgility(float amount)
    {
        agility += amount;
        attackSpeed += 0.1f * amount;
        defense += 0.1f * amount;

        if (CharacteristicType == CharacteristicType.Agility)
        {
            attackDamage += 0.9f * amount;
        }
    }
    public void IncreaseIntelligence(float amount)
    {
        intelligence += amount;
        energyRegen += 0.1f;
        magicResistance += 0.1f;
        if (CharacteristicType == CharacteristicType.Intellect)
        {
            attackDamage += 0.9f * amount;
        }

    }

    public void LevelUp()
    {

    }
}
