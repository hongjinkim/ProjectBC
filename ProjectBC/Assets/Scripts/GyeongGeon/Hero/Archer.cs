
using UnityEngine;

public class Archer : Hero
{
    public HeroClass _heroClass;
    public PenetratingArrow penetratingArrow;
    protected override void Start() 
    {
        base.Start();
        _heroClass = HeroClass.Archer;
        info.characteristicType = CharacteristicType.Agility;
        info.attackRange = 4;
        penetratingArrow = new PenetratingArrow();
        info.skills.Add(penetratingArrow);

        info.activeSkill = penetratingArrow;

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
