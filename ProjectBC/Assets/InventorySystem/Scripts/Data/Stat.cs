using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

[Serializable]
public class Stat
{
    public StatId id;
    public int value;

    public Stat()
    {
    }

    public Stat(StatId id, int value)
    {
        this.id = id;
        this.value = value;
    }

    public Stat Copy()
    {
        return new Stat(id, value);
    }
}