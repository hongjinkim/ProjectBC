using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hero : Character
{
    private const float MAX_ENERGY = 100f;
    protected override void Start()
    {
        base.Start();
        //SetStat();
        StartCoroutine(RegenerateEnergy());
    }
    protected virtual void Update()
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
    public float Energy
    {
        get { return info != null ? info.energy : 0; }
        set
        {
            if (info != null)
            {
                info.energy = Mathf.Clamp(value, 0, MAX_ENERGY);
            }
        }
    }
    protected virtual void UseSkill()
    {
        if (Energy >= 100)
        {
            
            // 실제 스킬 사용 로직은 하위 클래스에서 구현
            //Energy = 0;
        }
        else
        {
            
        }
    }
    protected virtual void ApplyPassiveSkill1() { }
    protected virtual void ApplyPassiveSkill2() { }
    protected virtual void ApplyPassiveSkill3() { }
    protected void CheckAndUseSkill()
    {
        if (info.energy >= 100)
        {
            UseSkill();
        }
    }
}