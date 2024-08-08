using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    
    private HeroManager heroManager;
    private DungeonManager dungeonManager;

    private int maxDeckSize = 4;

    [SerializeField] private GameObject KnightPrefab;
    [SerializeField] private GameObject WizardPrefab;
    [SerializeField] private GameObject PriestPrefab;
    [SerializeField] private GameObject ArcherPrefab;
    [SerializeField] private Dictionary<int, GameObject> heroPrefabs = new Dictionary<int, GameObject>();
    public List<GameObject> HeroDeckPrefab = new List<GameObject>();


    public Transform pickUpGrid;

    private void Start()
    {
        dungeonManager = DungeonManager.instance;
        heroManager = HeroManager.instance;

        HeroDeckPrefab = new List<GameObject>(new GameObject[maxDeckSize]);

        InitializeHeroPrefabs();
        // 초기 덱 상태 반영
        for (int i = 0; i < heroManager.Deck.Count; i++)
        {
            UpdateHeroDeckPrefab(i, heroManager.Deck[i].id);
        }
    }

    private void InitializeHeroPrefabs()
    {
        heroPrefabs.Clear();
        if (heroManager != null && heroManager.AllHeroes != null)
        {
            GameObject[] prefabArray = { KnightPrefab, WizardPrefab, PriestPrefab, ArcherPrefab };
            for (int i = 0; i < heroManager.AllHeroes.Count && i < prefabArray.Length; i++)
            {
                GameObject prefab = prefabArray[i];
                if (prefab != null)
                {
                    int heroId = heroManager.AllHeroes[i].id; // 1번 프리팹의 id = 1001
                    GameObject instance = Instantiate(prefab);

                    instance.GetComponent<Character>().info = GameDataManager.instance.playerInfo.heroes[i];

                    instance.SetActive(false);
                    heroPrefabs[heroId] = instance;

                }
            }
        }
    }

    //public void CreateHeroPrefabAtPosition(Vector3 position, int slotIndex)
    //{
    //    //if (heroPrefabs.TryGetValue(heroId, out GameObject prefab))
    //    //{
    //    //    if (prefab != null)
    //    //    {
    //    //        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
    //    //    }
    //    //}
    //    //if (slotIndex >= 0 && slotIndex < HeroDeckPrefab.Length && HeroDeckPrefab[slotIndex] != null)
    //    //{
    //    //    GameObject instance = Instantiate(HeroDeckPrefab[slotIndex], position, Quaternion.identity);
    //    //}

    //    if (slotIndex >= 0 && slotIndex < HeroDeckPrefab.Count && HeroDeckPrefab[slotIndex] != null)
    //    {
    //        GameObject instance = Instantiate(HeroDeckPrefab[slotIndex], position, Quaternion.identity);
    //        Character hero = instance.GetComponent<Character>();
    //        if (hero != null)
    //        {
    //            hero.gameObject.tag = "Hero";
    //            hero.gameObject.SetActive(true);

    //            // 던전 매니저를 찾아 영웅을 추가합니다.

    //            Dungeon dd = GameManager.Instance.dungeonManager._selectDungeon;

    //            //Dungeon dungeon = FindObjectOfType<Dungeon>();
    //            if (dd != null)
    //            {
    //                dd.AddHeroToBattlefield(hero);
    //            }

    //        }

    //    }
    //}

    public void CreateHeroPrefabAtPosition(Vector3 position, int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < HeroDeckPrefab.Count && HeroDeckPrefab[slotIndex] != null)
        {
            int heroId = heroManager.GetHeroIdFromDeckSlot(slotIndex);
            if (heroPrefabs.TryGetValue(heroId, out GameObject heroInstance))
            {
                heroInstance.transform.position = position;
                heroInstance.transform.rotation = Quaternion.identity;
                heroInstance.SetActive(true);

                Character hero = heroInstance.GetComponent<Character>();
                if (hero != null)
                {
                    hero.gameObject.tag = "Hero";

                    Dungeon dd = GameManager.instance.dungeonManager._selectDungeon;
                    if (dd != null)
                    {
                        dd.AddHeroToBattlefield(hero);
                    }
                }
            }
        }
    }

    public GameObject GetHeroPrefab(int id)
    {
        if (heroPrefabs.TryGetValue(id, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }

    public List<int> GetAllHeroIds()
    {
        return new List<int>(heroPrefabs.Keys);
    }



    ///

    public void UpdateHeroDeckPrefab(int index, int heroId)
    {
        if (heroPrefabs.TryGetValue(heroId, out GameObject prefab))
        {
            HeroDeckPrefab[index] = prefab;
        }
    }
}