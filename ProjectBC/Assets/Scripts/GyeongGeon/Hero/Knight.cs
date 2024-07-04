public enum HeroClass
{
    Knight,
    Archer,
    Priest,
    Wizard
}

public class Knight : Character
{
    public HeroClass _heroClass;

    protected override void Start() 
    {
        _heroClass = HeroClass.Knight;    
    }

    protected override void OnAnimAttack()
    {
        animator.SetTrigger("Slash1H");
        IsAction = true;
    }


}
