using UnityEngine;

public class Archer : Hero
{
    public HeroClass _heroClass;
    private ArcherSkills archerSkills;
    private LineRenderer skillRangeIndicator;
    protected override void Start() 
    {
        base.Start();
        _heroClass = HeroClass.Archer;
        playerStat.CharacteristicType = CharacteristicType.Agility;
        archerSkills = new ArcherSkills(playerStat);
        // LineRenderer 컴포넌트 추가 및 설정
        skillRangeIndicator = gameObject.AddComponent<LineRenderer>();
        skillRangeIndicator.positionCount = 5; // 직사각형을 그리기 위해 5개의 점이 필요합니다 (시작점으로 돌아오기 위해)
        skillRangeIndicator.startWidth = 0.1f;
        skillRangeIndicator.endWidth = 0.1f;
        skillRangeIndicator.material = new Material(Shader.Find("Sprites/Default"));
        skillRangeIndicator.startColor = Color.yellow;
        skillRangeIndicator.endColor = Color.yellow;
        skillRangeIndicator.enabled = false; // 초기에는 비활성화
    }
    public void ShowSkillRange()
    {
        PierceShot pierceShot = archerSkills.GetActiveSkill() as PierceShot;
        if (pierceShot != null)
        {
            var (topLeft, topRight, bottomRight, bottomLeft) = pierceShot.CalculateSkillArea(this);

            skillRangeIndicator.SetPosition(0, topLeft);
            skillRangeIndicator.SetPosition(1, topRight);
            skillRangeIndicator.SetPosition(2, bottomRight);
            skillRangeIndicator.SetPosition(3, bottomLeft);
            skillRangeIndicator.SetPosition(4, topLeft);

            skillRangeIndicator.enabled = true;
        }
    }

    public void HideSkillRange()
    {
        skillRangeIndicator.enabled = false;
    }

    // 스킬 사용 시 범위 표시를 업데이트하는 메서드
    private void UpdateSkillRangeIndicator()
    {
        if (_target != null)
        {
            ShowSkillRange();
        }
        else
        {
            HideSkillRange();
        }
    }

    protected override void Update()
    {
        base.Update();
        archerSkills.CheckAndUseActiveSkill();
        UpdateSkillRangeIndicator(); // 매 프레임마다 스킬 범위 표시를 업데이트합니다
    }
    protected override void OnAnimAttack()
    {
		animator.SetTrigger("ShotBow");
        IsAction = true;
    }
    public override void IncreaseCharacteristic(float amount)
    {
        IncreaseAgility(amount * 2);
    }

    // Archer 특유의 메서드 추가
    public void UsePierceShot()
    {
        archerSkills.UseActiveSkill();
    }
}
