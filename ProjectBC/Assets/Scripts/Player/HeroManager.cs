using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    [System.Serializable]
    public class Hero
    {
        public int id;
        public string name;
        public int level = 1;
        public int power = 1;
        public int speed = 1;
        public int hp = 100;


    }

    [SerializeField] private List<Hero> AllHeroes = new List<Hero>();
    [SerializeField] private List<Hero> MyHeroes = new List<Hero>();
    [SerializeField] private List<Hero> Deck = new List<Hero>();
    public int MaxDeckSize = 4;

    private void Awake()
    {
        InitializeAllHeroes();
    }
    void Start()
    {
        InitializeHeroSlots();
    }
    private void InitializeAllHeroes()
    {
        // 게임에서 제공하는 모든 히어로 초기화
        AllHeroes.Add(new Hero { id = 1, name = "Warrior", power = 10, speed = 5, hp = 150 });
        AllHeroes.Add(new Hero { id = 2, name = "Priest", power = 5, speed = 7, hp = 100 });
        AllHeroes.Add(new Hero { id = 3, name = "Archer", power = 5, speed = 7, hp = 100 });
        AllHeroes.Add(new Hero { id = 4, name = "Assassin", power = 5, speed = 7, hp = 100 });
        AllHeroes.Add(new Hero { id = 5, name = "Tanker", power = 5, speed = 7, hp = 100 });
    }

    public void InitializeHeroSlots()
    {
        MyHeroes.Clear();
        MyHeroes.Add(AllHeroes[0]);
        MyHeroes.Add(AllHeroes[2]);

        HeroSlotUI[] heroSlots = FindObjectsOfType<HeroSlotUI>();
        int slotCount = heroSlots.Length;

        for (int i = 0; i < slotCount; i++)
        {
            heroSlots[i].SetSlotIndex(i);
            if (i < MyHeroes.Count)
            {
                heroSlots[i].SetHeroData(MyHeroes[i]);
            }
            else
            {
                heroSlots[i].SetHeroData(null);
            }
        }
    }

    public void UnlockHero(int heroId)
    {
        Hero heroToUnlock = AllHeroes.Find(h => h.id == heroId);
        if (heroToUnlock != null && !MyHeroes.Exists(h => h.id == heroId))
        {
            MyHeroes.Add(new Hero
            {
                id = heroToUnlock.id,
                name = heroToUnlock.name,
                level = heroToUnlock.level,
                power = heroToUnlock.power,
                speed = heroToUnlock.speed,
                hp = heroToUnlock.hp
            });
        }
    }

    public bool AddHeroToDeck(int heroId)
    {
        if (Deck.Count >= MaxDeckSize) return false;

        Hero heroToAdd = MyHeroes.Find(h => h.id == heroId);
        if (heroToAdd != null && !Deck.Exists(h => h.id == heroId))
        {
            Deck.Add(heroToAdd);
            return true;
        }
        return false;
    }

    public bool RemoveHeroFromDeck(int heroId)
    {
        return Deck.RemoveAll(h => h.id == heroId) > 0;
    }

    public Hero[] GetAllHeroesAsArray()
    {
        return AllHeroes.ToArray();
    }

    public Hero[] GetMyHeroesAsArray()
    {
        return MyHeroes.ToArray();
    }

    public Hero[] GetDeckAsArray()
    {
        return Deck.ToArray();
    }

    public Hero GetHeroById(int id)
    {
        return AllHeroes.Find(h => h.id == id);
    }

    public void LevelUpHero(int heroId)
    {
        Hero hero = MyHeroes.Find(h => h.id == heroId);
        if (hero != null)
        {
            hero.level++;
            hero.power += 2;
            hero.speed += 1;
            hero.hp += 10;
        }
    }

    public void DisplayHeroInfo(int heroId)
    {
        Hero hero = GetHeroById(heroId);
        if (hero != null)
        {
            Debug.Log($"Hero: {hero.name}, Level: {hero.level}, Power: {hero.power}, Speed: {hero.speed}, HP: {hero.hp}");
        }
        else
        {
            Debug.Log("Hero not found");
        }
    }
}