using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    [System.Serializable]
    public class Hero
    {
        public int id;
        public string name;
        public HeroClass heroClass;
        public CharacteristicType characteristicType;
        public int level;
        public float currentExp;
        public float neededExp;
        public int strength; // 추가
        public int agility; // 추가
        public int intelligence; // 추가
        public int stamina; // 추가
        public int power;
        public int speed;
        public int hp;
        public Sprite sprite;
        public List<string> equippedItemIds = new List<string>();

        public void LevelUp()
        {
            level++;
            // 스탯 증가 로직 추가
            power += 2;
            speed += 1;
            hp += 10;
            neededExp *= 1.2f;
        }

        public void EquipItem(string itemId)
        {
            if (!equippedItemIds.Contains(itemId))
            {
                equippedItemIds.Add(itemId);
                // 아이템 효과 적용 로직 추가
            }
        }

        public void UnequipItem(string itemId)
        {
            if (equippedItemIds.Remove(itemId))
            {
                // 아이템 효과 제거 로직 추가
            }
        }
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
        LoadHeroesFromGameData();  // AllHeroes와 MyHeroes 초기화
        InitializeDeckSlots();     // 기존 코드 유지
        UpdateHeroSlots();         // UI 업데이트
        UpdateDeckSlots();         // UI 업데이트

        GameDataManager.HeroesUpdated += OnHeroesUpdated;  // 데이터 변경 이벤트 구독
    }
    private void OnDestroy()
    {
        GameDataManager.HeroesUpdated -= OnHeroesUpdated;
    }

    private void LoadHeroesFromGameData()
    {
        List<HeroInfo> savedHeroes = GameDataManager.instance.GetAllHeroes();
        AllHeroes.Clear();
        MyHeroes.Clear();

        foreach (HeroInfo heroInfo in savedHeroes)
        {
            Hero hero = CreateHeroFromInfo(heroInfo);
            AllHeroes.Add(hero);
            MyHeroes.Add(hero);
        }

        if (AllHeroes.Count == 0)
        {
            InitializeDefaultHeroes();
        }
    }

    private Hero CreateHeroFromInfo(HeroInfo heroInfo)
    {
        return new Hero
        {
            id = heroInfo.id,
            name = heroInfo.heroName,
            heroClass = heroInfo.heroClass,
            characteristicType = heroInfo.characteristicType,
            level = heroInfo.level,
            currentExp = heroInfo.currentExp,
            neededExp = heroInfo.neededExp,
            strength = heroInfo.strength,
            agility = heroInfo.agility,
            intelligence = heroInfo.intelligence,
            stamina = heroInfo.stamina,
            hp = heroInfo.hp,
            //attackDamage = heroInfo.attackDamage,
            //defense = heroInfo.defense,
            //magicResistance = heroInfo.magicResistance,
            //attackSpeed = heroInfo.attackSpeed,
            //healthRegen = heroInfo.healthRegen,
            //energyRegen = heroInfo.energyRegen,
            //attackRange = heroInfo.attackRange,
            sprite = Resources.Load<Sprite>($"Images/Heroes/{heroInfo.heroClass}"),
            equippedItemIds = heroInfo.equippedItemIds
        };
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
        Hero newHero = CreateHeroFromInfo(newHeroInfo);
        AllHeroes.Add(newHero);
        MyHeroes.Add(newHero);
        UpdateHeroSlots();
    }

    public void LevelUpHero(int heroIndex)
    {
        if (heroIndex >= 0 && heroIndex < MyHeroes.Count)
        {
            Hero hero = MyHeroes[heroIndex];
            hero.LevelUp();
            UpdateHeroInfo(hero);
            UpdateHeroSlots();
        }
    }

    public void EquipItemToHero(int heroIndex, string itemId)
    {
        if (heroIndex >= 0 && heroIndex < MyHeroes.Count)
        {
            Hero hero = MyHeroes[heroIndex];
            hero.EquipItem(itemId);
            UpdateHeroInfo(hero);
            UpdateHeroSlots();
        }
    }

    private void UpdateHeroInfo(Hero hero)
    {
        HeroInfo updatedInfo = new HeroInfo(hero.name, hero.heroClass, hero.characteristicType)
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
        UpdateHeroSlots();
        UpdateDeckSlots();
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