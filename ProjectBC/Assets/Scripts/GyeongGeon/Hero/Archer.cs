public class Archer : Character
{
    public HeroClass _heroClass;

    protected override void Start() 
    {
        _heroClass = HeroClass.Archer;   
    }

    protected override void OnAnimAttack()
    {
		animator.SetTrigger("ShotBow");
        IsAction = true;
    }
}
