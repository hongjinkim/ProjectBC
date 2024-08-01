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
        // LineRenderer ������Ʈ �߰� �� ����
        skillRangeIndicator = gameObject.AddComponent<LineRenderer>();
        skillRangeIndicator.positionCount = 5; // ���簢���� �׸��� ���� 5���� ���� �ʿ��մϴ� (���������� ���ƿ��� ����)
        skillRangeIndicator.startWidth = 0.1f;
        skillRangeIndicator.endWidth = 0.1f;
        skillRangeIndicator.material = new Material(Shader.Find("Sprites/Default"));
        skillRangeIndicator.startColor = Color.yellow;
        skillRangeIndicator.endColor = Color.yellow;
        skillRangeIndicator.enabled = false; // �ʱ⿡�� ��Ȱ��ȭ
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

    // ��ų ��� �� ���� ǥ�ø� ������Ʈ�ϴ� �޼���
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
        UpdateSkillRangeIndicator(); // �� �����Ӹ��� ��ų ���� ǥ�ø� ������Ʈ�մϴ�
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

    // Archer Ư���� �޼��� �߰�
    public void UsePierceShot()
    {
        archerSkills.UseActiveSkill();
    }
}
