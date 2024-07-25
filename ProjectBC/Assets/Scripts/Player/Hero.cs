using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    public void SetStat()
    {
        if(info != null)
        {
            maxHealth = info.hp;
            attackDamage = info.attackDamage;
            attackSpeed = info.attackSpeed;
            attackRange = info.attackRange;
        }
    }
}
