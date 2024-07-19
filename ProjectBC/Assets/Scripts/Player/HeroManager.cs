using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    private GameDataManager gameDataManager;

    public List<HeroInfo> AllHeroes = new List<HeroInfo>();
    [SerializeField] private List<HeroInfo> MyHeroes = new List<HeroInfo>();
    [SerializeField] public List<HeroInfo> Deck = new List<HeroInfo>();

    [SerializeField] private Transform heroSlotsParent;
    [SerializeField] private Transform deckSlotsParent;
    [SerializeField] private Transform battleDeckSlotsParent;
    private HeroSlot[] heroSlots;
    private DeckSlot[] deckSlots;
    private BattleDeckSlot[] battleDeckSlots;
    private int maxDeckSize = 4;

    private void Awake()
    {
        Debug.Log("HeroManager Awake");
        gameDataManager = FindObjectOfType<GameDataManager>();

        // heroSlots 초기화
        heroSlots = heroSlotsParent.GetComponentsInChildren<HeroSlot>(true);
        Debug.Log($"Found {heroSlots.Length} HeroSlots");

        // deckSlots 초기화
        deckSlots = deckSlotsParent.GetComponentsInChildren<DeckSlot>(true);
        Debug.Log($"Found {deckSlots.Length} DeckSlots");
        // battleDeckSlots 초기화
        battleDeckSlots = battleDeckSlotsParent.GetComponentsInChildren<BattleDeckSlot>(true);
        Debug.Log($"Found {battleDeckSlots.Length} BattleDeckSlots");
    }

    private void Start()
    {
        Debug.Log("HeroManager Start");
        gameDataManager.LogAllHeroes();
        LoadMyHeroes();
        UpdateHeroSlots();
        UpdateDeckSlots();
    }
    private void Update()
    {
        Debug.Log($"Current MyHeroes count: {MyHeroes.Count}");
    }
    //private void InitializeAllHeroes()
    //{
    //    // 기본 히어로 템플릿 초기화
    //    AllHeroes.Add(new HeroInfo { id = 1001, heroName = "Warrior", level = 1, attackDamage = 10, agility = 5, hp = 150, spritePath = "Images/currency/Gemstone" });
    //    AllHeroes.Add(new HeroInfo { id = 2001, heroName = "Priest", level = 1, attackDamage = 5, agility = 7, hp = 100, spritePath = "Images/currency/GreenGemstone" });
    //    // ... 다른 기본 히어로 추가 ...
    //}
    private void LoadMyHeroes()
    {
        MyHeroes = gameDataManager.GetAllHeroes();
        Debug.Log($"LoadMyHeroes: Loaded {MyHeroes.Count} heroes");
        foreach (var hero in MyHeroes)
        {
            Debug.Log($"Hero: {hero.heroName}, Class: {hero.heroClass}, ID: {hero.id}");
        }

        // 중복 방지를 위한 검사
        MyHeroes = MyHeroes.Distinct().ToList();
        Debug.Log($"After removing duplicates: {MyHeroes.Count} heroes");
    }
    public void UpdateHeroInfo(HeroInfo updatedHero)
    {
        int index = MyHeroes.FindIndex(h => h.id == updatedHero.id);
        if (index != -1)
        {
            MyHeroes[index] = updatedHero;
            gameDataManager.UpdateHero(updatedHero);
            UpdateHeroSlots();
            UpdateDeckSlots();
        }
    }
    private void InitializeSlots()
    {
        heroSlots = heroSlotsParent.GetComponentsInChildren<HeroSlot>();
        deckSlots = deckSlotsParent.GetComponentsInChildren<DeckSlot>();
        battleDeckSlots = battleDeckSlotsParent.GetComponentsInChildren<BattleDeckSlot>();
    }

    private void InitializeHeroes()
    {
        AllHeroes = gameDataManager.GetAllHeroes();
        MyHeroes = new List<HeroInfo>(AllHeroes);
        Deck = new List<HeroInfo>();
    }

    private void UpdateHeroSlots()
    {
        Debug.Log($"Updating hero slots. MyHeroes count: {MyHeroes.Count}, heroSlots count: {heroSlots?.Length ?? 0}");

        if (heroSlots == null || heroSlots.Length == 0)
        {
            Debug.LogError("heroSlots is not initialized properly!");
            return;
        }

        for (int i = 0; i < heroSlots.Length; i++)
        {
            if (heroSlots[i] == null)
            {
                Debug.LogError($"heroSlot at index {i} is null!");
                continue;
            }

            if (i < MyHeroes.Count)
            {
                Debug.Log($"Setting hero data for slot {i}: {MyHeroes[i].heroName}");
                heroSlots[i].SetHeroData(MyHeroes[i], i);
            }
            else
            {
                heroSlots[i].ClearSlot();
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
        if (battleDeckSlots == null)
        {
            Debug.LogError("battleDeckSlots is null!");
            return;
        }

        for (int i = 0; i < battleDeckSlots.Length; i++)
        {
            if (battleDeckSlots[i] == null)
            {
                Debug.LogError($"BattleDeckSlot at index {i} is null!");
                continue;
            }

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

        HeroInfo hero = MyHeroes[heroIndex];
        Deck.Add(hero);
        MyHeroes.RemoveAt(heroIndex);
        UpdateHeroSlots();
        UpdateDeckSlots();

        gameDataManager.UpdateHeroes(MyHeroes);
        GameManager_2.Instance.UpdateHeroDeckPrefab(Deck.Count - 1, hero.id);
    }

    public void RemoveHeroFromDeck(int deckIndex)
    {
        if (deckIndex < 0 || deckIndex >= Deck.Count)
            return;

        HeroInfo hero = Deck[deckIndex];
        MyHeroes.Add(hero);
        Deck.RemoveAt(deckIndex);
        UpdateHeroSlots();
        UpdateDeckSlots();

        gameDataManager.UpdateHeroes(MyHeroes);

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

    public void RemoveHeroFromMyHeroes(HeroInfo hero)
    {
        MyHeroes.Remove(hero);
        UpdateHeroSlots();
        gameDataManager.UpdateHeroes(MyHeroes);
    }
}