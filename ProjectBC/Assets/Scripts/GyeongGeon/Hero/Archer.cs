
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
    //        // ��ų ��� ����...
    //    }
    //}
    protected override void Update()
    {
        base.Update(); // �� �κ��� �߿��մϴ�!
        // �߰����� Archer Ư�� ������Ʈ ����
    }
    private void ApplyPassiveSkills()
    {
        // ��ȭ�� Ȱ ����
        info.trueDamage += enhancedBow.GetTrueDamageBonus();

        // ��ݼ� ����
        info.attackSpeed += marksmanship.GetAttackSpeedBonus();

        // ���� ���� ����
        info.defensePenetration += weaknessDetection.GetDefensePenetrationBonus();

        // �нú� ��ų ���� �� ���� �Ӽ� ������Ʈ
        UpdateAttackInterval();
    }

    // �������̳� ��ų ���� ���� �� ȣ��
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

    // Archer Ư���� �޼��� �߰�
    protected override void UseSkill()
    {
        Debug.Log($"Archer UseSkill method called. Current Energy: {Energy}");
        
        if (Energy >= 100 && info.activeSkill != null)
        {
            Debug.Log($"Archer energy is full, using {info.activeSkill.Name}");
            info.activeSkill.UseSkill(this);
            Energy = 0;  // ��ų ��� �� ������ ����
        }
        //else
        //{
        //    base.UseSkill();
        //    Debug.Log($"Not enough energy to use skill or no active skill set. Current energy: {Energy}, Active skill: {info.activeSkill?.Name ?? "None"}");
        //}
    }
}
