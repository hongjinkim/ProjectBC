using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [Header("BasicInformation")]
    public TilemapManagerGG tilemapManager;
    public string _themeCode;
    public string _themeName;
    public string _stageCode;
    public float _findTimer;
    public int _enemyQuantity;
    private int _randomEnemyIndex;
    private int _randomTileIndex;

    [Header("ActiveList")]
    public List<Character> _activeEnemyList = new List<Character>();
    public List<Character> _allCharacterList = new List<Character>();

    [Header("UI_StageSlot")]
    public string _stageName;
    public Sprite _stageImage;
    public int _navigationProgress;

    [Header("UI_StageInformation")]
    public List<Character> _enemyPool = new List<Character>();
    public List<Item> _ItemList = new List<Item>();
    public List<Character> _activeHeroList = new List<Character>();

    void Start()
    {   
        tilemapManager.CalculateTileCenters();
        // 테스트용
        SetHeroList();
        SetEnemyList();
        DungeonInit();
    }

    // 테스트용
    public List<Character> _heroPool = new List<Character>();
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

    void Update()
    {
        CheckAllEnemiesDead();
    }

    public void DungeonInit()
    {
        foreach (var character in _allCharacterList)
        {
            character.dungeon = this;
            character.tilemapManager = tilemapManager;
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
            Character enemy = Instantiate(_enemyPool[_randomEnemyIndex]);

            _activeEnemyList.Add(enemy);
            _allCharacterList.Add(enemy);

            _activeEnemyList[i].gameObject.tag = "Enemy";

            _randomTileIndex = Random.Range(0, tilemapManager.tileCenters.Count);
            enemy.transform.position = tilemapManager.tileCenters[_randomTileIndex];

            _activeEnemyList[i].gameObject.SetActive(true);
        }
    }

}
