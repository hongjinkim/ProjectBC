using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkillBook
{
    public PlayerSkill PiercingArrow { get; private set; }

    public ArcherSkillBook()
    {
        PiercingArrow = new PlayerSkill("�����ϴ� ȭ��",
                                  "�� �������� �������� �ڵ������� �����˴ϴ�.",
                                  5,
                                  new int[] { 12, 24, 36, 48, 60 },
                                  new float[] { 1.2f, 1.4f, 1.6f, 1.8f, 2.0f });
    }

    public void LevelUpSkill(PlayerSkill skill)
    {
        skill.LevelUp();
    }
}