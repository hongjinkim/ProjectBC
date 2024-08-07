using UnityEngine;
using System.Linq;

public class PurifyingLight : PlayerSkill
{
    public PurifyingLight() : base(
        "���� ��ȭ",
        "���忡�� HP�� ���� ���� ������ ġ���մϴ�.",
        5,
        new int[] { 200, 280, 360, 440, 520 },
        new float[] { 1, 1, 1, 1, 1 }  // ����� �����Ƿ� 1�� ����
    )
    {
    }

    public override void UseSkill(Hero caster)
    {
        Priest priest = caster as Priest;
        if (priest == null)
        {
            Debug.LogError("PurifyingLight skill can only be used by Priest");
            return;
        }

        int healAmount = BaseDamage[Level - 1];
        Hero targetHero = FindLowestHpHeroInRange(priest);

        if (targetHero != null)
        {
            HealHero(targetHero, healAmount);
            Debug.Log($"PurifyingLight healed {targetHero.name} for {healAmount} HP");
        }
        else
        {
            Debug.Log("No hero found in range to heal");
        }

        priest.Energy = 0;
    }

    private Hero FindLowestHpHeroInRange(Priest priest)
    {
        Hero lowestHpHero = GameObject.FindObjectsOfType<Hero>()
            .Where(h => h != priest && h.currentHealth < h.maxHealth &&
                   Vector3.Distance(h.transform.position, priest.transform.position) <= priest.info.attackRange)
            .OrderBy(h => h.currentHealth / (float)h.maxHealth)
            .FirstOrDefault();

        // AttackRange �ȿ� �ٸ� ������ ���� Priest �ڽ��� HP�� �ִ밡 �ƴ� ���
        if (lowestHpHero == null && priest.currentHealth < priest.maxHealth)
        {
            lowestHpHero = priest;
        }

        return lowestHpHero;
    }

    private void HealHero(Hero hero, int healAmount)
    {
        hero.currentHealth = Mathf.Min(hero.currentHealth + healAmount, hero.maxHealth);
    }
}