using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkills : HeroSkills
{
    public ArcherSkills(PlayerStat stat) : base(stat) { }

    protected override void InitializeSkills()
    {
        activeSkill = new PierceShot();
        // ���⿡ �нú� ��ų���� �߰��� �� �ֽ��ϴ�.
    }
}
