using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Modifier
{
    public ItemModifier id;
    public int level;

    public Modifier()
    {
    }

    public Modifier(ItemModifier id, int level)
    {
        this.id = id;
        this.level = level;
    }
}