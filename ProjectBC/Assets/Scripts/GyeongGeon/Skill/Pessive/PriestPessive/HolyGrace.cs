using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyGrace : PlayerSkill
{
    private int[] defenseBonus = { 2, 4, 6, 8, 10 };
    private int[] magicResistanceBonus = { 2, 4, 6, 8, 10 };

    public HolyGrace() : base(
        "�ż��� ����",
        "������ ���� ��ó �������� ��ȭ��ŵ�ϴ�.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    { }

    public int GetDefenseBonus() => defenseBonus[Level - 1];
    public int GetMagicResistanceBonus() => magicResistanceBonus[Level - 1];

    public override void UseSkill(Hero caster) { }
}
