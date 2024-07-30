public enum SkillType
{
    Passive,
    Active
}

public abstract class Skill
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public int Level { get; protected set; }
    public int MaxLevel { get; protected set; }
    public SkillType Type { get; protected set; }
    public bool IsUnlocked { get; private set; }
    public int UnlockLevel { get; protected set; }
    protected Skill(string name, string description, int unlockLevel, int maxLevel, SkillType type)
    {
        Name = name;
        Description = description;
        UnlockLevel = unlockLevel;
        MaxLevel = maxLevel;
        Type = type;
        Level = 1;
        IsUnlocked = false;
    }
    public void Unlock()
    {
        IsUnlocked = true;
    }
    public virtual bool LevelUp()
    {
        if (Level < MaxLevel)
        {
            Level++;
            return true;
        }
        return false;
    }

    public abstract void ApplyEffect(PlayerStat playerStat);
}