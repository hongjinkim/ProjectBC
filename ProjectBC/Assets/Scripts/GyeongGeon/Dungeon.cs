using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[Serializable]
public class Loot
{
    public string id;
    public float dropRate;
    public int minValue;
    public int maxValue;
    public GameObject droppedItemPrefab;

}
public class Dungeon : MonoBehaviour
{
    [Header("BasicInformation")]
    //public TilemapManagerGG tilemapManager;
    public string _themeCode;
    public string _themeName;
    public string _stageCode;
    public float _findTimer;
    public int _enemyQuantity;
    private int _randomEnemyIndex;
    private int _randomTileIndex;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    Camera mainCamera;

    [Header("ActiveList")]
    public List<Character> _activeEnemyList = new List<Character>();
    public List<Character> _allCharacterList = new List<Character>();

    [Header("UI_StageSlot")]
    public string _stageName;
    public Sprite _stageImage;
    public int _navigationProgress;

    [Header("UI_StageInformation")]
    public List<Enemy> _enemyPool = new List<Enemy>();
    public List<Item> _ItemList = new List<Item>();
    public List<Character> _activeHeroList = new List<Character>();

    [Header("Item Loot System")]
    public List<Loot> LootTable = new List<Loot>();
    public Sprite goldSprite;
    [SerializeField] private List<Item> droppedItems = new List<Item>();
    [SerializeField] private List<GameObject> droppedPrefabs = new List<GameObject>();
    [SerializeField] private int droppedGolds = 0;
    [SerializeField] private float totalDropRate = 0;
    public GameObject lootPrefab;

    [Header("ItemPickupText")]
    public ObjectPoolBehaviour objectPool;
    [SerializeField] private float fadeDuration = 2.0f;
    private WaitForSeconds wait => new WaitForSeconds(fadeDuration);

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    private void Awake()
    {
        foreach(Loot loot in LootTable)
        {
            loot.droppedItemPrefab = lootPrefab;
        }

        mainCamera = Camera.main;
    }
    void Start()
    {   
        //tilemapManager.CalculateTileCenters();
        // 테스트용
        //SetHeroList();

        
        // if (mainCamera != null)
        // {
        //     Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        //     Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));
            
        //     spawnAreaMin = new Vector2(bottomLeft.x, bottomLeft.y);
        //     spawnAreaMax = new Vector2(topRight.x, topRight.y);
        // }

        SetEnemyList();
        DungeonInit();
        InvokeRepeating("OnPickupItem", 0f, 5f);
    }

    // 테스트용
    //public List<Character> _heroPool = new List<Character>();
    // void SetHeroList()
    // {
    //    for (var i = 0; i < _heroPool.Count; i++)
    //    {
    //        Character hero = Instantiate(_heroPool[i]);

    //        _activeHeroList.Add(hero);
    //        _allCharacterList.Add(hero);

    //        _activeHeroList[i].gameObject.tag = "Hero";

    //        // 추후에 마우스로 드래그해서 SetActive 하는걸로 변경해야함.
    //        _activeHeroList[i].gameObject.SetActive(true);
    //        //hero.customTilemapManager.allCharacters.Add(hero);
    //    }
    // }

    void Update()
    {
        CheckAllEnemiesDead();
    }

    public void DungeonInit()
    {
        foreach (var character in _allCharacterList)
        {
            character.dungeon = this;
            //character.tilemapManager = tilemapManager;
        }
    }

    public Character GetTarget(Character unit)
    {
        Character targetUnit = null;
        List<Character> targetList = unit.CompareTag("Hero") ? _activeEnemyList : _activeHeroList;
        float closestDistance = float.MaxValue;

        foreach (var target in targetList)
        {
            if (target == null || !target.gameObject.activeInHierarchy || target._unitState == Character.UnitState.death)
            {
                continue;
            }

            float distanceSquared = ((Vector2)target.transform.localPosition - (Vector2)unit.transform.localPosition).sqrMagnitude;

            if (distanceSquared <= unit.findRange * unit.findRange && distanceSquared < closestDistance)
            {
                targetUnit = target;
                closestDistance = distanceSquared;
            }
        }

        return targetUnit;
    }

    void CheckAllEnemiesDead()
    {
        bool allDead = true;

        foreach (var enemy in _activeEnemyList)
        {
            if (enemy != null && !(enemy._unitState == Character.UnitState.death))
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            List<Character> enemiesToDestroy = new List<Character>();

            foreach (var enemy in _activeEnemyList)
            {
                if (enemy != null)
                {
                    enemiesToDestroy.Add(enemy);
                }
            }

            // 임시로 넣어놓은것이다. 추후에 히어로 추가되고 List에 담기게 해야 된다.
            // Character a = _allCharacterList[0];
            _allCharacterList.Clear();
            // _allCharacterList.Add(a);

            foreach (var hero in _activeHeroList)
            {
                if(hero.gameObject.activeInHierarchy)
                {
                    _allCharacterList.Add(hero);
                }
            }

            foreach (var enemy in enemiesToDestroy)
            {
                Destroy(enemy.gameObject);
            }

            _activeEnemyList.Clear();
            
            SetEnemyList();
            DungeonInit();
        }
    }

    void SetEnemyList()
    {
        for(var i = 0; i < _enemyQuantity; i++)
        {
            _randomEnemyIndex = Random.Range(0, _enemyPool.Count);
            Enemy enemy = Instantiate(_enemyPool[_randomEnemyIndex]);

            _activeEnemyList.Add(enemy);
            _allCharacterList.Add(enemy);

            _activeEnemyList[i].gameObject.tag = "Enemy";

            // _randomTileIndex = Random.Range(0, tilemapManager.tileCenters.Count);
            // enemy.transform.position = tilemapManager.tileCenters[_randomTileIndex];

            TilemapCollider2D collider = this.transform.GetChild(0).GetChild(0).GetComponent<TilemapCollider2D>();
            Vector2 min = collider.bounds.min;
            Vector2 max = collider.bounds.max;

            // 추후에 position 변경해야함
            Vector2 randomPosition = new Vector2(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
            );

            enemy.transform.position = randomPosition;

            _activeEnemyList[i].gameObject.SetActive(true);
        }
    }

    public void AddHeroToBattlefield(Character hero)
    {
        if (hero != null)
        {
            RemoveHeroFromAllDungeons(hero);

            _activeHeroList.Add(hero);
            _allCharacterList.Add(hero);

            hero.dungeon = this;
            //hero.tilemapManager = tilemapManager;

            // 필요한 경우 추가 초기화
            // hero.Initialize();
        }
    }
    //private void RemoveHeroFromAllDungeons(Character hero)
    //{
    //    // 모든 던전을 순회하며 해당 영웅을 제거
    //    Dungeon[] allDungeons = FindObjectsOfType<Dungeon>();
    //    foreach (Dungeon dungeon in allDungeons)
    //    {
    //        dungeon._activeHeroList.RemoveAll(h => h == hero);
    //        dungeon._allCharacterList.RemoveAll(c => c == hero);
    //    }
    //}

    private void RemoveHeroFromAllDungeons(Character hero) //에러가 나와서 포기?
    {
        // _allDungeonList를 순회하며 해당 영웅을 제거합니다.
        foreach (Dungeon dungeon in GameManager.instance.dungeonManager._allDungeonList)
        {
            dungeon._activeHeroList.Remove(hero);
            dungeon._allCharacterList.Remove(hero);
        }
    }


    /// Loot System
    public void GetDroppedItem(Transform transform)
    {
        if (totalDropRate == 0)
        {
            foreach (Loot i in LootTable)
            {
                totalDropRate += i.dropRate;
            }
        }
        float randomNumber = Random.value * totalDropRate;
        float cumulativeRate = 0f;
        foreach (Loot i in LootTable)
        {
            cumulativeRate += i.dropRate;
            if (randomNumber <= cumulativeRate)
            {
                if (i.id == "none")
                    return;
                else if (i.id == "gold")
                {
                    droppedGolds += Random.Range(i.minValue, i.maxValue);

                    SpriteRenderer renderer = i.droppedItemPrefab.GetComponent<SpriteRenderer>();
                    renderer.sprite = goldSprite;
                    // 추후 오브젝트 풇로 변경
                    droppedPrefabs.Add(Instantiate(i.droppedItemPrefab, transform.position, Quaternion.identity));
                }
                else
                {
                    var item = new Item(i.id);
                    item = RandomStat(item);

                    droppedItems.Add(item);

                    SpriteRenderer renderer = i.droppedItemPrefab.GetComponent<SpriteRenderer>();
                    renderer.sprite = ItemCollection.active.GetItemIcon(item).sprite;
                    // 추후 오브젝트 풇로 변경
                    droppedPrefabs.Add(Instantiate(i.droppedItemPrefab, transform.position, Quaternion.identity));
                }
                return;
            }
        }
        return;
    }

    public void OnPickupItem()
    {
        //골드 획득
        if (droppedGolds > 0)
        {
            GameDataManager.instance.playerInfo.gold += droppedGolds;
            StartCoroutine(PickupNotice(droppedGolds.ToString() + " 골드를 획득 했습니다."));
            EventManager.instance.TriggerEvent(EventType.FundsUpdated, null);
        }

        // 아이템 획득
        var inventory = GameDataManager.instance.playerInfo.items;
        if(droppedItems != null)
        {
            foreach (Item item in droppedItems)
            {
                if (item.Params.Type == ItemType.Usable || item.Params.Type == ItemType.Material || item.Params.Type == ItemType.Crystal || item.Params.Type == ItemType.Exp)
                {
                    bool hasItem = false;
                    foreach (Item _item in inventory)
                    {
                        if (item.Params.Id == _item.Params.Id)
                        {
                            _item.Count++;
                            hasItem = true;
                            break;
                        }
                    }
                    if (!hasItem)
                    {
                        StartCoroutine(PickupNotice(item.Params.Name + "을(를) 획득 했습니다"));
                        inventory.Add(item);
                    }

                }
                else
                {
                    StartCoroutine(PickupNotice(item.Params.Name + "을(를) 획득 했습니다"));
                    inventory.Add(item);
                }
            }
            EventManager.instance.TriggerEvent(EventType.ItemUpdated, null);
        }
        
        // 필드 위애 아이템 표시 모두 제거
        foreach (GameObject go in droppedPrefabs)
        {
            Destroy(go);
        }

        //
        droppedGolds = 0;
        droppedItems.Clear();
        droppedPrefabs.Clear();


        if (ExpScroll.Instance != null)
        {
            ExpScroll.Instance.UpdateExpScrollCount();
        }
        if (HeroPotion.Instance != null)
        {
            HeroPotion.Instance.UpdatePotionCount();
        }

        
        
    }
    

    public Item RandomStat(Item item)
    {
        if (item.IsEquipment)
        {
            var statData = GameDataManager.instance.equipmentStatData[item.Params.Index];
            item.Stat = new Stat(statData.BasicStats);

            for(int i = 0; i < item.Stat.basic.Count; i++)
            {
                item.LuckyPoint += item.Stat.basic[i].value - item.Stat.basic[i].minValue;
                item.LuckyPercent += item.LuckyPoint*100 / item.Stat.basic[i].maxValue / item.Stat.basic.Count;
            }

            var rarity = item.Params.Rarity;
            List<MagicStat> enumValues = new List<MagicStat>((MagicStat[])Enum.GetValues(typeof(MagicStat)));

            for (int i = 0; i<= (int)(rarity); i++)
            {
                var randomIndex = Random.Range(0, enumValues.Count);

                item.Stat.magic.Add(new Magic { id = (MagicStat)enumValues[randomIndex], value = 1/*추후 값  수정*/}) ;
                enumValues.RemoveAt(randomIndex);
            }
        }
        return item;
    }

    IEnumerator PickupNotice(string text)
    {
        GameObject textObject = objectPool.GetPooledObject();
        textObject.GetComponent<ItemPickupText>().SetText(text);
        textObject.SetActive(true);

        yield return wait;

        textObject.SetActive(false);
    }


}
