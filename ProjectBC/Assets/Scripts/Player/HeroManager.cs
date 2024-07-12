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
        GameDataManager.HeroesUpdated += OnHeroesUpdated;
    }

    private void OnDestroy()
    {
        GameDataManager.HeroesUpdated -= OnHeroesUpdated;
    }

    private void InitializeHeroes()
    {
        // GameDataManager���� ����� ����� ������ �ε�
        List<HeroInfo> savedHeroes = GameDataManager.instance.GetAllHeroes();
        foreach (HeroInfo heroInfo in savedHeroes)
        {
            Player hero = CreateHeroFromInfo(heroInfo);
            AllHeroes.Add(hero);
        }

        // ����� �����Ͱ� ������ �⺻ ����� ����
        if (AllHeroes.Count == 0)
        {
            CreateDefaultHeroes();
        }
    }

    private Player CreateHeroFromInfo(HeroInfo heroInfo)
    {
        // HeroInfo�� ������� ������ Player ��ü ����
        // �� �κ��� Player Ŭ������ ������ ���� �����ؾ� �մϴ�
        // ����:
        switch (heroInfo.heroClass)
        {
            case HeroClass.Archer:
                return new Archer(heroInfo);
            case HeroClass.Wizard:
                return new Wizard(heroInfo);
            case HeroClass.Priest:
                return new Priest(heroInfo);
            case HeroClass.Knight:
                return new Knight(heroInfo);
            default:
                throw new System.ArgumentException("Unknown hero class");
        }
    }

    private void CreateDefaultHeroes()
    {
        CreateNewHero("Archer1", HeroClass.Archer, CharacteristicType.Agility);
        CreateNewHero("Wizard1", HeroClass.Wizard, CharacteristicType.Intellect);
        CreateNewHero("Priest1", HeroClass.Priest, CharacteristicType.Intellect);
        CreateNewHero("Knight1", HeroClass.Knight, CharacteristicType.MuscularStrength);
    }

    public void CreateNewHero(string name, HeroClass heroClass, CharacteristicType characteristicType)
    {
        HeroInfo newHeroInfo = new HeroInfo(name, heroClass, characteristicType);
        GameDataManager.instance.AddHero(newHeroInfo);
        Player newHero = CreateHeroFromInfo(newHeroInfo);
        AllHeroes.Add(newHero);
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

    public void LevelUpHero(Player hero)
    {
        hero.LevelUp(1);
        UpdateHeroInfo(hero);
    }

    public void EquipItemToHero(Player hero, string itemId)
    {
        hero.EquipItem(itemId);
        UpdateHeroInfo(hero);
    }

    private void UpdateHeroInfo(Player hero)
    {
        HeroInfo updatedInfo = hero.GetHeroInfo(); // Player Ŭ������ �� �޼��� ���� �ʿ�
        GameDataManager.instance.UpdateHero(updatedInfo);
    }

    private void OnHeroesUpdated(List<HeroInfo> heroes)
    {
        // GameDataManager���� ����� �����Ͱ� ������Ʈ�Ǹ� ȣ���
        // �ʿ��� ��� AllHeroes ����Ʈ ������Ʈ
        UpdateAllHeroes(heroes);
    }

    private void UpdateAllHeroes(List<HeroInfo> heroes)
    {
        AllHeroes.Clear();
        foreach (HeroInfo heroInfo in heroes)
        {
            Player hero = CreateHeroFromInfo(heroInfo);
            AllHeroes.Add(hero);
        }
        // ActiveHeroes ����Ʈ�� �ʿ信 ���� ������Ʈ
    }
}