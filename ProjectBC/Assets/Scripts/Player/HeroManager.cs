using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{



    private List<HeroInfo> AllHeroes = new List<HeroInfo>();
    [SerializeField] private List<HeroInfo> MyHeroes = new List<HeroInfo>();
    [SerializeField] private List<HeroInfo> Deck = new List<HeroInfo>();

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
    [SerializeField] private List<Hero> Deck = new List<Hero>();

    [SerializeField] private Transform heroSlotsParent;
    [SerializeField] private Transform deckSlotsParent;
    [SerializeField] private Transform battleDeckSlotsParent;
    private HeroSlot[] heroSlots;
    private DeckSlot[] deckSlots;
    private BattleDeckSlot[] battleDeckSlots;
    private int maxDeckSize = 4;

    private void Awake()
    {
        InitializeSlots();
    }

    private void Start()
    {
        InitializeAllHeroes();

        LoadHeroesFromGameData();  // AllHeroes와 MyHeroes 초기화
        //InitializeDeckSlots();     // 기존 코드 유지
        UpdateHeroSlots();         // UI 업데이트
        UpdateDeckSlots();         // UI 업데이트

        GameDataManager.HeroesUpdated += OnHeroesUpdated;  // 데이터 변경 이벤트 구독
        Debug.Log($"Loaded {AllHeroes.Count} heroes, {MyHeroes.Count} in my heroes.");

    }
    private void OnDestroy()
    {
        GameDataManager.HeroesUpdated -= OnHeroesUpdated;
    }

    private void LoadHeroesFromGameData()
    {
        List<HeroInfo> savedHeroes = GameDataManager.instance.GetAllHeroes();
        if (savedHeroes == null || savedHeroes.Count == 0)
        {
            InitializeAllHeroes();
            SaveHeroesToGameData(); // 초기화된 히어로 데이터를 저장
        }
        else
        {
            AllHeroes.Clear();
            MyHeroes.Clear();



            foreach (HeroInfo heroInfo in savedHeroes)
            {
                HeroInfo hero = CreateHeroFromInfo(heroInfo);
                AllHeroes.Add(hero);
                MyHeroes.Add(hero);
            }

            if (AllHeroes.Count == 0)
            {
                InitializeDefaultHeroes();
            }
        }
    }
    private void SaveHeroesToGameData()
    {
        foreach (HeroInfo hero in AllHeroes)
        {
            GameDataManager.instance.AddHero(hero);
        }
    }
    private HeroInfo CreateHeroFromInfo(HeroInfo heroInfo)
    {
        // HeroInfo는 이미 모든 필요한 정보를 가지고 있으므로, 그대로 반환
        return heroInfo;
    }


    private void InitializeDefaultHeroes()
    {
        CreateNewHero("Warrior", HeroClass.Knight, CharacteristicType.MuscularStrength);
        CreateNewHero("Archer", HeroClass.Archer, CharacteristicType.Agility);
        CreateNewHero("Wizard", HeroClass.Wizard, CharacteristicType.Intellect);
        CreateNewHero("Priest", HeroClass.Priest, CharacteristicType.Intellect);
    }

    public void CreateNewHero(string name, HeroClass heroClass, CharacteristicType characteristicType)
    {
        HeroInfo newHeroInfo = new HeroInfo(name, heroClass, characteristicType);
        GameDataManager.instance.AddHero(newHeroInfo);
        HeroInfo newHero = CreateHeroFromInfo(newHeroInfo);
        AllHeroes.Add(newHero);
        MyHeroes.Add(newHero);
        UpdateHeroSlots();
    }

    public void LevelUpHero(int heroIndex)
    {
        if (heroIndex >= 0 && heroIndex < MyHeroes.Count)
        {
            HeroInfo hero = MyHeroes[heroIndex];
            hero.LevelUp();
            UpdateHeroInfo(hero);
            UpdateHeroSlots();
        }
    }

    public void EquipItemToHero(int heroIndex, string itemId)
    {
        if (heroIndex >= 0 && heroIndex < MyHeroes.Count)
        {
            HeroInfo hero = MyHeroes[heroIndex];
            hero.EquipItem(itemId);
            UpdateHeroInfo(hero);
            UpdateHeroSlots();
        }
    }

    private void UpdateHeroInfo(HeroInfo hero)
    {
        HeroInfo updatedInfo = new HeroInfo(hero.heroName, hero.heroClass, hero.characteristicType)
        {
            id = hero.id,
            level = hero.level,
            currentExp = hero.currentExp,
            neededExp = hero.neededExp,
            strength = hero.strength,
            agility = hero.agility,
            intelligence = hero.intelligence,
            stamina = hero.stamina,
            hp = hero.hp,
            //attackDamage = hero.attackDamage,
            //defense = hero.defense,
            //magicResistance = hero.magicResistance,
            //attackSpeed = hero.attackSpeed,
            //healthRegen = hero.healthRegen,
            //energyRegen = hero.energyRegen,
            //attackRange = hero.attackRange,
            equippedItemIds = hero.equippedItemIds
        };
        GameDataManager.instance.UpdateHero(updatedInfo);
    }

    private void OnHeroesUpdated(List<HeroInfo> heroes)
    {
        LoadHeroesFromGameData();

        InitializeMyHeroes();

        UpdateHeroSlots();
        UpdateDeckSlots();
    }
    private void InitializeSlots()
    {
        heroSlots = heroSlotsParent.GetComponentsInChildren<HeroSlot>();
        deckSlots = deckSlotsParent.GetComponentsInChildren<DeckSlot>();
        battleDeckSlots = battleDeckSlotsParent.GetComponentsInChildren<BattleDeckSlot>();
    }
    private void InitializeAllHeroes()
    {

        AllHeroes.Clear();
        AllHeroes.Add(new HeroInfo("Warrior", HeroClass.Knight, CharacteristicType.MuscularStrength)
        {
            id = 1,
            level = 1,
            attackDamage = 10,
            agility = 5,
            hp = 150
        });
        AllHeroes.Add(new HeroInfo("Priest", HeroClass.Priest, CharacteristicType.Intellect)
        {
            id = 2,
            level = 1,
            attackDamage = 5,
            agility = 7,
            hp = 100
        });
        AllHeroes.Add(new HeroInfo("Archer", HeroClass.Archer, CharacteristicType.Agility)
        {
            id = 3,
            level = 1,
            attackDamage = 5,
            agility = 7,
            hp = 100
        });
        MyHeroes.Clear();
        MyHeroes.AddRange(AllHeroes); // 모든 히어로를 MyHeroes에도 추가

        AllHeroes.Add(new Hero { id = 1001, name = "Warrior", level = 1, power = 10, speed = 5, hp = 150, sprite = Resources.Load<Sprite>("Images/currency/Gemstone") });
        AllHeroes.Add(new Hero { id = 2001, name = "Priest", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/GreenGemstone") });
        AllHeroes.Add(new Hero { id = 3001, name = "Archer", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/PurpleGemstone") });
        AllHeroes.Add(new Hero { id = 3002, name = "Assassin", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/RedGemstone") });
        AllHeroes.Add(new Hero { id = 3003, name = "Tanker", level = 1, power = 5, speed = 7, hp = 100, sprite = Resources.Load<Sprite>("Images/currency/YellowGemstone") });

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

            if (heroSlots[i] == null)
                continue;

            if (i < MyHeroes.Count)
                heroSlots[i].SetHeroData(MyHeroes[i], i);
            else
                heroSlots[i].SetHeroData(null, -1);

            if (i < MyHeroes.Count)
                heroSlots[i].SetHeroData(MyHeroes[i], i);
            else
                heroSlots[i].ClearSlot();

        }
    }

    private void UpdateDeckSlots()
    {

        if (deckSlots == null || deckSlots.Length == 0)
            return;

        for (int i = 0; i < deckSlots.Length; i++)
        {
            if (deckSlots[i] == null)
                continue;

            if (i < Deck.Count)
                deckSlots[i].DeckSetHeroData(Deck[i], i);
            else
                deckSlots[i].DeckSetHeroData(null, -1);

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
                battleDeckSlots[i].SetHeroData(Deck[i]);
            else
                battleDeckSlots[i].ClearSlot();
        }
    }

    public void AddHeroToDeck(int heroIndex)
    {

        if (heroIndex < 0 || heroIndex >= MyHeroes.Count)
            return;

        HeroInfo hero = MyHeroes[heroIndex];

        if (Deck.Count >= maxDeckSize || Deck.Contains(hero))
            return;


        if (heroIndex < 0 || heroIndex >= MyHeroes.Count || Deck.Count >= maxDeckSize)
            return;

        Hero hero = MyHeroes[heroIndex];

        Deck.Add(hero);
        MyHeroes.RemoveAt(heroIndex);
        UpdateHeroSlots();
        UpdateDeckSlots();
    }

    public void RemoveHeroFromDeck(int deckIndex)
    {

        if (deckIndex >= 0 && deckIndex < Deck.Count)
        {
            HeroInfo removedHero = Deck[deckIndex];
            Deck.RemoveAt(deckIndex);
            MyHeroes.Add(removedHero);
            UpdateDeckSlots();
            UpdateHeroSlots();
        }

        if (deckIndex < 0 || deckIndex >= Deck.Count)
            return;

        Hero hero = Deck[deckIndex];
        MyHeroes.Add(hero);
        Deck.RemoveAt(deckIndex);
        UpdateHeroSlots();
        UpdateDeckSlots();

    }

    public void RemoveHeroFromMyHeroes(HeroInfo hero)
    {
        MyHeroes.Remove(hero);
        UpdateHeroSlots();
    }
    public void ResetHeroData()
    {
        GameDataManager.instance.ResetHeroData();
        LoadHeroesFromGameData();
        UpdateHeroSlots();
        UpdateDeckSlots();
    }
}