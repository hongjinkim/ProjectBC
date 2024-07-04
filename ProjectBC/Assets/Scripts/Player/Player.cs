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