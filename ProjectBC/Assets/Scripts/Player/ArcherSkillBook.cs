using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkillBook
{
    public PlayerSkill PiercingArrow { get; private set; }

    public ArcherSkillBook()
    {
        PiercingArrow = new PlayerSkill("관통하는 화살",
                                  "온 에너지가 가득차면 자동적으로 시전됩니다.",
                                  5,
                                  new int[] { 12, 24, 36, 48, 60 },
                                  new float[] { 1.2f, 1.4f, 1.6f, 1.8f, 2.0f });
    }

    public void LevelUpSkill(PlayerSkill skill)
    {
        skill.LevelUp();
    }
}