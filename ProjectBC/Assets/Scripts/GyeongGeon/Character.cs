using UnityEngine;

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

    public UnitState _unitState = UnitState.idle;

    public Character _target;

    public Vector2 _tempDistance;
    public Vector2 _dirVec;

    public float health;
    public float moveSpeed;
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;
    public float findRange;

    public float findTimer;
    public float attackTimer;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        CheckState();
    }

    void OnCollisionEnter2D(Collision2D collision) 
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

    public void Damageable()
    {

    }

    public bool Die()
    {
        return health == 0;
    }

    public bool Alive()
    {
        return health > 0; 
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

    void OnAttck()
    {
        // 애니메이션 삽입
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

}
