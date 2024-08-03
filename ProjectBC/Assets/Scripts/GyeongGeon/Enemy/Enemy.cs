using System;
using UnityEngine;


public class Enemy : Character
{
    public int expToGive;
    
    protected override void Start()
    {
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
        //currentHealth = info.hp;
        info.attackRange = 1;
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
