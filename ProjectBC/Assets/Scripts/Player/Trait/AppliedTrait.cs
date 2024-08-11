using System;

[Serializable]
public class AppliedTrait
{
    public TraitType Type;
    public int Level;
    public bool IsLeftTrait;

    public AppliedTrait(TraitType type, int level, bool isLeftTrait)
    {
        Type = type;
        Level = level;
        IsLeftTrait = isLeftTrait;
    }
}