using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

[Serializable]
public class HeroInfo
{
    public Character character;
    public int id; // �߰�
    public List<string> equippedItemIds = new List<string>(); // �߰�
    public string heroName;
    public HeroClass heroClass;
    public CharacteristicType characteristicType;
    public int level;
    public float currentExp;
    public float neededExp;
    public string imagePath;
    [JsonIgnore] public int battlePoint => CalculateBattlePoint();

    // �⺻ ����
    public int strength;
    public int agility;
    public int intelligence;
    public int stamina;

    // �߰� ����
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
    public event Action<int> OnTraitSelectionAvailable;

    //
    public int hpLevel = 1;
    public int strengthLevel = 1;
    public int defenseLevel = 1;
    public int masicResistanceLevel = 1;
    public HeroInfo(string name, HeroClass heroClass, int id, string imagePath)
    {
        this.id = id;
        this.imagePath = imagePath;
        this.heroName = name;
        this.heroClass = heroClass;
        this.level = 1;
        this.currentExp = 0;
        this.neededExp = 2;

        // ���� �⺻ ���� ����
        this.energy = 0;
        this.strength = 0;
        this.agility = 0;
        this.intelligence = 0;
        this.stamina = 0;
        this.hp = 200;
        this.attackDamage = 10;//��������
        this.defense = 10;//���
        this.magicResistance = 10;//���� ����
        this.attackSpeed = 100;//���ݼӵ�
        this.healthRegen = 0;//ü�����
        this.energyRegen = 5;//���������
        this.expAmplification = 0;//����ġ����
        this.trueDamage = 0;//��������
        this.damageBlock = 0;//���������� �ϴ� ����
        this.lifeSteal = 0;//��������
        this.damageAmplification = 0;//��������
        this.damageReduction = 0;//���ذ���
        this.criticalChance = 0;//ũ��Ƽ��Ȯ��
        this.criticalDamage = 150;//ũ��Ƽ�õ�����
        this.defensePenetration = 0;//������
        // attackRange�� characteristicType�� ���⼭ �������� ����
        InitializeTraits();
    }
    private void InitializeTraits()
    {
        switch (heroClass)
        {
            case HeroClass.Archer:
                traits.Add(new ConcentrationTrait());
                break;
            case HeroClass.Knight:
                traits.Add(new ProtectionTrait());
                break;
            case HeroClass.Wizard:
                traits.Add(new MagicTrait());
                break;
            case HeroClass.Priest:
                traits.Add(new ProtectionTrait());
                //traits.Add(new PlunderTrait()); ����
                break;
        }
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
    public void SetCharacter(Character character)
    {
        this.character = character;
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
        float amplifiedExp = exp * (1 + (expAmplification / 100f));
        currentExp = Mathf.Min(currentExp + amplifiedExp, neededExp);

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
        if (level == 10 || level == 20 || level == 30 || level == 40)
        {
            OnTraitSelectionAvailable?.Invoke(level);
        }
    }
    public void SelectTrait(int level, bool isLeftTrait)
    {
        foreach (var trait in traits)
        {
            if (trait.Level == level)
            {
                trait.ChooseTrait(level, isLeftTrait);
                break;
            }
        }
    }
    public void ApplyTraits(Character character)
    {
        foreach (var trait in traits)
        {
            trait.ApplyEffect(character);
        }
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
        // Ư�� �߰��� ���� ���� ����
        
    }

    public void AddSkill(PlayerSkill skill)
    {
        skills.Add(skill);
    }

    public void SetActiveSkill(PlayerSkill skill)
    {
        activeSkill = skill;
    }

    private int CalculateBattlePoint()
    {
        // �� ������ ���� �뷱���� ���� �����ؾ� �� �� �ֽ��ϴ�.
        return hp * 2 + attackDamage * 2 + defense * 3 + magicResistance * 3 + level * 5 + strength * 2 + intelligence * 2 + agility * 2 + damageBlock * 3;
    }
}
