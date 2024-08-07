using UnityEngine;
using System.Collections.Generic;

public class ScorchedEarth : PlayerSkill
{
    public float aoeRadius = 0.8f; // AOE ���� �ݰ�

    public ScorchedEarth() : base(
        "������ ����",
        "������� ������ �¿�� �Ҳ��� ��ȯ�Ͽ� �⺻ ����+��������*��ų����� ���� �������ظ� �����ϴ�.",
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

        // ���� ���� ��� ���� ã�� �������� ����
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

        // ��ų ��� �� ������ �Ҹ�
        wizard.Energy = 0;

        // ��ų ����Ʈ ���� (�ɼ�)
        CreateSkillEffect(targetPosition);
    }

    private void CreateSkillEffect(Vector2 position)
    {
        // ���⿡ ��ų ����Ʈ�� �����ϴ� �ڵ带 �߰��� �� �ֽ��ϴ�.
        // ��: ��ƼŬ �ý����� ����Ͽ� �Ҳ� ȿ���� ����
        Debug.Log("ScorchedEarth effect created at " + position);
    }
}