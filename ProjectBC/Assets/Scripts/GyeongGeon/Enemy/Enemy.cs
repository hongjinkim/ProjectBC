using System;
using UnityEngine;


public class Enemy : Character
{
    public override void Die()
    {
        base.Die();
        dungeon.GetDroppedItem(this.transform);
    }
}
