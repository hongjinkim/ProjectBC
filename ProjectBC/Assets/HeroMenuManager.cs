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

    [Header("Potion")]
    public Button PotionEquipmentButton;
    public Button PotionEquipmentCloseButton;
    public Transform HeroScreen_Equipment;

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

    public void OnPotionEquipmentClicked()
    {
        Debug.Log("¿Ö ¾ÈµÅ");
        HeroScreen_Equipment.SetActive(true);
    }

    public void OpPotionEquipmentClosed()
    {
        HeroScreen_Equipment.SetActive(false);
    }
}
