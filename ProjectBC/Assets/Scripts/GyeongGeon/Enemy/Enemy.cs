using System;
using UnityEngine;


public class Enemy : Character
{
    public int expToGive;
    
    protected override void Start()
    {
        SetLayerToEnemy();
        InitializeStats();
        base.Start();
    }
    protected virtual void InitializeStats()
    {
        if (info == null)
        {
            info = new HeroInfo("Enemy", HeroClass.Knight, 0, "");
        }
        info.hp = 100;  // 기본 적 HP
        info.attackDamage = 1;  // 기본 적 공격력
        info.attackRange = 1;
        info.energy = 0;
        info.strength = 0;
        info.agility = 0;
        info.intelligence = 0;
        info.stamina = 0;
        info.hp = 200;
        info.attackDamage = 1;
        info.defense = 10;
        info.magicResistance = 10;
        //this.attackSpeed = 100;
        info.healthRegen = 0;
        info.energyRegen = 5;
        info.expAmplification = 0;
        info.trueDamage = 0;
        info.damageBlock = 0;
        info.lifeSteal = 0;
        info.damageAmplification = 0;
        info.damageReduction = 0;
        info.criticalChance = 0;
        info.criticalDamage = 150;
        info.defensePenetration = 0;

    }
    private void SetLayerToEnemy()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if (enemyLayer != -1)  // "Enemy" 레이어가 존재하는지 확인
        {
            gameObject.layer = enemyLayer;

        }

    }
    public override void TakeDamage(Character attacker, float damage)
    {
        
        base.TakeDamage(attacker, damage);
        
    }

    public override void Die()
    {
        if (attacker != null && attacker.info != null)
        {
            attacker.info.AddExp(expToGive);
        }
        base.Die();
        if (dungeon != null)
        {
            dungeon.GetDroppedItem(this.transform);
        }
    }
}
