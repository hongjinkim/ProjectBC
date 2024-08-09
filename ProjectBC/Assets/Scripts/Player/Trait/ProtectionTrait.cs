using System;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionTrait : Trait
{
    private Dictionary<int, (string leftName, Action<Character> leftEffect, string rightName, Action<Character> rightEffect)> traitLevels;

    public ProtectionTrait() : base(TraitType.Protection, "��ȣ", "��� �ɷ��� ����ŵ�ϴ�.", 1, true)
    {
        InitializeTraitLevels();
    }

    private void InitializeTraitLevels()
    {
        traitLevels = new Dictionary<int, (string, Action<Character>, string, Action<Character>)>
        {
            {10, ("�Ŵ��� ����", ApplyGiantLineage, "��ȭ�� �Ǻ�", ApplyReinforcedSkin)},
            {20, ("������ Ȱ��", ApplyInfiniteVitality, "������", ApplyToughness)},
            {30, ("�ı��Ұ�", ApplyIndestructible, "���� ����", ApplyMagicResistance)},
            {40, ("���� ���", ApplyShieldUp, "�߰��� ����", ApplySolidSupport)}
        };
    }

    public override void ApplyEffect(Character character)
    {
        if (traitLevels.TryGetValue(Level, out var traitEffect))
        {
            if (IsLeftTrait)
                traitEffect.leftEffect(character);
            else
                traitEffect.rightEffect(character);
        }
    }

    private void ApplyGiantLineage(Character character)
    {
        character.maxHealth = Mathf.RoundToInt(character.maxHealth * 1.03f);
        character.currentHealth = character.maxHealth;
    }

    private void ApplyReinforcedSkin(Character character)
    {
        character.info.defense = Mathf.RoundToInt(character.info.defense * 1.04f);
    }

    private void ApplyInfiniteVitality(Character character)
    {
        character.info.defense += Mathf.RoundToInt(character.info.strength * 0.1f);
    }

    private void ApplyToughness(Character character)
    {
        character.info.magicResistance += Mathf.RoundToInt(character.info.strength * 0.1f);
    }

    private void ApplyIndestructible(Character character)
    {
        // TODO: Implement physical damage increase logic
        // character.OnTakeDamage += (damage, damageType) =>
        // {
        //     if (damageType == DamageType.Physical)
        //     {
        //         return damage * 1.05f;
        //     }
        //     return damage;
        // };
    }

    private void ApplyMagicResistance(Character character)
    {
        // TODO: Implement magic damage reduction logic
        // character.OnTakeDamage += (damage, damageType) =>
        // {
        //     if (damageType == DamageType.Magical)
        //     {
        //         return damage * 0.95f;
        //     }
        //     return damage;
        // };
    }

    private void ApplyShieldUp(Character character)
    {
        character.OnTakeDamage += (attacker, damage) =>
        {
            if (attacker.info.attackRange == 1)
            {
                return damage * 0.92f;
            }
            return damage;
        };
    }

    private void ApplySolidSupport(Character character)
    {
        character.OnTakeDamage += (attacker, damage) =>
        {
            if (attacker.info.attackRange == 4)
            {
                return damage * 0.92f;
            }
            return damage;
        };
    }
}