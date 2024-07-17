using System.Collections.Generic;
using UnityEngine;

public class GameManager_2 : MonoBehaviour
{
    private static GameManager_2 instance;
    public static GameManager_2 Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager_2");
                instance = go.AddComponent<GameManager_2>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private DungeonManager dungeonManager;
    private HeroManager heroManager;

    [SerializeField] private GameObject HeroPrefab_1;
    [SerializeField] private GameObject HeroPrefab_2;
    [SerializeField] private GameObject HeroPrefab_3;
    [SerializeField] private GameObject HeroPrefab_4;
    [SerializeField] private Dictionary<int, GameObject> heroPrefabs = new Dictionary<int, GameObject>();
    [SerializeField] public List<GameObject> HeroDeckPrefab = new List<GameObject>();
    [SerializeField] private List<GameObject> HeroDeckPrefabLive = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeHeroPrefabs();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        heroManager = FindObjectOfType<HeroManager>();
        Invoke("InitializeHeroPrefabs", 0.1f);

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
            for (int i = 0; i < heroManager.AllHeroes.Count; i++)
            {
                GameObject prefab = null;
                switch (i)
                {
                    case 0: prefab = HeroPrefab_1; break;
                    case 1: prefab = HeroPrefab_2; break;
                    case 2: prefab = HeroPrefab_3; break;
                    case 3: prefab = HeroPrefab_4; break;
                }

                if (prefab != null)
                {
                    heroPrefabs[heroManager.AllHeroes[i].id] = prefab;
                }
            }
        }
    }

    public void CreateHeroPrefabAtPosition(Vector3 position, int slotIndex)
    {
        //if (heroPrefabs.TryGetValue(heroId, out GameObject prefab))
        //{
        //    if (prefab != null)
        //    {
        //        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        //    }
        //}
        //if (slotIndex >= 0 && slotIndex < HeroDeckPrefab.Length && HeroDeckPrefab[slotIndex] != null)
        //{
        //    GameObject instance = Instantiate(HeroDeckPrefab[slotIndex], position, Quaternion.identity);
        //}

        if (slotIndex >= 0 && slotIndex < HeroDeckPrefab.Count && HeroDeckPrefab[slotIndex] != null)
        {
            GameObject instance = Instantiate(HeroDeckPrefab[slotIndex], position, Quaternion.identity);
            Character hero = instance.GetComponent<Character>();
            if (hero != null)
            {
                hero.gameObject.tag = "Hero";
                hero.gameObject.SetActive(true);

                // 던전 매니저를 찾아 영웅을 추가합니다.
                Dungeon dungeon = FindObjectOfType<Dungeon>();
                if (dungeon != null)
                {
                    dungeon.AddHeroToBattlefield(hero);
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