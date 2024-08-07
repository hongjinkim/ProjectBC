using UnityEngine;

public class PenetratingArrow : PlayerSkill
{
    public PenetratingArrow() : base(
        "�����ϴ� ȭ��",
        "Archer�� ������ ȭ���� ��� �⺻����()+��������*��ų����� �������� ��������� ��� ������ ������ ���ظ� ������.",
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
            float maxDistance = 100f; // ��ų�� �ִ� ��Ÿ�

            // ��� Enemy ��ü�� ã���ϴ�
            Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

            foreach (Enemy enemy in allEnemies)
            {
                Vector2 toEnemy = enemy.transform.position - archer.transform.position;
                float dotProduct = Vector2.Dot(toEnemy.normalized, direction);

                // ���� ȭ���� ��� �� �ְ�, �ִ� ��Ÿ� ���� �ִ��� Ȯ���մϴ�
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