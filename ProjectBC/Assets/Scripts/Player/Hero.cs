using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    public HeroClass heroClass;
    public HeroInfo info;
    public override void SetStat()
    {
        base.SetStat(); // Character�� SetStat ȣ��
        if (info != null)
        {
            // HeroInfo���� �߰����� ������ ������ �� �ֽ��ϴ�.
            // ��: Ư���� �ɷ�ġ�� HeroInfo���� �ִ� ����
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
