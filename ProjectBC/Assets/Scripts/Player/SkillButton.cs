using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Text levelText;
    public Image skillIcon;
    public GameObject lockIcon;
    public GameObject maxLevelIcon;

    private Skill skill;

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (skill != null)
        {
            levelText.text = skill.IsUnlocked ? skill.Level.ToString() : "";

            // ��ų ������ ���� (Resources �������� �������� �ε��Ѵٰ� ����)
            string iconPath = "SkillIcons/" + skill.Name; // ��ų �̸��� ���� ������ ���
            Sprite skillSprite = Resources.Load<Sprite>(iconPath);
            if (skillSprite != null)
            {
                skillIcon.sprite = skillSprite;
            }
            else
            {
                Debug.LogWarning("Skill icon not found for: " + skill.Name);
            }

            lockIcon.SetActive(!skill.IsUnlocked);
            maxLevelIcon.SetActive(skill.IsUnlocked && skill.Level == skill.MaxLevel);
        }
        else
        {
            Debug.LogError("Skill is not set for this button.");
        }
    }

    public void OnClick()
    {
        if (skill != null && skill.IsUnlocked)
        {
            // �÷��̾��� LevelUpSkill �޼��� ȣ��
            // ��: Player.Instance.LevelUpSkill(skill.Name);
            Debug.Log("Attempting to level up skill: " + skill.Name);
        }
        else
        {
            Debug.Log("Skill is locked or not set.");
        }
    }
}