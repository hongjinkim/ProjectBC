using UnityEngine;
using UnityEngine.UI;

public class HeroMenuManager : MonoBehaviour
{
    [Header("Menus")]
    public Transform EquipmentMenu;
    public Transform AttributeMenu;
    public Transform LevelUpMenu;
    public Transform SkillMenu;
    public Transform TalentMenu;

    private void Awake()
    {
        EquipmentMenu.SetAsLastSibling();
    }

    public void OnEquipmentButtonClicked()
    {
        EquipmentMenu.SetAsLastSibling();
    }

    public void OnAttributeButtonClicked()
    {
        AttributeMenu.SetAsLastSibling();
    }

    public void OnLevelUpButtonClicked()
    {
        LevelUpMenu.SetAsLastSibling();
    }

    public void OnSkillButtonClicked()
    {
        SkillMenu.SetAsLastSibling();
    }

    public void OnTalentButtonClicked()
    {
        TalentMenu.SetAsLastSibling();
    }
}
