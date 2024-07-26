using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

[Serializable]
public class Stat
{
    public BasicStat id;
    public int value;

    public Stat()
    {
    }

    public Stat(BasicStat id, int value)
    {
        this.id = id;
        this.value = value;
    }

    public Stat Copy()
    {
        return new Stat(id, value);
    }
}