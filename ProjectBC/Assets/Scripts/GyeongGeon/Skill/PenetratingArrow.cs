using UnityEngine;

public class PenetratingArrow : PlayerSkill
{
    public PenetratingArrow() : base(
        "관통하는 화살",
        "Archer는 강력한 화살을 쏘아 기본피해()+공격피해*스킬계수의 데미지를 직선경로의 모든 적에게 물리적 피해를 입힌다.",
        5,
        new int[] { 12, 24, 36, 48, 60 },
        new float[] { 1.2f, 1.4f, 1.6f, 1.8f, 2.0f })
    {
    }

    public override void UseSkill(Hero caster)
    {
        
        Archer archer = caster as Archer;
        if (archer == null)
        {
            
            return;
        }

        int baseDamage = BaseDamage[Level - 1];
        float coefficient = Coefficients[Level - 1];
        float totalDamage = baseDamage + (archer.attackDamage * coefficient);
        

        int enemiesHit = 0;

        if (archer._target != null)
        {
            Vector2 direction = (archer._target.transform.position - archer.transform.position).normalized;
            float maxDistance = 100f; // 스킬의 최대 사거리

            // 모든 Enemy 객체를 찾습니다
            Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

            foreach (Enemy enemy in allEnemies)
            {
                Vector2 toEnemy = enemy.transform.position - archer.transform.position;
                float dotProduct = Vector2.Dot(toEnemy.normalized, direction);

                // 적이 화살의 경로 상에 있고, 최대 사거리 내에 있는지 확인합니다
                if (dotProduct > 0.99f && toEnemy.magnitude <= maxDistance)
                {
                    enemy.TakeDamage(archer, totalDamage);
                    enemiesHit++;
                    
                }
            }
        }
        else
        {
            
        }

        
        archer.Energy = 0;
    }
}