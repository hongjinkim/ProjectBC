using System.Collections.Generic;
using UnityEngine;

public class GameManager_2 : MonoBehaviour
{
    private static GameManager_2 instance;
    public static GameManager_2 Instance
    {
        get { return instance; }
    }

    private DungeonManager dungeonManager;
    private HeroManager heroManager;
    private int maxDeckSize = 4;

    [SerializeField] private GameObject HeroPrefab_1;
    [SerializeField] private GameObject HeroPrefab_2;
    [SerializeField] private GameObject HeroPrefab_3;
    [SerializeField] private GameObject HeroPrefab_4;
    [SerializeField] private Dictionary<int, GameObject> heroPrefabs = new Dictionary<int, GameObject>();
    public List<GameObject> HeroDeckPrefab = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        HeroDeckPrefab = new List<GameObject>(new GameObject[maxDeckSize]);

        heroManager = FindObjectOfType<HeroManager>();
        Invoke("InitializeHeroPrefabs", 0.1f);

        // �ʱ� �� ���� �ݿ�
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
            GameObject[] prefabArray = { HeroPrefab_1, HeroPrefab_2, HeroPrefab_3, HeroPrefab_4 };
            for (int i = 0; i < heroManager.AllHeroes.Count && i < prefabArray.Length; i++)
            {
                GameObject prefab = prefabArray[i];
                if (prefab != null)
                {
                    int heroId = heroManager.AllHeroes[i].id; // 1�� �������� id = 1001
                    GameObject instance = Instantiate(prefab);

                    instance.GetComponent<Character>().info = GameDataManager.instance.playerInfo.heroes[i];

                    instance.SetActive(false);
                    heroPrefabs[heroId] = instance;

                }
            }
        }
    }


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

                    Dungeon dd = GameManager.Instance.dungeonManager._selectDungeon;
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