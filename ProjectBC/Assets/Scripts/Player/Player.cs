using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Player : MonoBehaviour
{
    protected PlayerStat playerStat;

    public float currentExp;
    public int level = 1;
    public float needexp;
    public int Level { get; private set; }
    public List<Trait> Traits { get; private set; }
    public PlayerSkill ActiveSkill { get; private set; }

    public Player()
    {
        Level = 1;
        Traits = new List<Trait>();
        ActiveSkill = new PlayerSkill("기본 스킬", "기본 설명", 5, new int[] { 10, 20, 30, 40, 50 }, new float[] { 1.1f, 1.2f, 1.3f, 1.4f, 1.5f });
    }

    protected virtual void Start()
    {
        if (playerStat == null)
        {
            playerStat = GetComponent<PlayerStat>();
        }
    }

    public virtual void IncreaseCharacteristic(float amount) { }

    protected void IncreaseStrength(float amount)
    {
        playerStat.Strength += (int)amount;
        playerStat.HealthRegen += (int)(0.1f * amount);
        playerStat.HP += (int)(1f * amount);

        if (playerStat.CharacteristicType == CharacteristicType.MuscularStrength)
        {
            playerStat.AttackDamage += (int)(0.7f * amount);
        }
    }

    protected void IncreaseAgility(float amount)
    {
        playerStat.Agility += (int)amount;
        playerStat.AttackSpeed += (int)(0.1f * amount);
        playerStat.Defense += (int)(0.1f * amount);

        if (playerStat.CharacteristicType == CharacteristicType.Agility)
        {
            playerStat.AttackDamage += (int)(0.9f * amount);
        }
    }

    protected void IncreaseIntelligence(float amount)
    {
        playerStat.Intelligence += (int)amount;
        playerStat.EnergyRegen += (int)(0.1f * amount);
        playerStat.MagicResistance += (int)(0.1f * amount);

        if (playerStat.CharacteristicType == CharacteristicType.Intellect)
        {
            playerStat.AttackDamage += (int)(0.9f * amount);
        }
    }

    protected void IncreaseStamina(float amount)
    {
        playerStat.Stamina += (int)amount;
        playerStat.HP += (int)(10f * amount);
    }

    public void LevelUp(int levelup)
    {
        level += levelup;
        IncreaseCharacteristic(levelup);
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (currentExp >= needexp)
        {
            currentExp -= needexp;
            LevelUp(1);
        }
    }

    private void SelectTrait()
    {
        List<Trait> availableTraits = GetAvailableTraits(Level);

        // 여기서 플레이어가 특성을 선택하는 로직을 추가합니다.
        // 예를 들어, UI를 통해 플레이어가 선택할 수 있도록 할 수 있습니다.
        // 여기서는 첫 번째 특성을 자동으로 선택하는 예시를 제공합니다.
        if (availableTraits.Count > 0)
        {
            Traits.Add(availableTraits[0]);
            availableTraits[0].ApplyEffect();
        }
    }

    private List<Trait> GetAvailableTraits(int level)
    {
        List<Trait> traits = new List<Trait>();

        if (level == 10)
        {
            traits.Add(new Trait("방어 침투 +3%", "방어 침투를 3% 증가시킵니다.", () => { /* 방어 침투 증가 로직 */ }));
            traits.Add(new Trait("피해 증폭 +3%", "피해를 3% 증폭시킵니다.", () => { /* 피해 증폭 로직 */ }));
        }
        else if (level == 20)
        {
            traits.Add(new Trait("후속 공격 추가 피해 +8%", "적을 5회 공격 후 후속 공격에 8%의 추가 피해를 입힙니다.", () => { /* 추가 피해 로직 */ }));
            traits.Add(new Trait("추가 공격 속도 +20", "적을 5회 공격 후 3초 동안 20의 추가 공격 속도를 얻습니다.", () => { /* 추가 공격 속도 로직 */ }));
        }
        else if (level == 30)
        {
            traits.Add(new Trait("아머 및 마법 저항 감소 6%", "일반 공격은 타겟의 아머와 마법 저항을 6% 줄입니다.", () => { /* 아머 및 마법 저항 감소 로직 */ }));
            traits.Add(new Trait("체력 재생 감소 20", "일반 공격은 타겟의 체력 재생을 3초 동안 20으로 줄입니다.", () => { /* 체력 재생 감소 로직 */ }));
        }
        else if (level == 40)
        {
            traits.Add(new Trait("공격 속도 증가 +25", "적을 죽이면 3초 동안 공격 속도가 25 증가합니다.", () => { /* 공격 속도 증가 로직 */ }));
            traits.Add(new Trait("공격 피해 증가 +12%", "적을 죽이면 3초 동안 공격 피해가 12% 증가합니다.", () => { /* 공격 피해 증가 로직 */ }));
        }

        return traits;
    }

}

namespace PlayerClasses
{
    public class Warrior : Player
    {
        protected override void Start()
        {
            base.Start();
            playerStat.CharacteristicType = CharacteristicType.MuscularStrength;
        }

        public override void IncreaseCharacteristic(float amount)
        {
            IncreaseStrength(amount * 3);
        }
    }

    public class Archer : Player
    {
        public ArcherSkillBook skillBook;
        protected override void Start()
        {
            base.Start();
            playerStat.CharacteristicType = CharacteristicType.Agility;
        }

        public override void IncreaseCharacteristic(float amount)
        {
            IncreaseAgility(amount * 2);
        }
        public void UseSkill()
        {
            if (playerStat.Energy >= 100) // 예시로 에너지가 100 이상일 때 스킬 사용
            {
                playerStat.Energy = 0; // 에너지를 소모
                int damage = skillBook.PiercingArrow.GetDamage(playerStat.AttackDamage);
                // 적에게 데미지를 입히는 로직을 추가
                Debug.Log($"관통하는 화살 사용! 피해량: {damage}");
            }
        }

        public void LevelUpSkill()
        {
            skillBook.LevelUpSkill(skillBook.PiercingArrow);
        }
    }

    public class Wizard : Player
    {
        protected override void Start()
        {
            base.Start();
            playerStat.CharacteristicType = CharacteristicType.Intellect;
        }

        public override void IncreaseCharacteristic(float amount)
        {
            IncreaseIntelligence(amount * 2);
        }
    }

    public class Priest : Player
    {
        protected override void Start()
        {
            base.Start();
            playerStat.CharacteristicType = CharacteristicType.Intellect;
        }

        public override void IncreaseCharacteristic(float amount)
        {
            IncreaseIntelligence(amount * 2);
        }
    }
}