public abstract class ActiveSkill : Skill
{
    protected ActiveSkill(string name, string description, int unlockLevel, int maxLevel)
        : base(name, description, unlockLevel, maxLevel, SkillType.Active)
    {
    }

    public abstract void Use(PlayerStat playerStat);
}