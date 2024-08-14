using UnityEngine;
using UnityEngine.UI;

public class HeroMenuManager : MonoBehaviour
{
    private HeroInfo currentHeroInfo;
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

    [Header("Skill")]
    public GameObject ArcherSkillPanel;
    public GameObject KnightSkillPanel;
    public GameObject WizardSkillPanel;
    public GameObject PriestSkillPanel;


    private void Awake()
    {
        EquipmentMenu.SetAsLastSibling();
    }

   
    public void OnEquipmentButtonClicked()
    {
        traitManager.HideAllPanels();
        EquipmentMenu.SetAsLastSibling();
    }

    public void OnAttributeButtonClicked()
    {
        traitManager.HideAllPanels();
        AttributeMenu.SetAsLastSibling();
    }

    public void OnLevelUpButtonClicked()
    {
        traitManager.HideAllPanels();
        LevelUpMenu.SetAsLastSibling();
    }

    public void OnSkillButtonClicked()
    {
        traitManager.HideAllPanels();
        SkillMenu.SetAsLastSibling();
    }

    public void OnTalentButtonClicked()
    {
        TraitMenu.SetAsLastSibling();
        if (traitManager == null)
        {
            Debug.LogError("TraitManager is null in HeroMenuManager");
            return;
        }
        if (currentHeroInfo == null)
        {
            Debug.LogError("currentHeroInfo is null in HeroMenuManager");
            return;
        }
        Debug.Log($"Showing trait panel for hero: {currentHeroInfo.heroName}");
        traitManager.ShowTraitPanel(currentHeroInfo);
    }
    public void UpdateCurrentHero(HeroInfo heroInfo)
    {
        currentHeroInfo = heroInfo;
    }

    public void OnPotionEquipmentClicked()
    {
        HeroScreen_Equipment.SetActive(true);
        HeroScreen_Equipment.SetAsLastSibling();
    }

    public void OpPotionEquipmentClosed()
    {
        HeroScreen_Equipment.SetActive(false);
    }
}
