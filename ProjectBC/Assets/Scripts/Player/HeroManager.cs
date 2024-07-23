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
    [SerializeField] private Transform myHeroDeckSlotsParent;
    private HeroSlot[] heroSlots;
    private DeckSlot[] deckSlots;
    private BattleDeckSlot[] battleDeckSlots;
    private MyHeroSlot[] myHeroSlots;
    private int maxDeckSize = 4;

    private void Awake()
    {
        gameDataManager = FindObjectOfType<GameDataManager>();
        if (gameDataManager == null)
        {
            Debug.LogError("GameDataManager not found in the scene!");
            return;
        }
        LoadMyHeroes();

    }

    private void Start()
    {
        InitializeSlots();
        InitializeAllHeroes();
        //InitializeMyHeroes();
        UpdateHeroSlots();
        UpdateDeckSlots();
        UpdateMyHeroSlots();
       
    }

    //private void InitializeAllHeroes()
    //{
    //    // �⺻ ����� ���ø� �ʱ�ȭ
    //    AllHeroes.Add(new HeroInfo { id = 1001, heroName = "Warrior", level = 1, attackDamage = 10, agility = 5, hp = 150, spritePath = "Images/currency/Gemstone" });
    //    AllHeroes.Add(new HeroInfo { id = 2001, heroName = "Priest", level = 1, attackDamage = 5, agility = 7, hp = 100, spritePath = "Images/currency/GreenGemstone" });
    //    // ... �ٸ� �⺻ ����� �߰� ...
    //}
    private void LoadMyHeroes()
    {
        Debug.Log("LoadMyHeroes started");
        List<HeroInfo> loadedHeroes = gameDataManager.GetAllHeroes();
        Debug.Log($"LoadMyHeroes: Loaded {loadedHeroes.Count} heroes from GameDataManager");

        AllHeroes = new List<HeroInfo>(loadedHeroes);
        MyHeroes = new List<HeroInfo>(loadedHeroes);

        foreach (var hero in MyHeroes)
        {
            Debug.Log($"Hero: {hero.heroName}, Class: {hero.heroClass}, ID: {hero.id}");
        }

        Debug.Log($"Before removing duplicates: AllHeroes = {AllHeroes.Count}, MyHeroes = {MyHeroes.Count}");

        // 중복 제거 로직 (필요한 경우)
        MyHeroes = MyHeroes.Distinct().ToList();

        Debug.Log($"Final counts: AllHeroes = {AllHeroes.Count}, MyHeroes = {MyHeroes.Count}");
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
        myHeroSlots = myHeroDeckSlotsParent.GetComponentsInChildren<MyHeroSlot>();
    }
    private void InitializeAllHeroes()
    {
        //AllHeroes.Add(new HeroInfo { id = 1001, name = "Warrior", level = 1, power = 10, speed = 5, hp = 150, sprite = Resources.Load<Sprite>("Images/currency/Warrior") });
        //AllHeroes.Add(new Hero { id = 2001, name = "Priest", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/Priest") });
        //AllHeroes.Add(new Hero { id = 3001, name = "Wizard", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/Wizard") });
        //AllHeroes.Add(new Hero { id = 3002, name = "Archer", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/Archer") });
        //AllHeroes.Add(new Hero { id = 3003, name = "non-slot", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/YellowGemstone") });
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
                //myHeroSlots[i].SetMyHeroData(MyHeroes[i], i);
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

    public int GetHeroIdFromDeckSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < Deck.Count)
        {
            return Deck[slotIndex].id;
        }
        return -1; // ��ȿ���� ���� ID
    }
}