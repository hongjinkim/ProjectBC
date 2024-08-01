using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GameDataManager;
[Serializable]
public class HeroInfo
{
    public int id; // �߰�
    public List<string> equippedItemIds = new List<string>(); // �߰�
    public string heroName;
    public HeroClass heroClass;
    public CharacteristicType characteristicType;
    public int level;
    public float currentExp;
    public float neededExp;
    public string imagePath;

    // �⺻ ����
    public int strength;
    public int agility;
    public int intelligence;
    public int stamina;

    // �߰� ����
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
        this.imagePath = imagePath; // �̹��� ��� ����
        this.heroName = name;
        this.heroClass = heroClass;
        this.characteristicType = characteristicType;
        this.level = 1;
        this.currentExp = 0;
        this.neededExp = 2;

        // CharacterBaseData���� �ʱ� ���� �ε�
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
            // ... �ٸ� ���ȵ� ���� ...
        }
        else
        {
            // baseData�� null�� ��� �⺻�� ����
            this.hp = 200;
            this.attackDamage = 10;
            this.strength = 10;
            this.agility = 10;
            this.intelligence = 10;
            this.stamina = 10;
            // ... �ٸ� �⺻ ���ȵ� ���� ...
        }
        // ... ��Ÿ ���� �ʱ�ȭ
        // �߰� ���� �ʱ� ���
        RecalculateStats();

        // PlayerStat �ʱ�ȭ
        playerStat = new PlayerStat();
        InitializePlayerStat();

        // ��ų �ʱ�ȭ (����)
        //InitializeSkills();
    }

    // �̹����� �ε��ϴ� �޼���
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
        // ... �ٸ� �Ӽ��鵵 ���� ...
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
    //        // �⺻ ���� ����
    //        strength = 10,
    //        agility = 10,
    //        intelligence = 10,
    //        stamina = 10,
    //        // �߰� ���� ���
    //        hp = 200,
    //        attackDamage = 10,
    //        // ... ��Ÿ ���� �ʱ�ȭ
    //    };
    //}
    private void InitializeBaseStats()
    {
        // 1���� ����� �⺻ ���� ����
        strength = 10;
        agility = 10;
        intelligence = 10;
        stamina = 10;

        // �߰� ���� ���
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

        // Ư���� ���� �߰� ���
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
        // ������ �� ���� ���� ����
        // �� �κ��� ���� �뷱���� ���� ������ �ʿ��մϴ�
        IncreaseStrength(1);
        IncreaseAgility(1);
        IncreaseIntelligence(1);
        IncreaseStamina(1);
    }

    public void AddTrait(Trait trait)
    {
        traits.Add(trait);
        // Ư�� �߰��� ���� ���� ����
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
        // ��ų ȿ���� ���� ���� ������Ʈ (����)
        switch (skill.Name)
        {
            case "Iron Skin":
                defense += 5 * skill.Level;
                break;
            case "Swift Movement":
                attackSpeed += 5 * skill.Level;
                break;
                // �ٸ� �нú� ��ų ȿ����...
        }
        RecalculateStats();
        InitializePlayerStat(); // PlayerStat ����
    }
}
