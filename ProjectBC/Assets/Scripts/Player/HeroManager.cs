using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    [System.Serializable]
    public class Hero
    {
        public int id;
        public string name;
        public int level;
        public int power;
        public int speed;
        public int hp;
        public Sprite sprite;
    }

    [SerializeField] private List<Hero> AllHeroes = new List<Hero>();
    [SerializeField] private List<Hero> MyHeroes = new List<Hero>();
    [SerializeField] private List<Hero> Deck = new List<Hero>();
    [SerializeField] private HeroSlotUI[] heroSlots;
    [SerializeField] private Transform heroSlotsParent;

    private void Awake()
    {
            InitializeHeroSlots();
    }

    private void Start()
    {
        InitializeAllHeroes();
        InitializeMyHeroes();
        UpdateHeroSlots();
    }

    private void InitializeHeroSlots()
    {
        if (heroSlotsParent != null)
        {
            heroSlots = heroSlotsParent.GetComponentsInChildren<HeroSlotUI>();
        }

    }
    private void InitializeAllHeroes()
    {
        AllHeroes.Add(new Hero { id = 1, name = "Warrior", level = 1, power = 10, speed = 5, hp = 150, sprite = Resources.Load<Sprite>("Images/currency/Gemstone") });
        AllHeroes.Add(new Hero { id = 2, name = "Priest", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/GreenGemstone") });
        AllHeroes.Add(new Hero { id = 3, name = "Archer", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/PurpleGemstone") });
        AllHeroes.Add(new Hero { id = 4, name = "Assassin", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/RedGemstone") });
        AllHeroes.Add(new Hero { id = 5, name = "Tanker", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/YellowGemstone") });
    }

    private void InitializeMyHeroes()
    {
        MyHeroes.Clear();
        MyHeroes.Add(AllHeroes[0]);
        MyHeroes.Add(AllHeroes[2]);
    }

    private void UpdateHeroSlots()
    {
        for (int i = 0; i < heroSlots.Length; i++)
        {
            if (heroSlots[i] == null)
            {
                continue;
            }

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
}