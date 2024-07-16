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
        public int strength; // �߰�
        public int agility; // �߰�
        public int intelligence; // �߰�
        public int stamina; // �߰�
        public int power;
        public int speed;
        public int hp;
        public Sprite sprite;
        public List<string> equippedItemIds = new List<string>();

        public void LevelUp()
        {
            level++;
            // ���� ���� ���� �߰�
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
                // ������ ȿ�� ���� ���� �߰�
            }
        }

        public void UnequipItem(string itemId)
        {
            if (equippedItemIds.Remove(itemId))
            {
                // ������ ȿ�� ���� ���� �߰�
            }
        }
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
        LoadHeroesFromGameData();  // AllHeroes�� MyHeroes �ʱ�ȭ
        InitializeDeckSlots();     // ���� �ڵ� ����
        UpdateHeroSlots();         // UI ������Ʈ
        UpdateDeckSlots();         // UI ������Ʈ

        GameDataManager.HeroesUpdated += OnHeroesUpdated;  // ������ ���� �̺�Ʈ ����
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
    private void InitializeSlots()
    {
        heroSlots = heroSlotsParent.GetComponentsInChildren<HeroSlot>();
        deckSlots = deckSlotsParent.GetComponentsInChildren<DeckSlot>();
        battleDeckSlots = battleDeckSlotsParent.GetComponentsInChildren<BattleDeckSlot>();
    }
    private void InitializeAllHeroes()
    {
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
            if (i < MyHeroes.Count)
                heroSlots[i].SetHeroData(MyHeroes[i], i);
            else
                heroSlots[i].ClearSlot();
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
                battleDeckSlots[i].SetHeroData(Deck[i]);
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
    }
    public void RemoveHeroFromMyHeroes(Hero hero)
    {
        MyHeroes.Remove(hero);
        UpdateHeroSlots();
    }
}