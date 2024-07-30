using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkills : HeroSkills
{
    public ArcherSkills(PlayerStat stat) : base(stat) { }

    protected override void InitializeSkills()
    {
        activeSkill = new PierceShot();
        // 여기에 패시브 스킬들을 추가할 수 있습니다.
    }
}
