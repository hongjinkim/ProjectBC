using UnityEngine;
using System.Linq;

public class PurifyingLight : PlayerSkill
{
    public PurifyingLight() : base(
        "빛의 정화",
        "전장에서 HP가 가장 낮은 영웅을 치유합니다.",
        5,
        new int[] { 200, 280, 360, 440, 520 },
        new float[] { 1, 1, 1, 1, 1 }  // 계수가 없으므로 1로 설정
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

        // AttackRange 안에 다른 영웅이 없고 Priest 자신의 HP가 최대가 아닐 경우
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