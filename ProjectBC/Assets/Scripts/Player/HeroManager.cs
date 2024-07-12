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

    private List<Hero> AllHeroes = new List<Hero>();
    [SerializeField] private List<Hero> MyHeroes = new List<Hero>();
    [SerializeField] private List<Hero> Deck = new List<Hero>();
    [SerializeField] private Transform heroSlotsParent;
    [SerializeField] private Transform deckSlotsParent;
    private HeroSlot[] heroSlots;
    private DeckSlot[] deckSlots;
    private int maxDeckSize = 4;

    private void Awake()
    {
        InitializeHeroSlots();

        deckSlots = new DeckSlot[maxDeckSize];
    }

    private void Start()
    {
        InitializeAllHeroes();
        InitializeMyHeroes();
        UpdateHeroSlots();
        InitializeDeckSlots();
    }
    private void InitializeDeckSlots()
    {
        if (deckSlotsParent != null)
        {
            deckSlots = deckSlotsParent.GetComponentsInChildren<DeckSlot>();
        }
    }

    private void InitializeHeroSlots()
    {
        if (heroSlotsParent != null)
        {
            heroSlots = heroSlotsParent.GetComponentsInChildren<HeroSlot>();
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
                heroSlots[i].SetHeroData(MyHeroes[i], i);
            }
            else
            {
                heroSlots[i].SetHeroData(null, -1);
            }
        }
    }
    private void UpdateDeckSlots()
    {
        if (deckSlots == null || deckSlots.Length == 0)
        {
            return;
        }

        for (int i = 0; i < deckSlots.Length; i++)
        {
            if (deckSlots[i] == null)
            {
                continue;
            }

            if (i < Deck.Count)
            {
                deckSlots[i].DeckSetHeroData(Deck[i], i);
            }
            else
            {
                deckSlots[i].DeckSetHeroData(null, -1);
            }
        }
    }



    public void AddHeroToDeck(int heroIndex)
    {
        if (heroIndex < 0 || heroIndex >= MyHeroes.Count)
        {
            return;
        }

        Hero hero = MyHeroes[heroIndex];

        if (Deck.Count >= maxDeckSize)
        {
            return;
        }

        if (Deck.Contains(hero))
        {
            return;
        }

        Deck.Add(hero);
        RemoveHeroFromMyHeroes(hero);
        UpdateDeckSlots();
    }
    public void RemoveHeroFromDeck(int deckIndex)
    {
        if (deckIndex >= 0 && deckIndex < Deck.Count)
        {
            Hero removedHero = Deck[deckIndex];
            Deck.RemoveAt(deckIndex);
            MyHeroes.Add(removedHero);
            UpdateDeckSlots();
            UpdateHeroSlots();
        }
    }
    public void RemoveHeroFromMyHeroes(Hero hero)
    {
        MyHeroes.Remove(hero);
        UpdateHeroSlots();
    }
}