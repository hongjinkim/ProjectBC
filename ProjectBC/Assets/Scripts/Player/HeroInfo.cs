using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GameDataManager;
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
    public int hp;
    public int attackDamage;
    public int defense;
    public int magicResistance;
    public float attackSpeed;
    public float healthRegen;
    public float energyRegen;
    public int attackRange;
    public string spritePath;
    public List<Trait> traits = new List<Trait>();
    public List<PlayerSkill> skills = new List<PlayerSkill>();
    public PlayerSkill activeSkill;
    private Sprite _sprite;
    public PlayerStat playerStat;
    public HeroPage heroPage;

    public event Action OnExperienceChanged;
    public event Action OnLevelUp;
    public HeroInfo(string name, HeroClass heroClass, CharacteristicType characteristicType, int id, string imagePath)
    {
        this.id = id;
        this.imagePath = imagePath; // 이미지 경로 저장
        this.heroName = name;
        this.heroClass = heroClass;
        this.characteristicType = characteristicType;
        this.level = 1;
        this.currentExp = 0;
        this.neededExp = 2;

        // CharacterBaseData에서 초기 스탯 로드
        CharacterBaseData baseData = GameDataManager.instance.characterBaseData.FirstOrDefault(d => d.name.ToLower() == heroClass.ToString().ToLower());
        if (GameDataManager.instance != null && GameDataManager.instance.characterBaseData != null)
        {
            this.hp = baseData.hp;
            this.attackDamage = baseData.attackDamage;
            this.defense = baseData.defense;
            this.magicResistance = baseData.magicResistance;
            this.strength = baseData.strength;
            this.agility = baseData.agility;
            this.intelligence = baseData.intelligence;
            this.stamina = baseData.stamina;
            // ... 다른 스탯들 설정 ...
        }
        else
        {
            // baseData가 null인 경우 기본값 설정
            this.hp = 200;
            this.attackDamage = 10;
            this.strength = 10;
            this.agility = 10;
            this.intelligence = 10;
            this.stamina = 10;
            // ... 다른 기본 스탯들 설정 ...
        }
        // ... 기타 스탯 초기화
        // 추가 스탯 초기 계산
        RecalculateStats();

        // PlayerStat 초기화
        playerStat = new PlayerStat();
        InitializePlayerStat();

        // 스킬 초기화 (예시)
        //InitializeSkills();
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
    public HeroInfo(HeroInfo other)
    {
        this.id = other.id;
        this.heroName = other.heroName;
        this.level = other.level;
        this.attackDamage = other.attackDamage;
        this.agility = other.agility;
        this.hp = other.hp;
        this.spritePath = other.spritePath;
        // ... 다른 속성들도 복사 ...
    }
    //public static HeroInfo CreateNewHero(string name, HeroClass heroClass, CharacteristicType characteristicType)
    //{
    //    return new HeroInfo
    //    {
    //        heroName = name,
    //        heroClass = heroClass,
    //        characteristicType = characteristicType,
    //        level = 1,
    //        currentExp = 0,
    //        neededExp = 100,
    //        // 기본 스탯 설정
    //        strength = 10,
    //        agility = 10,
    //        intelligence = 10,
    //        stamina = 10,
    //        // 추가 스탯 계산
    //        hp = 200,
    //        attackDamage = 10,
    //        // ... 기타 스탯 초기화
    //    };
    //}
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
    private void InitializePlayerStat()
    {
        playerStat.HP = hp;
        playerStat.AttackDamage = attackDamage;
        playerStat.Defense = defense;
        playerStat.MagicResistance = magicResistance;
        playerStat.AttackSpeed = (int)attackSpeed;
        playerStat.HealthRegen = (int)healthRegen;
        playerStat.EnergyRegen = (int)energyRegen;
        playerStat.AttackRange = attackRange;
        playerStat.Strength = strength;
        playerStat.Agility = agility;
        playerStat.Intelligence = intelligence;
        playerStat.Stamina = stamina;
        playerStat.CharacteristicType = characteristicType;
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
        IncreaseStats();
        OnLevelUp?.Invoke();
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
    //public void LevelUpPassiveSkill(int skillIndex)
    //{
    //    if (skillIndex >= 0 && skillIndex < skills.Count)
    //    {
    //        PlayerSkill skill = skills[skillIndex];
    //        if (skill.SkillType == SkillType.Passive)
    //        {
    //            skill.LevelUp();
    //            ApplyPassiveSkillEffect(skill);
    //        }
    //    }
    //}
    private void ApplyPassiveSkillEffect(PlayerSkill skill)
    {
        // 스킬 효과에 따라 스탯 업데이트 (예시)
        switch (skill.Name)
        {
            case "Iron Skin":
                defense += 5 * skill.Level;
                break;
            case "Swift Movement":
                attackSpeed += 5 * skill.Level;
                break;
                // 다른 패시브 스킬 효과들...
        }
        RecalculateStats();
        InitializePlayerStat(); // PlayerStat 갱신
    }
}
