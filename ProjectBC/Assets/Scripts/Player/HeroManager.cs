using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public List<Player> AllHeroes { get; private set; } = new List<Player>();
    public List<Player> ActiveHeroes { get; private set; } = new List<Player>();
    public int MaxActiveHeroes = 4;

    private void Start()
    {
        InitializeHeroes();
    }

    private void InitializeHeroes()
    {
        // 모든 영웅 초기화 (예시)
        //AllHeroes.Add(new Archer());
        //AllHeroes.Add(new Wizard());
        //AllHeroes.Add(new Priest());
        //AllHeroes.Add(new Knight());
    }

    public bool AddHeroToPortal(Player hero)
    {
        if (ActiveHeroes.Count < MaxActiveHeroes && !ActiveHeroes.Contains(hero))
        {
            ActiveHeroes.Add(hero);
            return true;
        }
        return false;
    }

    public bool RemoveHeroFromPortal(Player hero)
    {
        return ActiveHeroes.Remove(hero);
    }

    public void SwapHeroes(Player heroToAdd, Player heroToRemove)
    {
        if (ActiveHeroes.Contains(heroToRemove))
        {
            ActiveHeroes.Remove(heroToRemove);
            ActiveHeroes.Add(heroToAdd);
        }
    }
}