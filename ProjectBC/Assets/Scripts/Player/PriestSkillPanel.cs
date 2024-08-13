using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PriestSkillPanel : MonoBehaviour
{
    public Button purifyingLightButton;
    public Button dazzlingLightButton;
    public Button holyGraceButton;
    public Button mysticalPowerButton;

    public TextMeshProUGUI purifyingLightLevelText;
    public TextMeshProUGUI dazzlingLightLevelText;
    public TextMeshProUGUI holyGraceLevelText;
    public TextMeshProUGUI mysticalPowerLevelText;

    private Priest currentPriest;

    private void Start()
    {
        purifyingLightButton.onClick.AddListener(() => LevelUpSkill(currentPriest.purifyingLight));
        dazzlingLightButton.onClick.AddListener(() => LevelUpSkill(currentPriest.dazzlingLight));
        holyGraceButton.onClick.AddListener(() => LevelUpSkill(currentPriest.holyGrace));
        mysticalPowerButton.onClick.AddListener(() => LevelUpSkill(currentPriest.mysticalPower));
    }

    public void SetCurrentPriest(Priest priest)
    {
        currentPriest = priest;
        UpdateSkillLevels();
    }

    private void LevelUpSkill(PlayerSkill skill)
    {
        if (skill.Level < skill.MaxLevel)
        {
            skill.LevelUp();
            UpdateSkillLevels();
        }
    }

    private void UpdateSkillLevels()
    {
        purifyingLightLevelText.text = $"Lv.{currentPriest.purifyingLight.Level}";
        dazzlingLightLevelText.text = $"Lv.{currentPriest.dazzlingLight.Level}";
        holyGraceLevelText.text = $"Lv.{currentPriest.holyGrace.Level}";
        mysticalPowerLevelText.text = $"Lv.{currentPriest.mysticalPower.Level}";
    }
}