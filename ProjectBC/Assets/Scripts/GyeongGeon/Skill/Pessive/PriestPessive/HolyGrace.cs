using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyGrace : PlayerSkill
{
    private int[] defenseBonus = { 2, 4, 6, 8, 10 };
    private int[] magicResistanceBonus = { 2, 4, 6, 8, 10 };

    public HolyGrace() : base(
        "신성한 은혜",
        "사제의 빛은 근처 영웅들을 강화시킵니다.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    { }

    public int GetDefenseBonus() => defenseBonus[Level - 1];
    public int GetMagicResistanceBonus() => magicResistanceBonus[Level - 1];

    public override void UseSkill(Hero caster) { }
}
