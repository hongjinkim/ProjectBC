using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum HeroClass
{
    Knight,
    Archer,
    Priest,
    Wizard
}

public class Knight : Hero, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public HeroClass _heroClass;
    private LineRenderer lineRenderer;
    public bool isSelected = false;
    private List<Vector3> previewPath;
    private KnightSkills knightSkills;
    private void Awake() 
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    protected override void Start() 
    {
        base.Start();
        _heroClass = HeroClass.Knight;
        playerStat.CharacteristicType = CharacteristicType.MuscularStrength;
        knightSkills = new KnightSkills(playerStat);
        SetStat();
    }
    protected override void Update()
    {
        base.Update();
        knightSkills.CheckAndUseActiveSkill();
    }
    public override void SetStat()
    {
        base.SetStat(); // Hero의 SetStat 호출
        AdjustKnightStats(); // Knight 특유의 스탯 조정
        UpdateStatsFromPlayerStat(); // 최종 동기화
    }
    private void AdjustKnightStats()
    {
        playerStat.HP = info.hp;
        playerStat.AttackDamage = info.attackDamage;
        playerStat.Defense = info.defense;
        // Knight 특성에 맞는 추가 스탯 조정
        playerStat.Strength += 5; // 예: 기본 힘 증가
        playerStat.Defense += 3; // 예: 기본 방어력 증가
        UpdateStatsFromPlayerStat(); // 조정된 스탯 반영
    }
    public void UsePowerfulStrike()
    {
        knightSkills.UseActiveSkill();
    }
    public override void IncreaseCharacteristic(float amount)
    {
        IncreaseStrength(amount * 3);
    }
    protected override void OnAnimAttack()
    {
        animator.SetTrigger("Slash1H");
        IsAction = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSelected = true;
        lineRenderer.positionCount = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPosition.z = 0;
        //Vector3 nearestValidEndPosition = base.customTilemapManager.GetNearestValidPosition(endPosition);

        // if (customTilemapManager.IsValidMovePosition(nearestValidEndPosition))
        // {
        //     Vector3 start = customTilemapManager.GetNearestValidPosition(transform.position);
        //     previewPath = customTilemapManager.FindPath(start, nearestValidEndPosition);
        //     DrawPath(previewPath);
        // }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandleSelectionAndMovement();
        isSelected = false;

        lineRenderer.positionCount = 0;

        //autoMove = false;
    }

    private void DrawPath(List<Vector3> pathToDraw)
    {
        if (pathToDraw != null && pathToDraw.Count > 0)
        {
            lineRenderer.positionCount = pathToDraw.Count;
            lineRenderer.SetPositions(pathToDraw.ToArray());
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    public void HandleSelectionAndMovement()
    {
        Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPosition.z = 0;

        Move();
        // base.nearestValidPosition = customTilemapManager.GetNearestValidPosition(endPosition);

        // if (customTilemapManager.IsValidMovePosition(nearestValidPosition))
        // {
        //     base.SetNewPath(nearestValidPosition);
        // }
    }
    public void UseSkill()
    {
        if (playerStat.Energy >= 100)
        {
            playerStat.Energy = 0;
            // ��ų ��� ����...
        }
    }
}
