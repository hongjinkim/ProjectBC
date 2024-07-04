public class Wizard : Character
{
    public HeroClass _heroClass;

    protected override void Start() 
    {
        _heroClass = HeroClass.Priest;    
    }

    protected override void OnAnimAttack()
    {
        animator.SetTrigger("Slash1H");
        IsAction = true;
    }
}
