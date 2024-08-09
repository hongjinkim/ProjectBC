using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hero : Character
{
    private const float MAX_ENERGY = 100f;


    private const float REGEN_INTERVAL = 1f;
    private const float REGEN_PERCENT = 0.05f;
    private bool isRegenerating = false;
    private bool isTraitSelectionPending = false;
    private int pendingTraitLevel = 0;
    protected virtual void OnEnable()
    {
        InstantFadeIn();
        if (isRegenerating)
        {
            CancelInvoke("RegenerateHealth");
            isRegenerating = false;
        }
    }


    protected virtual void OnDisable()
    {
        if (!isRegenerating)
        {
            isRegenerating = true;
            InvokeRepeating("RegenerateHealth", 0f, REGEN_INTERVAL);
        }

        if (_unitState == Character.UnitState.death)
        {
            Revive();
        }
    }


    protected override void Start()
    {
        base.Start();
        //SetStat();
        StartCoroutine(RegenerateEnergy());
        SetupHeroInfoEvents();

    }
    protected override void Update()
    {
        base.Update();
        CheckAndUseSkill();
    }
    private IEnumerator RegenerateEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (info != null)
            {
                info.energy = Mathf.Min(info.energy + info.energyRegen, MAX_ENERGY);
                
            }
        }
    }
    
    private void SetupHeroInfoEvents()
    {
        info.OnLevelUp += OnHeroLevelUp;
        info.OnTraitSelectionAvailable += OnTraitSelectionAvailable;
    }
    private void OnHeroLevelUp()
    {
        // 레벨업 시 수행할 작업...
        info.ApplyTraits(this);
    }
    private void OnTraitSelectionAvailable(int level)
    {
        isTraitSelectionPending = true;
        pendingTraitLevel = level;
        // UI를 통해 사용자에게 특성 선택을 요청
        RequestTraitSelectionFromUser();
    }
    private void RequestTraitSelectionFromUser()
    {
        // 이 메서드는 UI 시스템을 통해 사용자에게 특성 선택을 요청합니다.
        // 예를 들어, 특성 선택 팝업을 표시할 수 있습니다.
        // 실제 구현은 게임의 UI 시스템에 따라 달라집니다.
        
    }
    public void OnTraitSelected(bool isLeftTrait)
    {
        if (isTraitSelectionPending)
        {
            info.SelectTrait(pendingTraitLevel, isLeftTrait);
            info.ApplyTraits(this);
            isTraitSelectionPending = false;
            pendingTraitLevel = 0;
        }
    }
   
    protected virtual void ApplyPassiveSkill1() { }
    protected virtual void ApplyPassiveSkill2() { }
    protected virtual void ApplyPassiveSkill3() { }
   
    private void RegenerateHealth()
    {
        if (!gameObject.activeInHierarchy && currentHealth < maxHealth)
        {
            currentHealth = (int)Mathf.Min(currentHealth + (maxHealth * REGEN_PERCENT), maxHealth);
            if (currentHealth >= maxHealth)
            {
                CancelInvoke("RegenerateHealth");
                isRegenerating = false;
            }
        }
    }
}


//    protected void IncreaseIntelligence(float amount)
//    {
//        info.intelligence += (int)amount;
//        info.energyRegen += (int)(0.1f * amount);
//        info.magicResistance += (int)(0.1f * amount);

//        if (info.characteristicType == CharacteristicType.Intellect)
//        {
//            info.attackDamage += (int)(0.9f * amount);
//        }
//        SetStat();
//    }

//    protected void IncreaseStamina(float amount)
//    {
//        info.stamina += (int)amount;
//        info.hp += (int)(10f * amount);
//        SetStat();
//    }

//    private void RegenerateHealth()
//    {
//        if (!gameObject.activeInHierarchy && currentHealth < maxHealth)
//        {
//            currentHealth = Mathf.Min(currentHealth + (maxHealth * REGEN_PERCENT), maxHealth);
//            if (currentHealth >= maxHealth)
//            {
//                CancelInvoke("RegenerateHealth");
//                isRegenerating = false;
//            }
//        }
//    }
//}

