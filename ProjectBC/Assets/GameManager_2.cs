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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private DungeonManager dungeonManager;
    private HeroManager heroManager;

    [SerializeField] private GameObject HeroPrefab_1;
    [SerializeField] private GameObject HeroPrefab_2;
    [SerializeField] private GameObject HeroPrefab_3;
    [SerializeField] private GameObject HeroPrefab_4;
    [SerializeField] private Dictionary<int, GameObject> heroPrefabs = new Dictionary<int, GameObject>();
    [SerializeField] private List<GameObject> prefabList;


    private void Start()
    {
        heroManager = FindObjectOfType<HeroManager>();
        InitializeHeroPrefabs();

        if (prefabList == null || prefabList.Count == 0)
        {
            prefabList = new List<GameObject> { HeroPrefab_1, HeroPrefab_2, HeroPrefab_3, HeroPrefab_4 };
        }
    }

    private void InitializeHeroPrefabs()
    {
        heroPrefabs.Clear();
        for (int i = 0; i < heroManager.AllHeroes.Count; i++)
        {
            if (i < prefabList.Count && prefabList[i] != null)
            {
                heroPrefabs[heroManager.AllHeroes[i].id] = prefabList[i];
            }
            else
            {
                Debug.LogWarning($"Missing prefab for hero ID: {heroManager.AllHeroes[i].id}");
            }
        }
    }

    public GameObject GetHeroPrefab(int id)
    {
        if (heroPrefabs.TryGetValue(id, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"Hero prefab with ID {id} not found.");
        return null;
    }

    public List<int> GetAllHeroIds()
    {
        return new List<int>(heroPrefabs.Keys);
    }
}
