using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wizard : Hero, IDragHandler, IEndDragHandler, IBeginDragHandler
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
        _heroClass = HeroClass.Priest;
        info.characteristicType = CharacteristicType.Intellect;
        info.attackRange = 4;
    }
    public override void IncreaseCharacteristic(float amount)
    {
        //IncreaseIntelligence(amount * 2);
    }
    public void UseSkill()
    {
        if (info.energy >= 100)
        {
            info.energy = 0;
            // ��ų ��� ����...
        }
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
        // base.nearestValidPosition = customTilemapManager.GetNearestValidPosition(endPosition);

        // if (customTilemapManager.IsValidMovePosition(nearestValidPosition))
        // {
        //     base.SetNewPath(nearestValidPosition);
        // }
    }
}
