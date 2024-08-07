using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

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
    public ShieldBash shieldBash;
    public HeavenlyBlessing heavenlyBlessing;
    public Impregnable impregnable;
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
        info.characteristicType = CharacteristicType.MuscularStrength;
        info.attackRange = 1; // 근접 공격 범위
        shieldBash = new ShieldBash();
        heavenlyBlessing = new HeavenlyBlessing();
        impregnable = new Impregnable();
        info.skills.Add(shieldBash);
        info.skills.Add(heavenlyBlessing);
        info.skills.Add(impregnable);
        info.activeSkill = shieldBash;
        ApplyPassiveSkills();
    }
    protected override void Update()
    {
        base.Update();
        UpdateDamageReduction();
        CheckAndUseSkill();
    }
    private void ApplyPassiveSkills()
    {
        heavenlyBlessing.ApplyEffect(this);
        UpdateDamageReduction();
    }
    protected override void UseSkill()
    {
        Debug.Log($"Knight UseSkill method called. Current Energy: {Energy}");

        if (Energy >= 100 && info.activeSkill != null)
        {
            Debug.Log($"Knight energy is full, using {info.activeSkill.Name}");
            info.activeSkill.UseSkill(this);
            Energy = 0;  // 스킬 사용 후 에너지 리셋
        }
    }
    private void UpdateDamageReduction()
    {
        int baseReduction = info.damageReduction;
        int bonusReduction = impregnable.GetDamageReductionBonus(this);
        info.damageReduction = baseReduction + bonusReduction;
    }


    public override void IncreaseCharacteristic(float amount)
    {
        //IncreaseStrength(amount * 3);
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
   
}
