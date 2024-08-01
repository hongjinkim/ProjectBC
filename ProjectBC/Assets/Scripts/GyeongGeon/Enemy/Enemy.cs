using System;
using UnityEngine;


public class Enemy : Character
{
    public int expToGive;

    public override void Die()
    {
        try
        {
            if (attacker != null && attacker.info != null)
            {
                attacker.info.AddExp(expToGive);
            }
            else
            {
                Debug.LogWarning("Attacker or attacker info is null in Enemy.Die()");
            }

            base.Die();

            if (dungeon != null)
            {
                dungeon.GetDroppedItem(this.transform);
            }
            else
            {
                Debug.LogWarning("Dungeon is null in Enemy.Die()");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in Enemy.Die(): {e.Message}");
        }
    }
}
