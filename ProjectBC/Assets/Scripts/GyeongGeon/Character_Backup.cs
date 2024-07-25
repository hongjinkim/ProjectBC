// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.Pool;

// public abstract class Character_Backup : MonoBehaviour, IBehavior
// {
//     public enum UnitState
//     {
//         idle = 0,
//         move = 2,
//         attack,
//         skill,
//         death = 6
//     }

//     public enum AttackType
//     {
//         melee,
//         Projectile
//     }

//     public enum CharacterDirection
//     {
//         down,
//         up,
//         left,
//         Right
//     }
//     private List<ISkill> skillList;
//     private IObjectPool<DamageText> DamageTextPool;
//     public List<GameObject> Parts;
//     public List<GameObject> Shadows;

//     public Animator animator;

//     public GameObject projectilePrefab;

//     public GameObject canvas;
//     public GameObject PrefabDmgTxt;
//     float height = 1f;

//     public Character _target;

//     public Vector2 _tempDistance;
//     public Vector2 _dirVec;
//     public UnitState _unitState = UnitState.idle;
//     public AttackType attackType;
//     public CharacterDirection characterDirection;

//     public float maxHealth;
//     public float currentHealth;
//     public float moveSpeed;
//     public float attackDamage;
//     public float attackSpeed;
//     public float attackRange;
//     public float findRange;

//     public float findTimer;
//     public float attackTimer;

//     [Header("DieEffect")]
//     public GameObject fadeObject;
//     public SpriteRenderer[] spriteRenderers;
//     private Color[] originalColors;
//     public float fadeDuration = 1.0f;


//     protected virtual void Start()
//     {
//         currentHealth = maxHealth;
//     }

//     protected void Update()
//     {
//         CheckState();

//         if(currentHealth <= 0)
//         {
//             Die();
//         }
//     }

//     void OnTriggerEnter2D(Collider2D collision) 
//     {
//         string targetTag = "";

//         switch (gameObject.tag)
//         {
//             case "Hero": 
//                 targetTag = "Enemy";
//                 break;

//             case "Enemy": 
//                 targetTag = "Hero";
//                 break;
//         }

//         if(collision.gameObject.CompareTag(targetTag))
//         {
            
//         }
//         else if(collision.gameObject.CompareTag(gameObject.tag))
//         {
//             SetState(UnitState.idle);
//         }
//     }

//     public void Move()
//     {
//         OnMove();
//     }

//     public void TakeDamage(Character target, float _damage)
//     {
//         if (target != null)
//         {
//             target.currentHealth -= _damage;
//             InstantiateDmgTxtObj(_damage);
//         }
//     }

//     public void Die()
//     {
//         // switch (gameObject.tag)
//         // {
//         //     case "Hero":
//         //         GameManager.Instance.dungeonManager._heroUnitList.Remove(this);
//         //         break;
//         //     case "Enemy":
//         //         GameManager.Instance.dungeonManager._enemyUnitList.Remove(this);
//         //         break;
//         // }

//         SetState(UnitState.death);
//         //gameObject.SetActive(false);
//         InitFadeEffect();
//         StartCoroutine(FadeOut());
//     }

//     public bool Alive()
//     {
//         return currentHealth > 0; 
//     }

//     public void Attack()
//     {
//         OnAttck();
//     }

//     void CheckState()
//     {
//         switch(_unitState)
//         {
//             case UnitState.idle:
//                 FindTarget();
//                 break;

//             case UnitState.move:
//                 FindTarget();
//                 Move();
//                 break;

//             case UnitState.attack:
//                 CheckAttack();
//                 break;

//             case UnitState.skill:
//                 break;

//             case UnitState.death:
//                 break;
//         }
//     }

//     void SetState(UnitState state)
//     {
//         _unitState = state;

//         switch (_unitState)
//         {
//             case UnitState.idle:
//                 SetAnimatorState(_unitState);
//                 break;

//             case UnitState.move:
//                 SetAnimatorState(_unitState);
//                 break;

//             case UnitState.attack:
//                 SetAnimatorState(UnitState.idle);
//                 break;

//             case UnitState.skill:
//                 break;

//             case UnitState.death:
//                 SetAnimatorState(_unitState);
//                 break;
//         }

//     }

//     void FindTarget()
//     {
//         // 적을 찾을때 연산이 많아져서 느려질 수 있는 문제를 방지한다.
//         findTimer += Time.deltaTime;

//         if(findTimer > GameManager.Instance.dungeonManager._findTimer)
//         {
//             _target = GameManager.Instance.dungeonManager.GetTarget(this);

//             if(_target != null) SetState(UnitState.move);
//             else SetState(UnitState.idle);

//             findTimer = 0;
//         }
//     }

//     bool CheckTarget()
//     {
//         //if (_target == null) return false;

//         bool value = true;

//         if(_target == null) value = false;
//         if(_target._unitState == UnitState.death) value = false;
//         if(!_target.gameObject.activeInHierarchy) value = false;

//         if(!value)
//         {
//             SetState(UnitState.idle);
//         }

//         return value;
//     }

//     void OnMove()
//     {
//         //if(!CheckTarget()) return;
//         CheckDistance();
        
//         _dirVec = _tempDistance.normalized;

//         SetDirection();

//         transform.position += (Vector3)_dirVec * moveSpeed * Time.deltaTime;
//     }

//     void SetDirection()
//     {
//         if (Mathf.Abs(_dirVec.x) > Mathf.Abs(_dirVec.y))
//         {
//             // 좌우 방향
//             if (_dirVec.x >= 0)
//             {
//                 // 오른쪽을 향하도록 설정
//                 characterDirection = CharacterDirection.Right;
//             }
//             else
//             {
//                 // 왼쪽을 향하도록 설정
//                 characterDirection = CharacterDirection.left;
//             }
//         }
//         else
//         {
//             // 상하 방향
//             if (_dirVec.y >= 0)
//             {
//                 // 위를 향하도록 설정
//                 characterDirection = CharacterDirection.up;
//             }
//             else
//             {
//                 // 아래를 향하도록 설정
//                 characterDirection = CharacterDirection.down;
//             }
//         }

//         for (var i = 0; i < Parts.Count; i++)
//         {
//             Parts[i].SetActive(i == (int)characterDirection);
//             Shadows[i].SetActive(i == (int)characterDirection);
//         }
//     }

//     bool CheckDistance()
//     {
//         if (_target == null)
//         {
//             SetState(UnitState.idle);
//             return false;
//         }

//         _tempDistance = (Vector2)(_target.transform.localPosition - transform.position);
//         float distanceSquared = _tempDistance.sqrMagnitude;

//         if(distanceSquared <= attackRange * attackRange)
//         {
//             SetState(UnitState.attack);
//             return true;
//         }
//         else
//         {
//             if(!CheckTarget()) SetState(UnitState.idle);
//             else SetState(UnitState.move);

//             return false;
//         }
//     }

//     void CheckAttack()
//     {
//         if(!CheckTarget()) return;
//         if(!CheckDistance()) return;

//         attackTimer += Time.deltaTime;

//         if(attackTimer > attackSpeed)
//         {
//             Attack();
//             attackTimer = 0;
//         }
//     }

//     void OnAttck()
//     {
//         if (_target == null) return;

//         _dirVec = (Vector2)(_target.transform.localPosition - transform.position).normalized;
//         SetDirection();

//         // 애니메이션 삽입
//         OnAnimAttack();
//         SetAttack();
//     }

//     void SetAttack()
//     {
//         if (_target == null) return;

//         if (AttackType.Projectile.Equals(attackType))
//         {
//             if (projectilePrefab != null && _target != null)
//             {
//                 GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
//                 Projectile projectile = projectileInstance.GetComponent<Projectile>();

//                 if (projectile != null)
//                 {
//                     projectile.InitProjectileRotation(_target.transform.position);
//                 }
//             }
//         }

//         TakeDamage(_target, attackDamage);
//     }

//     void InstantiateDmgTxtObj(float damage)
//     {
//         //if (_target == null) return;

//         GameObject DamageTxtObj = Instantiate(PrefabDmgTxt, GameManager.Instance.dungeonManager.canvas.transform);
//         DamageTxtObj.GetComponent<TextMeshProUGUI>().text = damage.ToString();

//         Vector3 damagetxtPos = Camera.main.WorldToScreenPoint(new Vector3(_target.transform.position.x, _target.transform.position.y + height, 0));
//         DamageTxtObj.GetComponent<RectTransform>().position = damagetxtPos;
//     }

//     public bool IsAction
//     {
//         get => animator.GetBool("Action");
//         set => animator.SetBool("Action", value);
//     }

//     public void SetAnimatorState(UnitState state)
// 	{
// 		animator.SetInteger("State", (int) state);

//         if (_target == null)
//         {
//            IsAction = false;    
//         }
// 	}

//     public void Slash1H()
// 	{
// 		animator.SetTrigger("Slash1H");
//         IsAction = true;
//     }

//     public void ShotBow()
// 	{
// 		animator.SetTrigger("ShotBow");
//         IsAction = true;
// 	}

//     protected virtual void OnAnimAttack(){Debug.Log("공격애니메이션발동");}

//     private void InitFadeEffect()
//     {
//         spriteRenderers = fadeObject.GetComponentsInChildren<SpriteRenderer>();
//         originalColors = new Color[spriteRenderers.Length];

//         for (int i = 0; i < spriteRenderers.Length; i++)
//         {
//             originalColors[i] = spriteRenderers[i].color;
//         }
//     }

//     // void ManageSkill()
//     // {


//     //     for (int i = 0; i < length; i++)
//     //     {
            
//     //     }
//     // }

//     private IEnumerator FadeOut()
//     {
//         float elapsedTime = 0f;

//         while (elapsedTime < fadeDuration)
//         {
//             elapsedTime += Time.deltaTime;
//             float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);

//             for (int i = 0; i < spriteRenderers.Length; i++)
//             {
//                 Color newColor = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, alpha);
//                 spriteRenderers[i].color = newColor;
//             }

//             yield return null; 
//         }

//         for (int i = 0; i < spriteRenderers.Length; i++)
//         {
//             spriteRenderers[i].color = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0.0f);
//         }

//         gameObject.SetActive(false);
//     }

// }
