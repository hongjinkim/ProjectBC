using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static GameDataManager;
[Serializable]
public class PlayerStat : MonoBehaviour
{
    public CharacteristicType CharacteristicType;
    public JobType JobType;

    private int _hp;
    private int _attackDamage;
    private int _defense;
    private int _magicResistance;
    private int _strength;
    private int _agility;
    private int _intelligence;
    private int _stamina;
    private int _attackSpeed;
    private int _healthRegen;
    private int _energyRegen;
    private int _attackRange;
    private int _expAmplification;
    private int _trueDamage;
    private int _energy;
    private int _fixedDamage; // 추가 고정 피해 추가
    public HeroClass heroClass;
    public int FixedDamage // 추가 고정 피해 속성 추가
    {
        get { return _fixedDamage; }
        set { _fixedDamage = value; }
    }
    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }

    public int AttackDamage
    {
        get { return _attackDamage; }
        set { _attackDamage = value; }
    }

    public int Defense
    {
        get { return _defense; }
        set { _defense = value; }
    }

    public int MagicResistance
    {
        get { return _magicResistance; }
        set { _magicResistance = value; }
    }

    public int Strength
    {
        get { return _strength; }
        set { _strength = value; }
    }

    public int Agility
    {
        get { return _agility; }
        set { _agility = value; }
    }

    public int Intelligence
    {
        get { return _intelligence; }
        set { _intelligence = value; }
    }

    public int Stamina
    {
        get { return _stamina; }
        set { _stamina = value; }
    }

    public int AttackSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }

    public int HealthRegen
    {
        get { return _healthRegen; }
        set { _healthRegen = value; }
    }

    public int EnergyRegen
    {
        get { return _energyRegen; }
        set { _energyRegen = value; }
    }

    public int AttackRange
    {
        get { return _attackRange; }
        set { _attackRange = value; }
    }

    public int ExpAmplification
    {
        get { return _expAmplification; }
        set { _expAmplification = value; }
    }

    public int TrueDamage
    {
        get { return _trueDamage; }
        set { _trueDamage = value; }
    }
    public int Energy // 에너지 속성 추가
    {
        get { return _energy; }
        set { _energy = value; }
    }
    private void Start()
    {
        InitializeStats();
        StartCoroutine(RegenerateEnergy());
    }
    public void InitializeStats()
    {
        if (GameDataManager.instance != null)
        {
            CharacterBaseData baseData = GameDataManager.instance.characterBaseData
                .FirstOrDefault(d => d.name.ToLower() == heroClass.ToString().ToLower());

            if (baseData != null)
            {
                InitializeFromBaseData(baseData);
            }
            else
            {
                Debug.LogError($"CharacterBaseData not found for {heroClass}");
                // 기본값으로 초기화하는 로직 추가
            }
        }
        else
        {
            Debug.LogError("GameDataManager instance is null");
            // 기본값으로 초기화하는 로직 추가
        }
    }
    public void InitializeFromBaseData(GameDataManager.CharacterBaseData baseData)
    {
        HP = baseData.hp;
        AttackDamage = baseData.attackDamage;
        Defense = baseData.defense;
        MagicResistance = baseData.magicResistance;
        Strength = baseData.strength;
        Agility = baseData.agility;
        Intelligence = baseData.intelligence;
        Stamina = baseData.stamina;
        AttackSpeed = baseData.attackSpeed;
        HealthRegen = baseData.healthRegen;
        EnergyRegen = baseData.energyRegen;
        AttackRange = baseData.attackRange;
        ExpAmplification = baseData.expAmplification;
        TrueDamage = baseData.trueDamage;
        Energy = 100; // 이 값은 baseData에 없으므로 기본값 사용
        FixedDamage = 0; // 이 값도 baseData에 없으므로 기본값 사용

        // 캐릭터 타입에 따른 추가 초기화 로직
        ApplyCharacteristicBonus();
    }

    private void ApplyCharacteristicBonus()
    {
        switch (CharacteristicType)
        {
            case CharacteristicType.MuscularStrength:
                Strength += 5;
                break;
            case CharacteristicType.Agility:
                Agility += 5;
                break;
            case CharacteristicType.Intellect:
                Intelligence += 5;
                break;
        }
    }

    private IEnumerator RegenerateEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _energy = Mathf.Min(_energy + _energyRegen, 100); // 최대 에너지는 100으로 제한
        }
    }
}