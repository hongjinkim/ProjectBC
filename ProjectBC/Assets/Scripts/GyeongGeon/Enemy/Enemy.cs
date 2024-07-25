using System;
using UnityEngine;


public class Enemy : Character
{
    public int expToGive;

    public override void Die()
    {
        attacker.info.AddExp(expToGive);
        base.Die();
        dungeon.GetDroppedItem(this.transform);
    }
}
