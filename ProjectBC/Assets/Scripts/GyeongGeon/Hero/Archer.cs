
using UnityEngine;

public class Archer : Hero
{
    public HeroClass _heroClass;
    public PenetratingArrow penetratingArrow;
    public EnhancedBow enhancedBow;
    public Marksmanship marksmanship;
    public WeaknessDetection weaknessDetection;
    protected override void Start() 
    {
        base.Start();
        _heroClass = HeroClass.Archer;
        info.characteristicType = CharacteristicType.Agility;
        info.attackRange = 4;
        penetratingArrow = new PenetratingArrow();
        enhancedBow = new EnhancedBow();
        marksmanship = new Marksmanship();
        weaknessDetection = new WeaknessDetection();
        info.skills.Add(penetratingArrow);
        info.skills.Add(enhancedBow);
        info.skills.Add(marksmanship);
        info.skills.Add(weaknessDetection);

        info.activeSkill = penetratingArrow;
        ApplyPassiveSkills();
        Debug.Log($"Archer initialized with {info.skills.Count} skills. Active skill: {info.activeSkill?.Name ?? "None"}");
    }

    //private void Update()
    //{
    //    if (info.energy >= 100)
    //    {
    //        info.energy = 0;
    //        UseSkill();
    //        // 스킬 사용 로직...
    //    }
    //}
    protected override void Update()
    {
        base.Update(); // 이 부분이 중요합니다!
        // 추가적인 Archer 특정 업데이트 로직
    }
    private void ApplyPassiveSkills()
    {
        // 강화된 활 적용
        info.trueDamage += enhancedBow.GetTrueDamageBonus();

        // 사격술 적용
        info.attackSpeed += marksmanship.GetAttackSpeedBonus();

        // 약점 포착 적용
        info.defensePenetration += weaknessDetection.GetDefensePenetrationBonus();

        // 패시브 스킬 적용 후 관련 속성 업데이트
        UpdateAttackInterval();
    }

    // 레벨업이나 스킬 레벨 변경 시 호출
    public void UpdatePassiveSkills()
    {
        ApplyPassiveSkills();
    }
    protected override void OnAnimAttack()
    {
		animator.SetTrigger("ShotBow");
        IsAction = true;
    }
    public override void IncreaseCharacteristic(float amount)
    {
        //IncreaseAgility(amount * 2);
    }

    // Archer 특유의 메서드 추가
    protected override void UseSkill()
    {
        Debug.Log($"Archer UseSkill method called. Current Energy: {Energy}");
        
        if (Energy >= 100 && info.activeSkill != null)
        {
            Debug.Log($"Archer energy is full, using {info.activeSkill.Name}");
            info.activeSkill.UseSkill(this);
            Energy = 0;  // 스킬 사용 후 에너지 리셋
        }
        //else
        //{
        //    base.UseSkill();
        //    Debug.Log($"Not enough energy to use skill or no active skill set. Current energy: {Energy}, Active skill: {info.activeSkill?.Name ?? "None"}");
        //}
    }
}
