public class Archer : Hero
{
    public HeroClass _heroClass;

    protected override void Start() 
    {
        base.Start();
        _heroClass = HeroClass.Archer;
        info.characteristicType = CharacteristicType.Agility;
        info.attackRange = 4;
    }

    protected override void OnAnimAttack()
    {
		animator.SetTrigger("ShotBow");
        IsAction = true;
    }
    public override void IncreaseCharacteristic(float amount)
    {
        IncreaseAgility(amount * 2);
    }

    // Archer Ư���� �޼��� �߰�
    public void UseSkill()
    {
        if (info.energy >= 100)
        {
            info.energy = 0;
            // ��ų ��� ����...
        }
    }
}
