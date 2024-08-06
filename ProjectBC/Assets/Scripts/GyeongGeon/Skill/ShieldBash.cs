using UnityEngine;

public class ShieldBash : PlayerSkill
{
    public ShieldBash() : base(
        "���� ����",
        "���� ���� �⺻ ����+���� ����*��ų ��� �� ������ ���ظ� �����ϴ�.",
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

        // ��ų ��� �� ������ �Ҹ�
        knight.Energy = 0;

        // ��ų ����Ʈ ���� (�ɼ�)
        CreateSkillEffect(knight._target.transform.position);
    }

    private void CreateSkillEffect(Vector3 position)
    {
        // ���⿡ ��ų ����Ʈ�� �����ϴ� �ڵ带 �߰��� �� �ֽ��ϴ�.
        // ��: ��ƼŬ �ý����� ����Ͽ� ���� �浹 ȿ���� ����
        Debug.Log("ShieldBash effect created at " + position);
    }
}