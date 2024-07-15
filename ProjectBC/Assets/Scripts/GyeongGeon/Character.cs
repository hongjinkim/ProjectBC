using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Character : MonoBehaviour, IBehavior
{
    public enum UnitState
    {
        idle = 0,
        move = 2,
        attack,
        skill,
        death = 6
    }

    public enum AttackType
    {
        melee,
        Projectile
    }

    public enum CharacterDirection
    {
        down,
        up,
        left,
        Right
    }
    protected PlayerStat playerStat;
    private List<ISkill> skillList;
    private IObjectPool<DamageText> DamageTextPool;
    public List<GameObject> Parts;
    public List<GameObject> Shadows;

    public Animator animator;

    public GameObject projectilePrefab;

    public GameObject canvas;
    public GameObject PrefabDmgTxt;
    float height = 1f;

    public Character _target;

    public Vector2 _tempDistance;
    public Vector2 _dirVec;
    public UnitState _unitState = UnitState.idle;
    public AttackType attackType;
    public CharacterDirection characterDirection;

    public float maxHealth;
    public float currentHealth;
    public float moveSpeed;
    public float attackDamage;
    public float attackSpeed;
    public int attackRange;
    public float findRange;

    public float findTimer;
    public float attackTimer;

    [Header("DieEffect")]
    public GameObject fadeObject;
    public SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    public float fadeDuration = 1.0f;

    [Header("TileMap")]
    public CustomTilemapManagerGG customTilemapManager;
    public float updatePathInterval = 1f;
    private int currentPathIndex;
    private float lastPathUpdateTime = 0f;

    private Vector3 targetPosition;
    private Vector3 currentPosition;
    protected Vector3 nearestValidPosition;
    private List<Vector3> path;

    private const float PositionTolerance = 0.15f;
    private const float PositionToleranceSquared = PositionTolerance * PositionTolerance;

    protected bool isCustomTilemapManagerInitialized = false;

    public Sprite Icon { get; protected set; }
    public float currentExp;
    public float needexp;
    public int Level { get; protected set; }
    public List<Trait> Traits { get; protected set; } = new List<Trait>();
    public TraitType SelectedTraitType { get; protected set; }
    public PlayerSkill ActiveSkill { get; protected set; }

    
    protected HeroSkills skills;

    // 기존 Character 메서드와 Player에서 가져온 메서드 통합
    protected virtual void InitializeSkillBook() { }
    public virtual void IncreaseCharacteristic(float amount) { }

    protected virtual void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        if (playerStat == null)
        {
            playerStat = gameObject.AddComponent<PlayerStat>();
        }
        customTilemapManager = new CustomTilemapManagerGG(TilemapManagerGG.Instance, this);
        //customTilemapManager.allCharacters.Add(this);
        transform.position = customTilemapManager.GetNearestValidPosition(transform.position);
        StartCoroutine(AutoMoveCoroutine());

        InitializePlayerStat();
        StartCoroutine(InitializeCustomTilemapManagerCoroutine());

        currentHealth = maxHealth;
    }
    protected virtual void InitializePlayerStat()
    {
        playerStat = GetComponent<PlayerStat>();
        if (playerStat == null)
        {
            Debug.Log("PlayerStat component not found. Adding a new one.");
            playerStat = gameObject.AddComponent<PlayerStat>();
        }
        Debug.Log($"PlayerStat initialized: {(playerStat != null ? "Success" : "Failed")}");
    }
    private IEnumerator AIMovementCoroutine()
    {
        Debug.Log($"Starting AI Movement for {gameObject.name}");
        while (true)
        {
            if (customTilemapManager != null && isCustomTilemapManagerInitialized)
            {
                UpdatePath();
                Move();
            }
            else
            {
                Debug.LogWarning($"CustomTilemapManager not ready for {gameObject.name}");
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator InitializeCustomTilemapManagerCoroutine()
    {
        while (TilemapManagerGG.Instance == null)
        {
            Debug.Log("Waiting for TilemapManagerGG.Instance to be initialized...");
            yield return new WaitForSeconds(0.1f);
        }

        try
        {
            customTilemapManager = new CustomTilemapManagerGG(TilemapManagerGG.Instance, this);
            if (customTilemapManager == null)
            {
                Debug.LogError("CustomTilemapManagerGG is null after creation");
                StartCoroutine(AIMovementCoroutine());
            }
            else
            {
                isCustomTilemapManagerInitialized = true;
                Debug.Log("CustomTilemapManagerGG created successfully");
                StartCoroutine(AIMovementCoroutine());
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error creating CustomTilemapManagerGG: {e.Message}\nStack Trace: {e.StackTrace}");
        }
    }

    protected void Update()
    {
        CheckState();

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        string targetTag = "";

        switch (gameObject.tag)
        {
            case "Hero": 
                targetTag = "Enemy";
                break;

            case "Enemy": 
                targetTag = "Hero";
                break;
        }

        if(collision.gameObject.CompareTag(targetTag))
        {
            
        }
        else if(collision.gameObject.CompareTag(gameObject.tag))
        {
            SetState(UnitState.idle);
        }
    }

    public void Move()
    {
        if (this.gameObject.activeInHierarchy)
        {
            OnMove();
        }
    }

    public void TakeDamage(Character target, float _damage)
    {
        if (target != null)
        {
            target.currentHealth -= _damage;
            InstantiateDmgTxtObj(_damage);
        }
    }

    public void Die()
    {
        // switch (gameObject.tag)
        // {
        //     case "Hero":
        //         GameManager.Instance.dungeonManager._heroUnitList.Remove(this);
        //         break;
        //     case "Enemy":
        //         GameManager.Instance.dungeonManager._enemyUnitList.Remove(this);
        //         break;
        // }

        SetState(UnitState.death);
        //gameObject.SetActive(false);
        InitFadeEffect();
        StartCoroutine(FadeOut());
    }

    public bool Alive()
    {
        return currentHealth > 0; 
    }

    public void Attack()
    {
        OnAttck();
    }

    void CheckState()
    {
        switch(_unitState)
        {
            case UnitState.idle:
                FindTarget();
                break;

            case UnitState.move:
                FindTarget();
                Move();
                break;

            case UnitState.attack:
                CheckAttack();
                break;

            case UnitState.skill:
                break;

            case UnitState.death:
                break;
        }
    }

    void SetState(UnitState state)
    {
        _unitState = state;

        switch (_unitState)
        {
            case UnitState.idle:
                SetAnimatorState(_unitState);
                break;

            case UnitState.move:
                SetAnimatorState(_unitState);
                break;

            case UnitState.attack:
                SetAnimatorState(UnitState.idle);
                break;

            case UnitState.skill:
                break;

            case UnitState.death:
                SetAnimatorState(_unitState);
                break;
        }

    }

    void FindTarget()
    {
        // 적을 찾을때 연산이 많아져서 느려질 수 있는 문제를 방지한다.
        findTimer += Time.deltaTime;

        if(findTimer > GameManager.Instance.dungeonManager._findTimer)
        {
            _target = GameManager.Instance.dungeonManager.GetTarget(this);

            if(_target != null) SetState(UnitState.move);
            else SetState(UnitState.idle);

            findTimer = 0;
        }
    }

    bool CheckTarget()
    {
        //if (_target == null) return false;

        bool value = true;

        if(_target == null) value = false;
        if(_target._unitState == UnitState.death) value = false;
        if(!_target.gameObject.activeInHierarchy) value = false;

        if(!value)
        {
            SetState(UnitState.idle);
        }

        return value;
    }

    void OnMove()
    {
        //if(!CheckTarget()) return;
        if(CheckDistance()) return;
        
        _dirVec = (Vector2)(_target.transform.position - transform.position).normalized;

        SetDirection();

        //transform.position += (Vector3)_dirVec * moveSpeed * Time.deltaTime;
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        MoveAlongPath();

        if (path == null || path.Count == 0)
        {
            SnapToNearestTileCenter();
        }
    }

    void SetDirection()
    {
        if (Mathf.Abs(_dirVec.x) > Mathf.Abs(_dirVec.y))
        {
            // 좌우 방향
            if (_dirVec.x >= 0)
            {
                // 오른쪽을 향하도록 설정
                characterDirection = CharacterDirection.Right;
            }
            else
            {
                // 왼쪽을 향하도록 설정
                characterDirection = CharacterDirection.left;
            }
        }
        else
        {
            // 상하 방향
            if (_dirVec.y >= 0)
            {
                // 위를 향하도록 설정
                characterDirection = CharacterDirection.up;
            }
            else
            {
                // 아래를 향하도록 설정
                characterDirection = CharacterDirection.down;
            }
        }

        for (var i = 0; i < Parts.Count; i++)
        {
            Parts[i].SetActive(i == (int)characterDirection);
            Shadows[i].SetActive(i == (int)characterDirection);
        }
    }

    bool CheckDistance()
    {
        if (_target == null)
        {
            SetState(UnitState.idle);
            return false;
        }

        Vector3 targetPosition = _target.transform.position;
        Vector3 currentPosition = transform.position;

        if(IsWithinAttackRange(currentPosition, targetPosition, attackRange))
        {
            
            SetState(UnitState.attack);

            return true;
        }
        else
        {
            if (!CheckTarget())
            {
                SetState(UnitState.idle);
            }
            else
            {
                SetState(UnitState.move);
            }

            return false;
        }
    }

    void CheckAttack()
    {
        if(!CheckTarget()) return;
        if(!CheckDistance()) return;

        attackTimer += Time.deltaTime;

        if(attackTimer > attackSpeed)
        {
            Attack();
            attackTimer = 0;
        }
    }

    void OnAttck()
    {
        if (_target == null) return;

        _dirVec = (Vector2)(_target.transform.position - transform.position).normalized;
        SetDirection();

        // 애니메이션 삽입
        OnAnimAttack();
        SetAttack();
    }

    void SetAttack()
    {
        if (_target == null) return;

        if (AttackType.Projectile.Equals(attackType))
        {
            if (projectilePrefab != null && _target != null)
            {
                GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Projectile projectile = projectileInstance.GetComponent<Projectile>();

                if (projectile != null)
                {
                    projectile.InitProjectileRotation(_target.transform.position);
                }
            }
        }

        TakeDamage(_target, attackDamage);
    }

    void InstantiateDmgTxtObj(float damage)
    {
        //if (_target == null) return;

        GameObject DamageTxtObj = Instantiate(PrefabDmgTxt, GameManager.Instance.dungeonManager.canvas.transform);
        DamageTxtObj.GetComponent<TextMeshProUGUI>().text = damage.ToString();

        Vector3 damagetxtPos = Camera.main.WorldToScreenPoint(new Vector3(_target.transform.position.x, _target.transform.position.y + height, 0));
        DamageTxtObj.GetComponent<RectTransform>().position = damagetxtPos;
    }

    public bool IsAction
    {
        get => animator.GetBool("Action");
        set => animator.SetBool("Action", value);
    }

    public void SetAnimatorState(UnitState state)
	{
		animator.SetInteger("State", (int) state);

        if (_target == null)
        {
           IsAction = false;    
        }
	}

    public void Slash1H()
	{
		animator.SetTrigger("Slash1H");
        IsAction = true;
    }

    public void ShotBow()
	{
		animator.SetTrigger("ShotBow");
        IsAction = true;
	}

    protected virtual void OnAnimAttack(){Debug.Log("공격애니메이션발동");}

    private void InitFadeEffect()
    {
        spriteRenderers = fadeObject.GetComponentsInChildren<SpriteRenderer>();
        originalColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }
    }

    // void ManageSkill()
    // {


    //     for (int i = 0; i < length; i++)
    //     {
            
    //     }
    // }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime / 0.5f;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                Color newColor = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, alpha);
                spriteRenderers[i].color = newColor;
            }

            yield return null;
        }

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0.0f);
        }

        gameObject.SetActive(false);
        Destroy(customTilemapManager);
        GameManager.Instance.dungeonManager._allCharacterList.Remove(this);

    }



    IEnumerator AutoMoveCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(updatePathInterval);

        while (true)
        {
            yield return wait;

            UpdatePath();
            
        }
    }

    void UpdatePath()
    {
        GameObject closestObject = _target.gameObject;

        if (closestObject != null)
        {
            targetPosition = closestObject.transform.position;
            currentPosition = customTilemapManager.GetNearestValidPosition(transform.position);

            if ((currentPosition - targetPosition).sqrMagnitude > PositionToleranceSquared)
            {
                List<Vector3> surroundingPositions = GetSurroundingPositions(targetPosition);

                Vector3? bestPosition = FindBestPosition(surroundingPositions, currentPosition);

                if (bestPosition.HasValue)
                {
                    SetNewPath(bestPosition.Value);
                }
                else
                {
                    path = null;
                }
            }
            else
            {
                transform.position = currentPosition;
                path = null;
            }
        }
        
        lastPathUpdateTime = Time.time;
    }

    private List<Vector3> GetSurroundingPositions(Vector3 targetPosition)
    {
        List<Vector3> surroundingPositions = new List<Vector3>();

        for (int x = -attackRange; x <= 1; x++)
        {
            for (int y = -attackRange; y <= attackRange; y++)
            {
                if (x == 0 && y == 0) continue;

                surroundingPositions.Add(new Vector3(targetPosition.x + x, targetPosition.y + y, targetPosition.z));
            }
        }

        return surroundingPositions;
    }

    private Vector3? FindBestPosition(List<Vector3> positions, Vector3 currentPosition)
    {
        Vector3? bestPosition = null;
        float minSqrDistance = float.MaxValue;

        for (int i = 0; i < positions.Count; i++)
        {
            if (customTilemapManager.IsValidMovePosition(positions[i]))
            {
                float sqrDistance = (currentPosition - positions[i]).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    bestPosition = positions[i];
                }
            }
        }

        return bestPosition;
    }

    // private Vector3? FindBestPosition(List<Vector3> positions, Vector3 currentPosition)
    // {
    //     Vector3? bestPosition = null;
    //     float minSqrDistance = float.MaxValue;

    //     for (int i = 0; i < positions.Count; i++)
    //     {
    //         bool isValid = customTilemapManager.IsValidMovePosition(positions[i]);
    //         Debug.Log($"Position {positions[i]} is valid: {isValid}");

    //         if (isValid)
    //         {
    //             float sqrDistance = (currentPosition - positions[i]).sqrMagnitude;
    //             Debug.Log($"Distance to {positions[i]}: {sqrDistance}");

    //             if (sqrDistance < minSqrDistance)
    //             {
    //                 minSqrDistance = sqrDistance;
    //                 bestPosition = positions[i];
    //                 Debug.Log($"New best position: {bestPosition}");
    //             }
    //         }
    //     }

    //     Debug.Log($"Final best position: {bestPosition}");
    //     return bestPosition;
    // }

    protected void SetNewPath(Vector3 target)
    {
        Vector3 start = customTilemapManager.GetNearestValidPosition(transform.position);
        path = customTilemapManager.FindPath(start, target);
        currentPathIndex = 0;

        if (path != null && path.Count > 0)
        {
            TilemapManagerGG.Instance.SetDebugPath(path);
        }
    }

    private void MoveAlongPath()
    {
        if (path != null && currentPathIndex < path.Count)
        {
            Vector3 targetPosition = path[currentPathIndex];
            //targetPosition.z = 0; // 추가: z 값을 0으로 설정

            if (!customTilemapManager.IsValidMovePosition(targetPosition))
            {
                UpdatePath();
                return;
            }

            if ((transform.position - targetPosition).sqrMagnitude > PositionToleranceSquared)
            {
                Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                //newPosition.z = 0; // 추가: z 값을 0으로 설정
                transform.position = newPosition;
            }
            else
            {
                //transform.position = targetPosition;
                //transform.position = new Vector3(targetPosition.x, targetPosition.y, 0); // 추가: z 값을 0으로 설정
                currentPathIndex++;
            }

            if (currentPathIndex >= path.Count)
            {
                path = null;

                SnapToNearestTileCenter();

                StartCoroutine(WaitAndFindNewPath());
            }
        }
    }

    private void SnapToNearestTileCenter()
    {
        nearestValidPosition = customTilemapManager.GetNearestValidPosition(transform.position);
        nearestValidPosition.z = 0; // 추가: z 값을 0으로 설정

        if ((transform.position - nearestValidPosition).sqrMagnitude < PositionToleranceSquared)
        {
            transform.position = nearestValidPosition;
        }
    }

    IEnumerator WaitAndFindNewPath()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject closestObject = _target.gameObject;

        if (closestObject != null)
        {
            Vector3 targetPosition = closestObject.transform.position;
            SetNewPath(targetPosition);
        }
        
    }

    // 거리 체크 메서드
    public bool IsWithinAttackRange(Vector3 currentPosition, Vector3 targetPosition, int attackRange)
    {
        // 모든 범위 내의 좌표를 가져옴
        List<Vector3> attackRangePositions = GetAttackableRangePositions(currentPosition, attackRange);

        // targetPosition이 attackRangePositions에 포함되는지 확인
        foreach (var position in attackRangePositions)
        {
            if (position.x == targetPosition.x && position.y == targetPosition.y)
            {
                return true;
            }
        }

        return false;
    }

    private List<Vector3> GetAttackableRangePositions(Vector3 position, int range)
    {
        List<Vector3> attackableRangePositions = new List<Vector3>();

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                // 대각선 포함 모든 타일 좌표 추가
                Vector3 pos = new Vector3(position.x + x, position.y + y, 0);
                attackableRangePositions.Add(pos);
            }
        }

        return attackableRangePositions;
    }
    protected void IncreaseStrength(float amount)
    {
        playerStat.Strength += (int)amount;
        playerStat.HealthRegen += (int)(0.1f * amount);
        playerStat.HP += (int)(1f * amount);

        if (playerStat.CharacteristicType == CharacteristicType.MuscularStrength)
        {
            playerStat.AttackDamage += (int)(0.7f * amount);
        }
    }

    protected void IncreaseAgility(float amount)
    {
        playerStat.Agility += (int)amount;
        playerStat.AttackSpeed += (int)(0.1f * amount);
        playerStat.Defense += (int)(0.1f * amount);

        if (playerStat.CharacteristicType == CharacteristicType.Agility)
        {
            playerStat.AttackDamage += (int)(0.9f * amount);
        }
    }

    protected void IncreaseIntelligence(float amount)
    {
        playerStat.Intelligence += (int)amount;
        playerStat.EnergyRegen += (int)(0.1f * amount);
        playerStat.MagicResistance += (int)(0.1f * amount);

        if (playerStat.CharacteristicType == CharacteristicType.Intellect)
        {
            playerStat.AttackDamage += (int)(0.9f * amount);
        }
    }

    protected void IncreaseStamina(float amount)
    {
        playerStat.Stamina += (int)amount;
        playerStat.HP += (int)(10f * amount);
    }

    //public abstract void IncreaseCharacteristic(float amount);

}
