using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroInfo
{
    public int id; // 추가
    public List<string> equippedItemIds = new List<string>(); // 추가
    public string heroName;
    public HeroClass heroClass;
    public CharacteristicType characteristicType;
    public int level;
    public float currentExp;
    public float neededExp;

    // 기본 스탯
    public int strength;
    public int agility;
    public int intelligence;
    public int stamina;

    // 추가 스탯
    public int hp;
    public int attackDamage;
    public int defense;
    public int magicResistance;
    public float attackSpeed;
    public float healthRegen;
    public float energyRegen;
    public int attackRange;

    public List<Trait> traits = new List<Trait>();
    public List<PlayerSkill> skills = new List<PlayerSkill>();
    public PlayerSkill activeSkill;

    public HeroInfo(string name, HeroClass heroClass, CharacteristicType characteristicType)
    {
        this.heroName = name;
        this.heroClass = heroClass;
        this.characteristicType = characteristicType;
        this.level = 1;
        this.currentExp = 0;
        this.neededExp = 100;
        InitializeBaseStats();
    }

    private void InitializeBaseStats()
    {
        // 1레벨 히어로 기본 스탯 설정
        strength = 10;
        agility = 10;
        intelligence = 10;
        stamina = 10;

        // 추가 스탯 계산
        RecalculateStats();
    }

    public void RecalculateStats()
    {
        hp = 200 + (stamina * 10);
        attackDamage = 10;
        defense = 0;
        magicResistance = 0;
        attackSpeed = 100;
        healthRegen = 0;
        energyRegen = 0;
        attackRange = 0;

        // 특성에 따른 추가 계산
        ApplyCharacteristicBonuses();
    }

    private void ApplyCharacteristicBonuses()
    {
        switch (characteristicType)
        {
            case CharacteristicType.MuscularStrength:
                attackDamage += (int)(strength * 0.7f);
                break;
            case CharacteristicType.Agility:
                attackDamage += (int)(agility * 0.9f);
                break;
            case CharacteristicType.Intellect:
                attackDamage += (int)(intelligence * 0.9f);
                break;
        }
    }

    public void IncreaseStrength(int amount)
    {
        strength += amount;
        healthRegen += 0.1f * amount;
        hp += 1 * amount;

        if (characteristicType == CharacteristicType.MuscularStrength)
        {
            attackDamage += (int)(0.7f * amount);
        }
    }

    public void IncreaseAgility(int amount)
    {
        agility += amount;
        attackSpeed += 0.1f * amount;
        defense += (int)(0.1f * amount);

        if (characteristicType == CharacteristicType.Agility)
        {
            attackDamage += (int)(0.9f * amount);
        }
    }

    public void IncreaseIntelligence(int amount)
    {
        intelligence += amount;
        energyRegen += 0.1f * amount;
        magicResistance += (int)(0.1f * amount);

        if (characteristicType == CharacteristicType.Intellect)
        {
            attackDamage += (int)(0.9f * amount);
        }
    }

    public void IncreaseStamina(int amount)
    {
        stamina += amount;
        hp += 10 * amount;
    }

    public void AddExp(float exp)
    {
        currentExp += exp;
        while (currentExp >= neededExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        currentExp -= neededExp;
        neededExp *= 1.2f;
        IncreaseStats();
    }

    private void IncreaseStats()
    {
        // 레벨업 시 스탯 증가 로직
        // 이 부분은 게임 밸런스에 따라 조정이 필요합니다
        IncreaseStrength(1);
        IncreaseAgility(1);
        IncreaseIntelligence(1);
        IncreaseStamina(1);
    }

    public void AddTrait(Trait trait)
    {
        traits.Add(trait);
        // 특성 추가에 따른 스탯 재계산
        RecalculateStats();
    }

    public void AddSkill(PlayerSkill skill)
    {
        skills.Add(skill);
    }

    public void SetActiveSkill(PlayerSkill skill)
    {
        activeSkill = skill;
    }
}
