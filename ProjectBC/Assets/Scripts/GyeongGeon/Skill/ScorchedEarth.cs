using UnityEngine;
using System.Collections.Generic;

public class ScorchedEarth : PlayerSkill
{
    public float aoeRadius = 0.8f; // AOE 범위 반경

    public ScorchedEarth() : base(
        "그을린 대지",
        "마법사는 대지를 태우는 불꽃을 소환하여 기본 피해+공격피해*스킬계수의 범위 마법피해를 입힙니다.",
        5,
        new int[] { 15, 25, 35, 45, 60 },
        new float[] { 1.2f, 1.4f, 1.6f, 1.8f, 2.0f }
    )
    {
    }

    public override void UseSkill(Hero caster)
    {
        Wizard wizard = caster as Wizard;
        if (wizard == null)
        {
            Debug.LogError("ScorchedEarth skill can only be used by Wizard");
            return;
        }

        if (wizard._target == null)
        {
            Debug.Log("No target found for ScorchedEarth skill");
            return;
        }

        int baseDamage = BaseDamage[Level - 1];
        float coefficient = Coefficients[Level - 1];
        float totalDamage = baseDamage + (wizard.attackDamage * coefficient);

        Vector2 targetPosition = wizard._target.transform.position;

        // 범위 내의 모든 적을 찾아 데미지를 입힘
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(targetPosition, aoeRadius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(wizard, totalDamage);
                Debug.Log($"ScorchedEarth hit {enemy.name} for {totalDamage} damage");
            }
        }

        // 스킬 사용 후 에너지 소모
        wizard.Energy = 0;

        // 스킬 이펙트 생성 (옵션)
        CreateSkillEffect(targetPosition);
    }

    private void CreateSkillEffect(Vector2 position)
    {
        // 여기에 스킬 이펙트를 생성하는 코드를 추가할 수 있습니다.
        // 예: 파티클 시스템을 사용하여 불꽃 효과를 생성
        Debug.Log("ScorchedEarth effect created at " + position);
    }
}