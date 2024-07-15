using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour
{
    public Canvas canvasPrefab;
    public Canvas canvas;

    public List<Character> _heroPool = new List<Character>();
    public List<Character> _enemyPool = new List<Character>();
    public List<Character> _activeHeroList = new List<Character>();
    public List<Character> _activeEnemyList = new List<Character>();
    public List<Character> _allCharacterList = new List<Character>();

    public float _findTimer;
    private int randomEnemyIndex;
    private int randomeTileIndex;
    public int enemyQuantity;

    // 랜덤 위치 범위 설정
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    private void Awake() 
    {
        GameManager.Instance.dungeonManager = this;

        canvas = Instantiate(canvasPrefab);
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));
            
            spawnAreaMin = new Vector2(bottomLeft.x, bottomLeft.y);
            spawnAreaMax = new Vector2(topRight.x, topRight.y);
        }

    }

    private void Start()
    {
        SetHeroList();
        SetEnemyList();
    }

    private void Update() 
    {
        CheckAllEnemiesDead();
    }

    // void SetUnitList()
    // {
    //     _ActiveHeroList.Clear();
    //     _ActiveEnemyList.Clear();

    //     //_heroUnitList[0].

    //     for(var i = 0; i < _unitPool.Count; i++)
    //     {
    //         for(var j = 0; j < _unitPool[i].childCount; j++)
    //         {
    //             switch(i)
    //             {
    //                 case 0:
    //                     _ActiveHeroList.Add(_unitPool[i].GetChild(j).GetComponent<Character>());
    //                     _unitPool[i].GetChild(j).gameObject.tag = "Hero";
    //                     break;

    //                 case 1:
    //                     _ActiveEnemyList.Add(_unitPool[i].GetChild(j).GetComponent<Character>());
    //                     _unitPool[i].GetChild(j).gameObject.tag = "Enemy";
    //                     break;
    //             }
    //         }
    //     }
    // }

    void SetHeroList()
    {
        for (var i = 0; i < _heroPool.Count; i++)
        {
            Character hero = Instantiate(_heroPool[i]);

            _activeHeroList.Add(hero);
            _allCharacterList.Add(hero);

            _activeHeroList[i].gameObject.tag = "Hero";

            // 추후에 마우스로 드래그해서 SetActive 하는걸로 변경해야함.
            _activeHeroList[i].gameObject.SetActive(true);
            //hero.customTilemapManager.allCharacters.Add(hero);
        }
    }

    void SetEnemyList()
    {
        for(var i = 0; i < enemyQuantity; i++)
        {
            randomEnemyIndex = Random.Range(0, _enemyPool.Count);
            Character enemy = Instantiate(_enemyPool[randomEnemyIndex]);

            _activeEnemyList.Add(enemy);
            _allCharacterList.Add(enemy);

            _activeEnemyList[i].gameObject.tag = "Enemy";

            randomeTileIndex = Random.Range(0, TilemapManagerGG.Instance.tileCenters.Count);
            enemy.transform.position = TilemapManagerGG.Instance.tileCenters[randomeTileIndex];

            // 랜덤한 위치 생성
            // Vector2 randomPosition = new Vector2(
            //     Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            //     Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            // );
            //BoundsInt bounds = enemy.customTilemapManager.tilemap.cellBounds;

            //Vector2 randomPosition = GetRandomTilemapPosition(_ActiveEnemyList[i]);

            //enemy.transform.position = randomPosition;
            _activeEnemyList[i].gameObject.SetActive(true);
            //enemy.customTilemapManager.allCharacters.Add(enemy);

        }
    }

    Vector2 GetRandomTilemapPosition(Character enemy)
    {
        // 타일맵 영역 범위
        BoundsInt bounds = enemy.customTilemapManager.tilemap.cellBounds;

        // 랜덤한 위치 생성
        Vector2 randomPosition = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );

        // 랜덤 위치를 타일맵 기준으로 조정하여 유효한 위치로 반환
        return enemy.customTilemapManager.GetNearestValidPosition(randomPosition);
    }

    void CheckAllEnemiesDead()
    {
        // foreach (var enemy in _ActiveEnemyList)
        // {
        //     if (enemy != null && !(enemy._unitState == Character.UnitState.death))
        //     {
        //         return;
        //     }
        // }

        // foreach (var enemy in _ActiveEnemyList)
        // {
        //     Destroy(enemy);
        // }
        
        // _ActiveEnemyList.Clear();
        // SetEnemyList();

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

            // foreach (var enemy in _ActiveEnemyList)
            // {
            //     enemy.customTilemapManager.allCharacters.Clear();
            // }

            // foreach (var hero in _ActiveHeroList)
            // {
            //     hero.customTilemapManager.allCharacters.Clear();
            // }

            foreach (var enemy in enemiesToDestroy)
            {
                //_allCharacterList.Remove(enemy);
                Destroy(enemy.gameObject);
            }
            _allCharacterList.Clear();

            _activeEnemyList.Clear();
            
            SetEnemyList();
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

}
