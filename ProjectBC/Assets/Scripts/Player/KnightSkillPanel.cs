using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KnightSkillPanel : MonoBehaviour
{
    public Button shieldBashButton;
    public Button heavenlyBlessingButton;
    public Button impregnableButton;

    public TextMeshProUGUI shieldBashLevelText;
    public TextMeshProUGUI heavenlyBlessingLevelText;
    public TextMeshProUGUI impregnableLevelText;

    private Knight currentKnight;

    private void Start()
    {
        shieldBashButton.onClick.AddListener(() => LevelUpSkill(currentKnight.shieldBash));
        heavenlyBlessingButton.onClick.AddListener(() => LevelUpSkill(currentKnight.heavenlyBlessing));
        impregnableButton.onClick.AddListener(() => LevelUpSkill(currentKnight.impregnable));
    }

    public void SetCurrentKnight(Knight knight)
    {
        currentKnight = knight;
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
        shieldBashLevelText.text = $"Lv.{currentKnight.shieldBash.Level}";
        heavenlyBlessingLevelText.text = $"Lv.{currentKnight.heavenlyBlessing.Level}";
        impregnableLevelText.text = $"Lv.{currentKnight.impregnable.Level}";
    }
}