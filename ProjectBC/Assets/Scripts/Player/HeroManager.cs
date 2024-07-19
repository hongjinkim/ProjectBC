using System.Collections.Generic;
using Unity.VisualScripting;
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

    public List<Hero> AllHeroes = new List<Hero>();
    [SerializeField] private List<Hero> MyHeroes = new List<Hero>();
    [SerializeField] public List<Hero> Deck = new List<Hero>();
    [SerializeField] private Transform heroSlotsParent;
    [SerializeField] private Transform deckSlotsParent;
    [SerializeField] private Transform battleDeckSlotsParent;
    [SerializeField] private Transform myHeroDeckSlotsParent;
    private HeroSlot[] heroSlots;
    private DeckSlot[] deckSlots;
    private BattleDeckSlot[] battleDeckSlots;
    private MyHeroSlot[] myHeroSlots;
    private int maxDeckSize = 4;

    private void Awake()
    {

    }

    private void Start()
    {
        InitializeSlots();
        InitializeAllHeroes();
        InitializeMyHeroes();
        UpdateHeroSlots();
        UpdateDeckSlots();
        UpdateMyHeroSlots();
    }
    private void InitializeSlots()
    {
        heroSlots = heroSlotsParent.GetComponentsInChildren<HeroSlot>();
        deckSlots = deckSlotsParent.GetComponentsInChildren<DeckSlot>();
        battleDeckSlots = battleDeckSlotsParent.GetComponentsInChildren<BattleDeckSlot>();
        myHeroSlots = myHeroDeckSlotsParent.GetComponentsInChildren<MyHeroSlot>();
    }
    private void InitializeAllHeroes()
    {
        AllHeroes.Add(new Hero { id = 1001, name = "Warrior", level = 1, power = 10, speed = 5, hp = 150, sprite = Resources.Load<Sprite>("Images/currency/Warrior") });
        AllHeroes.Add(new Hero { id = 2001, name = "Priest", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/Priest") });
        AllHeroes.Add(new Hero { id = 3001, name = "Wizard", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/Wizard") });
        AllHeroes.Add(new Hero { id = 3002, name = "Archer", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/Archer") });
        AllHeroes.Add(new Hero { id = 3003, name = "non-slot", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/YellowGemstone") });
    }

    private void InitializeMyHeroes()
    {
        MyHeroes.Clear();
        MyHeroes.Add(AllHeroes[0]);
        MyHeroes.Add(AllHeroes[1]);
        MyHeroes.Add(AllHeroes[2]);
        MyHeroes.Add(AllHeroes[3]);
        MyHeroes.Add(AllHeroes[4]);
    }

    private void UpdateHeroSlots()
    {
        for (int i = 0; i < heroSlots.Length; i++)
        {
            if (i < MyHeroes.Count)
            {
                heroSlots[i].SetHeroData(MyHeroes[i], i);               
            }

            else
            {
                heroSlots[i].ClearSlot();
            }

        }
    }

    private void UpdateMyHeroSlots()
    {
        for (int i = 0; i < myHeroSlots.Length; i++)
        {
            if (i < MyHeroes.Count)
            {
                myHeroSlots[i].SetMyHeroData(MyHeroes[i], i);
            }
        }
    }

    private void UpdateDeckSlots()
    {
        for (int i = 0; i < deckSlots.Length; i++)
        {
            if (i < Deck.Count)
                deckSlots[i].DeckSetHeroData(Deck[i], i);
            else
                deckSlots[i].ClearSlot();
        }
        UpdateBattleDeckSlots();
    }

    private void UpdateBattleDeckSlots()
    {
        for (int i = 0; i < battleDeckSlots.Length; i++)
        {
            if (i < Deck.Count)
                battleDeckSlots[i].SetHeroData(Deck[i], i);
            else
                battleDeckSlots[i].ClearSlot();
        }
    }

    public void AddHeroToDeck(int heroIndex)
    {
        if (heroIndex < 0 || heroIndex >= MyHeroes.Count || Deck.Count >= maxDeckSize)
            return;

        Hero hero = MyHeroes[heroIndex];
        Deck.Add(hero);
        MyHeroes.RemoveAt(heroIndex);
        UpdateHeroSlots();
        UpdateDeckSlots();

        // GameManager_2의 HeroDeckPrefab 업데이트
        GameManager_2.Instance.UpdateHeroDeckPrefab(Deck.Count - 1, hero.id);
    }

    public void RemoveHeroFromDeck(int deckIndex)
    {
        if (deckIndex < 0 || deckIndex >= Deck.Count)
            return;

        Hero hero = Deck[deckIndex];
        MyHeroes.Add(hero);
        Deck.RemoveAt(deckIndex);
        UpdateHeroSlots();
        UpdateDeckSlots();

        // GameManager_2의 HeroDeckPrefab 업데이트
        for (int i = deckIndex; i < maxDeckSize; i++)
        {
            if (i < Deck.Count)
            {
                GameManager_2.Instance.UpdateHeroDeckPrefab(i, Deck[i].id);
            }
            else
            {
                GameManager_2.Instance.HeroDeckPrefab[i] = null;
            }
        }
    }
    public void RemoveHeroFromMyHeroes(Hero hero)
    {
        MyHeroes.Remove(hero);
        UpdateHeroSlots();
    }

    public int GetHeroIdFromDeckSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < Deck.Count)
        {
            return Deck[slotIndex].id;
        }
        return -1; // 유효하지 않은 ID
    }
}