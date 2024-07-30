using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSkills : HeroSkills
{
    public KnightSkills(PlayerStat stat) : base(stat) { }

    protected override void InitializeSkills()
    {
        skills.Add(new PowerfulStrike());
    }
}