using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteCollection", menuName = "Scriptables/SpriteCollection")]
public class SpriteCollection : ScriptableObject
{
    public string Id;

    [Header("Where to find sprites?")]
    public List<Object> SpriteFolders;
    public List<string> CollectionFilter;
    public List<string> CollectionFilterIgnore;

    [Header("Body Parts")]
    public List<ItemSprite> Body;
    public List<ItemSprite> Ears;
    public List<ItemSprite> Hair;
    public List<ItemSprite> Beard;
    public List<ItemSprite> Eyebrows;
    public List<ItemSprite> Eyes;
    public List<ItemSprite> Mouth;

    [Header("Equipment")]
    public List<ItemSprite> Armor;
    public List<ItemSprite> MeleeWeapon1H;
    public List<ItemSprite> MeleeWeapon2H;
    public List<ItemSprite> Bow;
    public List<ItemSprite> Crossbow;
    public List<ItemSprite> Firearm1H;
    public List<ItemSprite> Firearm2H;
    public List<ItemSprite> Shield;
    public List<ItemSprite> Back;
    public List<ItemSprite> Throwable;
    public List<ItemSprite> Supplies;

    [Header("Accessories")]
    public List<ItemSprite> Makeup;
    public List<ItemSprite> Mask;
    public List<ItemSprite> Earrings;
    public List<ItemSprite> Wings;

    [Header("Service")]
    public bool IncludePsd;
    public bool DebugLogging;

    public List<ItemSprite> GetAllSprites()
    {
        return Body.Union(Ears).Union(Hair).Union(Beard).Union(Eyebrows).Union(Eyes).Union(Mouth)
            .Union(Armor).Union(Back).Union(MeleeWeapon1H).Union(MeleeWeapon2H)
            .Union(Bow).Union(Crossbow).Union(Firearm1H).Union(Firearm2H).Union(Shield).Union(Throwable).Union(Supplies)
            .Union(Makeup).Union(Mask).Union(Earrings).Union(Wings).ToList();
    }
}
