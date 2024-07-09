using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour
{
    public Canvas canvasPrefab;
    public Canvas canvas;

    public List<Character> _heroPool = new List<Character>();
    public List<Character> _enemyPool = new List<Character>();
    public List<Character> _ActiveHeroList = new List<Character>();
    public List<Character> _ActiveEnemyList = new List<Character>();

    public float _findTimer;
    private int randomEnemyIndex;
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
        AllEnemiesDeadCheck();
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

            _ActiveHeroList.Add(hero);
            _ActiveHeroList[i].gameObject.tag = "Hero";

            // 추후에 마우스로 드래그해서 SetActive 하는걸로 변경해야함.
            _ActiveHeroList[i].gameObject.SetActive(true);
        }
    }

    void SetEnemyList()
    {
        for(var i = 0; i < enemyQuantity; i++)
        {
            randomEnemyIndex = Random.Range(0, _enemyPool.Count);
            Character enemy = Instantiate(_enemyPool[randomEnemyIndex]);

            _ActiveEnemyList.Add(enemy);
            _ActiveEnemyList[i].gameObject.tag = "Enemy";

            // 랜덤한 위치 생성
            // Vector2 randomPosition = new Vector2(
            //     Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            //     Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            // );
            //BoundsInt bounds = enemy.customTilemapManager.tilemap.cellBounds;

            //Vector2 randomPosition = GetRandomTilemapPosition(_ActiveEnemyList[i]);

            //enemy.transform.position = randomPosition;
            _ActiveEnemyList[i].gameObject.SetActive(true);
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

    void AllEnemiesDeadCheck()
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

        foreach (var enemy in _ActiveEnemyList)
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

            foreach (var enemy in _ActiveEnemyList)
            {
                if (enemy != null)
                {
                    enemiesToDestroy.Add(enemy);
                }
            }

            foreach (var enemy in enemiesToDestroy)
            {
                Destroy(enemy.gameObject);
            }

            _ActiveEnemyList.Clear();

            SetEnemyList();
        }
    }

    public Character GetTarget(Character unit)
    {
        Character targetUnit = null;

        List<Character> targetList = new List<Character>();

        switch (unit.tag)
        {
            case "Hero":
                targetList = _ActiveEnemyList;
                break;

            case "Enemy":
                targetList = _ActiveHeroList;
                break;
        }
        
        // closestDistance: 가장 가까운 유닛과의 거리를 저장하는 변수입니다. 초기값을 매우 큰 값(float.MaxValue)으로 설정하여 첫 번째 비교에서 무조건 갱신되도록 합니다.
        float closestDistance = float.MaxValue;

        for (var i = 0; i < targetList.Count; i++)
        {
            // distanceSquared: 현재 유닛과 unit 간의 거리의 제곱입니다. Vector2를 사용하여 계산하며, sqrMagnitude를 사용하여 제곱 값을 얻습니다. 거리 제곱을 사용하는 이유는 루트 계산을 생략하여 성능을 최적화하기 위함입니다.
            float distanceSquared = ((Vector2)targetList[i].transform.localPosition - (Vector2)unit.transform.localPosition).sqrMagnitude;

            // unit.findRange * unit.findRange: 적을 찾는 범위의 제곱입니다. 거리를 비교할 때 제곱 값을 사용하여 루트 계산을 피합니다.
            if(distanceSquared <= unit.findRange * unit.findRange)
            {
                if(targetList[i].gameObject.activeInHierarchy)
                {
                    if(targetList[i]._unitState != Character.UnitState.death)
                    {
                        if(distanceSquared < closestDistance)
                        {
                            targetUnit = targetList[i];
                            closestDistance = distanceSquared;
                        }
                    }
                }
            }
        }

        return targetUnit;
    }

}
