using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WizardSkillPanel : MonoBehaviour
{
    public Button scorchedEarthButton;
    public Button mysticResonanceButton;
    public Button waveOfHeatButton;

    public TextMeshProUGUI scorchedEarthLevelText;
    public TextMeshProUGUI mysticResonanceLevelText;
    public TextMeshProUGUI waveOfHeatLevelText;

    private Wizard currentWizard;

    private void Start()
    {
        scorchedEarthButton.onClick.AddListener(() => LevelUpSkill(currentWizard.scorchedEarth));
        mysticResonanceButton.onClick.AddListener(() => LevelUpSkill(currentWizard.mysticResonance));
        waveOfHeatButton.onClick.AddListener(() => LevelUpSkill(currentWizard.waveOfHeat));
    }

    public void SetCurrentWizard(Wizard wizard)
    {
        currentWizard = wizard;
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
        scorchedEarthLevelText.text = $"Lv.{currentWizard.scorchedEarth.Level}";
        mysticResonanceLevelText.text = $"Lv.{currentWizard.mysticResonance.Level}";
        waveOfHeatLevelText.text = $"Lv.{currentWizard.waveOfHeat.Level}";
    }
}