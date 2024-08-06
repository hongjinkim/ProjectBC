using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    private const float MAX_ENERGY = 100f;

    private const float REGEN_INTERVAL = 1f;
    private const float REGEN_PERCENT = 0.05f;
    private bool isRegenerating = false;

    protected virtual void OnEnable()
    {
        InstantFadeIn();
        if (isRegenerating)
        {
            CancelInvoke("RegenerateHealth");
            isRegenerating = false;
        }
    }


    protected virtual void OnDisable()
    {
        if (!isRegenerating)
        {
            isRegenerating = true;
            InvokeRepeating("RegenerateHealth", 0f, REGEN_INTERVAL);
        }

        if (_unitState == Character.UnitState.death)
        {
            Revive();
        }
    }

    protected override void Start()
    {
        base.Start();
        SetStat();
        StartCoroutine(RegenerateEnergy());
    }

    private IEnumerator RegenerateEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (info != null)
            {
                info.energy = Mathf.Min(info.energy + info.energyRegen, MAX_ENERGY);
            }
        }
    }

    public float Energy
    {
        get { return info != null ? info.energy : 0; }
        set
        {
            if (info != null)
            {
                info.energy = Mathf.Clamp(value, 0, MAX_ENERGY);
            }
        }
    }
    public void SetStat()
    {
        if(info != null)
        {
            maxHealth = info.hp;
            currentHealth = maxHealth;
            attackDamage = info.attackDamage;
            //attackSpeed = info.attackSpeed;
            

            // 새로 추가된 스탯들
            energy = info.energy;
            strength = info.strength;
            agility = info.agility;
            intelligence = info.intelligence;
            stamina = info.stamina;
            defense = info.defense;
            magicResistance = info.magicResistance;
            healthRegen = info.healthRegen;
            energyRegen = info.energyRegen;
            expAmplification = info.expAmplification;
            trueDamage = info.trueDamage;
            damageBlock = info.damageBlock;
            lifeSteal = info.lifeSteal;
            damageAmplification = info.damageAmplification;
            damageReduction = info.damageReduction;
            criticalChance = info.criticalChance;
            criticalDamage = info.criticalDamage;
            defensePenetration = info.defensePenetration;

        }
    }
    protected void IncreaseStrength(float amount)
    {
        info.strength += (int)amount;
        info.healthRegen += (int)(0.1f * amount);
        info.hp += (int)(1f * amount);

        if (info.characteristicType == CharacteristicType.MuscularStrength)
        {
            info.attackDamage += (int)(0.7f * amount);
        }
        SetStat();
    }

    protected void IncreaseAgility(float amount)
    {
        info.agility += (int)amount;
        info.attackSpeed += (int)(0.1f * amount);
        info.defense += (int)(0.1f * amount);

        if (info.characteristicType == CharacteristicType.Agility)
        {
            info.attackDamage += (int)(0.9f * amount);
        }
        SetStat();
    }

    protected void IncreaseIntelligence(float amount)
    {
        info.intelligence += (int)amount;
        info.energyRegen += (int)(0.1f * amount);
        info.magicResistance += (int)(0.1f * amount);

        if (info.characteristicType == CharacteristicType.Intellect)
        {
            info.attackDamage += (int)(0.9f * amount);
        }
        SetStat();
    }

    protected void IncreaseStamina(float amount)
    {
        info.stamina += (int)amount;
        info.hp += (int)(10f * amount);
        SetStat();
    }

    private void RegenerateHealth()
    {
        if (!gameObject.activeInHierarchy && currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + (maxHealth * REGEN_PERCENT), maxHealth);
            if (currentHealth >= maxHealth)
            {
                CancelInvoke("RegenerateHealth");
                isRegenerating = false;
            }
        }
    }
}
