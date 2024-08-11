using UnityEngine;
using UnityEngine.UI;

public class HeroMenuManager : MonoBehaviour
{
    public TraitManager traitManager;
    [Header("Menus")]
    public Transform EquipmentMenu;
    public Transform AttributeMenu;
    public Transform LevelUpMenu;
    public Transform SkillMenu;
    public Transform TraitMenu;

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
        TraitMenu.SetAsLastSibling();
    }

    public void OnPotionEquipmentClicked()
    {
        HeroScreen_Equipment.SetActive(true);
    }

    public void OpPotionEquipmentClosed()
    {
        HeroScreen_Equipment.SetActive(false);
    }
}
