using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public float _findTimer;

    public List<Transform> _unitPool = new List<Transform>();
    public List<Character> _heroUnitList = new List<Character>();
    public List<Character> _enemyUnitList = new List<Character>();

    private void Awake() 
    {
        GameManager.Instance.dungeonManager = this;    
    }

    void Start()
    {
        SetUnitList();
    }

    void Update()
    {
        
    }

    void SetUnitList()
    {
        _heroUnitList.Clear();
        _enemyUnitList.Clear();

        for(var i = 0; i < _unitPool.Count; i++)
        {
            for(var j = 0; j < _unitPool[i].childCount; j++)
            {
                switch(i)
                {
                    case 0:
                        _heroUnitList.Add(_unitPool[i].GetChild(j).GetComponent<Knight>());
                        _unitPool[i].GetChild(j).gameObject.tag = "Hero";
                        break;

                    case 1:
                        _enemyUnitList.Add(_unitPool[i].GetChild(j).GetComponent<Dragon>());
                        _unitPool[i].GetChild(j).gameObject.tag = "Enemy";
                        break;
                }
            }
        }
    }

    public Character GetTarget(Character _unit)
    {
        Character targetUnit = null;

        List<Character> targetList = new List<Character>();

        switch (_unit.tag)
        {
            case "Hero":
                targetList = _enemyUnitList;
                break;

            case "Enemy":
                targetList = _heroUnitList;
                break;
        }
        
        // closestDistance: 가장 가까운 유닛과의 거리를 저장하는 변수입니다. 초기값을 매우 큰 값(float.MaxValue)으로 설정하여 첫 번째 비교에서 무조건 갱신되도록 합니다.
        float closestDistance = float.MaxValue;

        for (var i = 0; i < targetList.Count; i++)
        {
            // distanceSquared: 현재 유닛과 _unit 간의 거리의 제곱입니다. Vector2를 사용하여 계산하며, sqrMagnitude를 사용하여 제곱 값을 얻습니다. 거리 제곱을 사용하는 이유는 루트 계산을 생략하여 성능을 최적화하기 위함입니다.
            float distanceSquared = ((Vector2)targetList[i].transform.localPosition - (Vector2)_unit.transform.localPosition).sqrMagnitude;

            // _unit.findRange * _unit.findRange: 적을 찾는 범위의 제곱입니다. 거리를 비교할 때 제곱 값을 사용하여 루트 계산을 피합니다.
            if(distanceSquared <= _unit.findRange * _unit.findRange)
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
