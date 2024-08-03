using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public string imagePath;

    // 기본 스탯
    public int strength;
    public int agility;
    public int intelligence;
    public int stamina;

    // 추가 스탯
    public float energy;
    public int hp;
    public int attackDamage;
    public int defense;
    public int magicResistance;
    public float attackSpeed;
    public float healthRegen;
    public float energyRegen;
    public int attackRange;
    public int expAmplification;
    public int trueDamage;
    public int damageBlock;
    public int lifeSteal;
    public int damageAmplification;
    public int damageReduction;
    public int criticalChance;
    public int criticalDamage;
    public int defensePenetration;
    public string spritePath;
    public List<Trait> traits = new List<Trait>();
    public List<PlayerSkill> skills = new List<PlayerSkill>();
    public PlayerSkill activeSkill;
    private Sprite _sprite;

    public HeroPage heroPage;

    public event Action OnExperienceChanged;
    public event Action OnLevelUp;
    public HeroInfo(string name, HeroClass heroClass, int id, string imagePath)
    {
        this.id = id;
        this.imagePath = imagePath;
        this.heroName = name;
        this.heroClass = heroClass;
        this.level = 1;
        this.currentExp = 0;
        this.neededExp = 2;

        // 공통 기본 스탯 설정
        this.energy = 0;
        this.strength = 0;
        this.agility = 0;
        this.intelligence = 0;
        this.stamina = 0;
        this.hp = 200;
        this.attackDamage = 10;
        this.defense = 10;
        this.magicResistance = 10;
        //this.attackSpeed = 100;
        this.healthRegen = 0;
        this.energyRegen = 5;
        this.expAmplification = 0;
        this.trueDamage = 0;
        this.damageBlock = 0;
        this.lifeSteal = 0;
        this.damageAmplification = 0;
        this.damageReduction = 0;
        this.criticalChance = 0;
        this.criticalDamage = 150;
        this.defensePenetration = 0;
        // attackRange와 characteristicType은 여기서 설정하지 않음
    }

    // 이미지를 로드하는 메서드
    //public Sprite LoadImage()
    //{
    //    return Resources.Load<Sprite>(imagePath);
    //}
    public Sprite Sprite
    {
        get
        {
            if (_sprite == null)
            {
                _sprite = Resources.Load<Sprite>(imagePath);
                if (_sprite != null)
                {
                    return _sprite;
                }
            }
            return _sprite;
        }
    }

    public void IncreaseStrength(float amount)
    {
        strength += (int)amount;
        healthRegen += (int)(0.1f * amount);
        hp += (int)(1f * amount);

        if (characteristicType == CharacteristicType.MuscularStrength)
        {
            attackDamage += (int)(0.7f * amount);
        }
    }

    public void IncreaseAgility(float amount)
    {
        agility += (int)amount;
        attackSpeed += (int)(0.1f * amount);
        defense += (int)(0.1f * amount);

        if (characteristicType == CharacteristicType.Agility)
        {
            attackDamage += (int)(0.9f * amount);
        }
    }

    public void IncreaseIntelligence(float amount)
    {
        intelligence += (int)amount;
        energyRegen += (int)(0.1f * amount);
        magicResistance += (int)(0.1f * amount);

        if (characteristicType == CharacteristicType.Intellect)
        {
            attackDamage += (int)(0.9f * amount);
        }
    }

    public void IncreaseStamina(float amount)
    {
        stamina += (int)amount;
        hp += (int)(10f * amount);
    }





    public void AddExp(float exp)
    {
        if (level >= 40 && currentExp >= neededExp)
        {
            return;
        }
        
        currentExp = Mathf.Min(currentExp + exp, neededExp);

        OnExperienceChanged?.Invoke();

        if (level < 40 && currentExp >= neededExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        currentExp -= neededExp;
        neededExp *= 1.2f;
        switch (characteristicType)
        {
            case CharacteristicType.Agility:
                IncreaseAgility(2);
                break;
            case CharacteristicType.MuscularStrength:
                IncreaseStrength(3);
                break;
            case CharacteristicType.Intellect:
                IncreaseIntelligence(2);
                break;
        }
        OnLevelUp?.Invoke();
    }

    public void TriggerExperienceChanged()
    {
        OnExperienceChanged?.Invoke();
    }

    public void TriggerLevelUp()
    {
        OnLevelUp?.Invoke();
    }

    public void AddTrait(Trait trait)
    {
        traits.Add(trait);
        // 특성 추가에 따른 스탯 재계산
        
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
