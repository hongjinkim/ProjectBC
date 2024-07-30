using System.Collections.Generic;
using UnityEngine;

public class PierceShot : ActiveSkill
{
    public PierceShot() : base("Pierce Shot", "Deals physical damage to enemies in a rectangular area", 1, 5)
    {
        BaseDamage = new[] { 12, 24, 36, 48, 60 };
        DamageScaling = new[] { 1.2f, 1.4f, 1.6f, 1.8f, 2.0f };
    }

    public override void ApplyEffect(PlayerStat playerStat)
    {
        // 이 메서드는 패시브 스킬에 사용되므로 액티브 스킬에서는 비워둡니다.
    }
    public (Vector2, Vector2, Vector2, Vector2) CalculateSkillArea(Character archer)
    {
        if (archer._target == null) return default;

        Vector2 direction = (archer._target.transform.position - archer.transform.position).normalized;
        float maxDistance = 10f; // 최대 사거리
        float width = 2f; // 직사각형의 폭

        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * (width / 2);
        Vector2 archerPos = archer.transform.position;
        Vector2 farEnd = archerPos + direction * maxDistance;

        Vector2 topLeft = archerPos + perpendicular;
        Vector2 topRight = farEnd + perpendicular;
        Vector2 bottomLeft = archerPos - perpendicular;
        Vector2 bottomRight = farEnd - perpendicular;

        return (topLeft, topRight, bottomRight, bottomLeft);
    }
    public override void Use(PlayerStat playerStat)
    {
        Character archer = playerStat.GetComponent<Character>();
        if (archer != null)
        {
            float damage = BaseDamage[Level - 1] + playerStat.AttackDamage * DamageScaling[Level - 1];

            List<Character> targets = FindTargetsInRectangle(archer);

            foreach (var target in targets)
            {
                archer.TakeDamage(target, damage);
            }

            Debug.Log($"Archer used Pierce Shot, hitting {targets.Count} enemies for {damage} damage each!");
        }
        else
        {
            Debug.LogError("PlayerStat is not attached to a Character!");
        }
    }

    private List<Character> FindTargetsInRectangle(Character archer)
    {
        List<Character> targets = new List<Character>();

        if (archer._target == null)
        {
            Debug.LogWarning("Archer's target is null!");
            return targets;
        }

        Vector2 direction = (archer._target.transform.position - archer.transform.position).normalized;
        float maxDistance = 10f; // 최대 사거리
        float width = 2f; // 직사각형의 폭

        // 직사각형의 네 꼭지점 계산
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * (width / 2);
        Vector2 archerPos = archer.transform.position;
        Vector2 farEnd = archerPos + direction * maxDistance;

        Vector2 topLeft = archerPos + perpendicular;
        Vector2 topRight = farEnd + perpendicular;
        Vector2 bottomLeft = archerPos - perpendicular;
        Vector2 bottomRight = farEnd - perpendicular;

        // 이 줄을 추가하여 계산된 범위를 로그로 출력
        Debug.Log($"Skill area: TL:{topLeft}, TR:{topRight}, BL:{bottomLeft}, BR:{bottomRight}");

        // 직사각형 영역 내의 모든 적 찾기
        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(bottomLeft, topRight);
        Debug.Log($"Found {hitColliders.Length} colliders in the area");

        foreach (var hitCollider in hitColliders)
        {
            Character enemy = hitCollider.GetComponent<Character>();
            if (enemy != null)
            {
                Debug.Log($"Found character: {enemy.name}, Tag: {enemy.gameObject.tag}");
                if (enemy.gameObject.CompareTag("Enemy"))
                {
                    Vector2 enemyPos = enemy.transform.position;
                    Debug.Log($"Enemy position: {enemyPos}");  // 이 줄을 추가
                                                               // 적이 직사각형 내부에 있는지 확인
                    if (IsPointInRectangle(enemyPos, topLeft, topRight, bottomLeft, bottomRight))
                    {
                        targets.Add(enemy);
                        Debug.Log($"Added enemy: {enemy.name} to targets");
                    }
                    else
                    {
                        Debug.Log($"Enemy {enemy.name} is outside the skill area");  // 이 줄을 추가
                    }
                }
            }
        }

        return targets;
    }

    private bool IsPointInRectangle(Vector2 point, Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
    {
        Vector2 AP = point - topLeft;
        Vector2 AB = topRight - topLeft;
        Vector2 AD = bottomLeft - topLeft;

        return (0 <= Vector2.Dot(AP, AB) && Vector2.Dot(AP, AB) <= Vector2.Dot(AB, AB))
            && (0 <= Vector2.Dot(AP, AD) && Vector2.Dot(AP, AD) <= Vector2.Dot(AD, AD));
    }
    private bool IsPointOnRightSide(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        return ((lineEnd.x - lineStart.x) * (point.y - lineStart.y) - (lineEnd.y - lineStart.y) * (point.x - lineStart.x)) > 0;
    }

    private int[] BaseDamage { get; }
    private float[] DamageScaling { get; }
}