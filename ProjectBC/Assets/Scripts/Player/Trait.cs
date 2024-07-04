using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait
{
    public TraitType Type { get; private set; }
    public string Description { get; private set; }
    public Action Effect { get; private set; }

    public Trait(TraitType type, string description, Action effect)
    {
        Type = type;
        Description = description;
        Effect = effect;
    }


    public void ApplyEffect()
    {
        Effect?.Invoke();
    }
}