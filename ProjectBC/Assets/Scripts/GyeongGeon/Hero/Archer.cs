public class Archer : Character
{
    public HeroClass _heroClass;

    protected override void Start() 
    {
        base.Start();
        _heroClass = HeroClass.Archer;
        playerStat.CharacteristicType = CharacteristicType.Agility;
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

    // Archer 특유의 메서드 추가
    public void UseSkill()
    {
        if (playerStat.Energy >= 100)
        {
            playerStat.Energy = 0;
            // 스킬 사용 로직...
        }
    }
}
