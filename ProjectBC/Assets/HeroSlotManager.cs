using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroSlotManager : MonoBehaviour
{
    public HeroManager heroManager;

    public List<MyHeroSlot> slots;

    public void OnValidate()
    {
        if (Application.isPlaying) return;

        slots = GetComponentsInChildren<MyHeroSlot>(true).ToList();
    }

    void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        var hero = heroManager.AllHeroes;
        for (int i = 0; i < hero.Count(); i++)
        {
            slots[i].SetMyHeroData(hero[i]);
        }
    }

   
}
