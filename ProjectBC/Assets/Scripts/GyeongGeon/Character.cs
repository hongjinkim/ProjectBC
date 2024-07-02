using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Character : MonoBehaviour, IBehavior
{
    public enum UnitState
    {
        idle,
        move,
        attack,
        skill,
        death
    }

    public enum AttackType
    {
        melee,
        Projectile
    }

    private IObjectPool<DamageText> DamageTextPool;

    public GameObject projectilePrefab;

    public GameObject canvas;
    public GameObject PrefabDmgTxt;
    float height = 1f;

    public Character _target;

    public Vector2 _tempDistance;
    public Vector2 _dirVec;
    public UnitState _unitState = UnitState.idle;
    public AttackType attackType;

    public float maxHealth;
    public float currentHealth;
    public float moveSpeed;
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;
    public float findRange;

    public float findTimer;
    public float attackTimer;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        CheckState();
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
        OnMove();
    }

    public void Damageable(Character target, float _damage)
    {
        target.currentHealth -= _damage;
        InstantiateDmgTxtObj(_damage);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        switch (gameObject.tag)
        {
            case "Hero":
                GameManager.Instance.dungeonManager._heroUnitList.Remove(this);
                break;
            case "Enemy":
                GameManager.Instance.dungeonManager._enemyUnitList.Remove(this);
                break;
        }
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
                //애니메이션 삽입
                break;

            case UnitState.move:
                //애니메이션 삽입
                break;

            case UnitState.attack:
                //애니메이션 삽입
                break;

            case UnitState.skill:
                //애니메이션 삽입
                break;

            case UnitState.death:
                //애니메이션 삽입
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
        if(!CheckTarget()) return;
        CheckDistance();
        
        _dirVec = _tempDistance.normalized;

        SetDirection();

        transform.position += (Vector3)_dirVec * moveSpeed * Time.deltaTime;
    }

    void SetDirection()
    {
        if(_dirVec.x >= 0)
        {
            // 애니메이션.transform.localScale = new Vector3(-1,1,1)
        }
        else
        {
            // 애니메이션.transform.localScale = new Vector3(1,1,1)
        }
    }

    bool CheckDistance()
    {
        _tempDistance = (Vector2)(_target.transform.localPosition - transform.position);
        float distanceSquared = _tempDistance.sqrMagnitude;

        if(distanceSquared <= attackRange * attackRange)
        {
            SetState(UnitState.attack);
            return true;
        }
        else
        {
            if(!CheckTarget()) SetState(UnitState.idle);
            else SetState(UnitState.move);

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
        _dirVec = (Vector2)(_target.transform.localPosition - transform.position).normalized;
        //_dirVec = _tempDistance = (Vector2)(_target.transform.localPosition - transform.position).normalized;
        SetDirection();
        // 애니메이션 삽입
        SetAttack();
    }

    void SetAttack()
    {
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

        Damageable(_target, attackDamage);
    }

    void InstantiateDmgTxtObj(float damage)
    {
        GameObject DamageTxtObj = Instantiate(PrefabDmgTxt, canvas.transform);
        DamageTxtObj.GetComponent<TextMeshProUGUI>().text = damage.ToString();

        Vector3 damagetxtPos = Camera.main.WorldToScreenPoint(new Vector3(_target.transform.position.x, _target.transform.position.y + height, 0));
        DamageTxtObj.GetComponent<RectTransform>().position = damagetxtPos;
    }

}
