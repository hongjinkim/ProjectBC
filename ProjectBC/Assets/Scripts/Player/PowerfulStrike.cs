using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerfulStrike : ActiveSkill
{
    public PowerfulStrike() : base("Powerful Strike", "Deals physical damage to the target", 1, 5)
    {
        BaseDamage = new[] { 10, 20, 30, 40, 50 };
        DamageScaling = new[] { 1.15f, 1.3f, 1.45f, 1.6f, 1.75f };
    }

    public override void ApplyEffect(PlayerStat playerStat)
    {
        // 이 메서드는 패시브 스킬에 사용되므로 액티브 스킬에서는 비워둡니다.
    }

    public override void Use(PlayerStat playerStat)
    {
        Character character = playerStat.GetComponent<Character>();
        if (character != null)
        {
            float damage = BaseDamage[Level - 1] + playerStat.AttackDamage * DamageScaling[Level - 1];

            // Character 클래스의 메서드를 사용하여 가장 가까운 적을 찾습니다
            Character target = character.dungeon.GetTarget(character);

            if (target != null)
            {
                // Character 클래스의 TakeDamage 메서드를 사용하여 데미지를 입힙니다
                character.TakeDamage(target, damage);
                Debug.Log($"Knight used Powerful Strike on {target.name} for {damage} damage!");
            }
            else
            {
                Debug.Log("No target found for Powerful Strike!");
            }
        }
        else
        {
            Debug.LogError("PlayerStat is not attached to a Character!");
        }
    }

    private int[] BaseDamage { get; }
    private float[] DamageScaling { get; }
}