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

public class Knight : Character, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public HeroClass _heroClass;
    private LineRenderer lineRenderer;
    public bool isSelected = false;
    private List<Vector3> previewPath;

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
        Vector3 nearestValidEndPosition = base.customTilemapManager.GetNearestValidPosition(endPosition);

        if (customTilemapManager.IsValidMovePosition(nearestValidEndPosition))
        {
            Vector3 start = customTilemapManager.GetNearestValidPosition(transform.position);
            previewPath = customTilemapManager.FindPath(start, nearestValidEndPosition);
            DrawPath(previewPath);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandleSelectionAndMovement();
        isSelected = false;

        lineRenderer.positionCount = 0;
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
        base.nearestValidPosition = customTilemapManager.GetNearestValidPosition(endPosition);

        if (customTilemapManager.IsValidMovePosition(nearestValidPosition))
        {
            base.SetNewPath(nearestValidPosition);
        }
    }
    public void UseSkill()
    {
        if (playerStat.Energy >= 100)
        {
            playerStat.Energy = 0;
            // 스킬 사용 로직...
        }
    }
}
