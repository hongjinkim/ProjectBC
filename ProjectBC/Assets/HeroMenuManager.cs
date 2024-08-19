using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroMenuManager : MonoBehaviour
{
    
    public HeroInfo currentHeroInfo;
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

    [Header("Skill Panels")]
    public ArcherSkillPanel archerSkillPanel;
    public KnightSkillPanel knightSkillPanel;
    public WizardSkillPanel wizardSkillPanel;
    public PriestSkillPanel priestSkillPanel;


    private void Awake()
    {
       
        EquipmentMenu.SetAsLastSibling();
    }

   
    public void OnEquipmentButtonClicked()
    {
        traitManager.HideAllPanels();
        EquipmentMenu.SetAsLastSibling();
       
    }


    private void HideAllSkillPanels()
    {
        archerSkillPanel.gameObject.SetActive(false);
        knightSkillPanel.gameObject.SetActive(false);
        wizardSkillPanel.gameObject.SetActive(false);
        priestSkillPanel.gameObject.SetActive(false);
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
        
        HideAllSkillPanels();
        SetCurrentHeroToSkillPanel();
    }
    private void SetCurrentHeroToSkillPanel()
    {
        
        switch (currentHeroInfo.heroClass)
        {
            case HeroClass.Archer:          

                    archerSkillPanel.gameObject.SetActive(true);
                
                    archerSkillPanel.SetCurrentArcher(currentHeroInfo.character as Archer);
                break;
            case HeroClass.Knight:
                knightSkillPanel.gameObject.SetActive(true);
                knightSkillPanel.SetCurrentKnight(currentHeroInfo.character as Knight);
                break;
            case HeroClass.Priest:
                priestSkillPanel.gameObject.SetActive(true);
                priestSkillPanel.SetCurrentPriest(currentHeroInfo.character as Priest);
                break;
            case HeroClass.Wizard:
                wizardSkillPanel.gameObject.SetActive(true);
                wizardSkillPanel.SetCurrentWizard(currentHeroInfo.character as Wizard);
                break;
        }
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
        Debug.Log(heroInfo.heroName);
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
