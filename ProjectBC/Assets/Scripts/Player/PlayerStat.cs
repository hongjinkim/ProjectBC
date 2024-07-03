using System;
using System.Collections;
using UnityEngine;

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
    private int _fixedDamage; // �߰� ���� ���� �߰�

    public int FixedDamage // �߰� ���� ���� �Ӽ� �߰�
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
    public int Energy // ������ �Ӽ� �߰�
    {
        get { return _energy; }
        set { _energy = value; }
    }
    private void Start()
    {
        // �ʱⰪ ����
        _energy = 100; // �⺻ ������ ����
        _energyRegen = 5; // ������ ��� �ӵ� ����
        StartCoroutine(RegenerateEnergy());
    }

    private IEnumerator RegenerateEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _energy = Mathf.Min(_energy + _energyRegen, 100); // �ִ� �������� 100���� ����
        }
    }
}