using UnityEngine;

public class ShieldBash : PlayerSkill
{
    public ShieldBash() : base(
        "방패 가격",
        "적을 때려 기본 피해+공격 피해*스킬 계수 의 물리적 피해를 입힙니다.",
        5,
        new int[] { 10, 20, 30, 40, 50 },
        new float[] { 1.15f, 1.3f, 1.45f, 1.6f, 1.75f }
    )
    {
    }

    public override void UseSkill(Hero caster)
    {
        Knight knight = caster as Knight;
        if (knight == null)
        {
            Debug.LogError("ShieldBash skill can only be used by Knight");
            return;
        }

        if (knight._target == null)
        {
            Debug.Log("No target found for ShieldBash skill");
            return;
        }

        int baseDamage = BaseDamage[Level - 1];
        float coefficient = Coefficients[Level - 1];
        float totalDamage = baseDamage + (knight.attackDamage * coefficient);

        Enemy enemy = knight._target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(knight, totalDamage);
            Debug.Log($"ShieldBash hit {enemy.name} for {totalDamage} damage");
        }
        else
        {
            Debug.Log("ShieldBash target is not an enemy");
        }

        // 스킬 사용 후 에너지 소모
        knight.Energy = 0;

        // 스킬 이펙트 생성 (옵션)
        CreateSkillEffect(knight._target.transform.position);
    }

    private void CreateSkillEffect(Vector3 position)
    {
        // 여기에 스킬 이펙트를 생성하는 코드를 추가할 수 있습니다.
        // 예: 파티클 시스템을 사용하여 방패 충돌 효과를 생성
        Debug.Log("ShieldBash effect created at " + position);
    }
}