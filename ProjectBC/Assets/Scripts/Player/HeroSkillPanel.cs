using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HeroSkillPanel : MonoBehaviour
{
    public List<Button> skillButtons;
    public List<TextMeshProUGUI> skillLevelTexts;
    private HeroInfo currentHeroInfo;

    public void Initialize(HeroInfo heroInfo)
    {
        currentHeroInfo = heroInfo;
        UpdateSkillUI();
    }

    private void UpdateSkillUI()
    {
        for (int i = 0; i < currentHeroInfo.skills.Count; i++)
        {
            int index = i; // Capture the index for the lambda
            PlayerSkill skill = currentHeroInfo.skills[i];

            skillButtons[i].onClick.RemoveAllListeners();
            skillButtons[i].onClick.AddListener(() => OnSkillButtonClicked(index));

            UpdateSkillButtonText(i);
        }
    }

    private void UpdateSkillButtonText(int index)
    {
        PlayerSkill skill = currentHeroInfo.skills[index];
        if (skill.Level == 0)
        {
            skillLevelTexts[index].text = "Unlock";
        }
        else
        {
            skillLevelTexts[index].text = $"Level {skill.Level}";
        }
    }

    private void OnSkillButtonClicked(int index)
    {
        PlayerSkill skill = currentHeroInfo.skills[index];
        if (skill.Level == 0)
        {
            skill.LevelUp(); // This will set the level to 1
            if (skill is PlayerSkill activeSkill)
            {
                currentHeroInfo.activeSkill = activeSkill;
            }
        }
        else if (skill.Level < skill.MaxLevel)
        {
            skill.LevelUp();
        }

        UpdateSkillButtonText(index);

        // If it's a passive skill, apply its effects
        ApplyPassiveSkillEffects();
    }

    private void ApplyPassiveSkillEffects()
    {
        // This method should be implemented in each hero class to apply passive skill effects
        if (currentHeroInfo.character is Archer archer)
        {
            archer.UpdatePassiveSkills();
        }
        // Add similar checks for other hero types
    }
}