using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    public HeroClass heroClass;
    public HeroInfo info;
    public override void SetStat()
    {
        base.SetStat(); // Character의 SetStat 호출
        if (info != null)
        {
            // HeroInfo에서 추가적인 정보를 가져올 수 있습니다.
            // 예: 특별한 능력치나 HeroInfo에만 있는 정보
            maxHealth = info.hp;
            attackDamage = info.attackDamage;
            attackSpeed = info.attackSpeed;
            attackRange = info.attackRange;
        }
        UpdateStatsFromPlayerStat();
    }

    protected override void Start()
    {
        base.Start();
        SetStat();
    }
}
