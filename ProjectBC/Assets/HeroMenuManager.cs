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
                Debug.Log($"Attempting to set Archer: {currentHeroInfo.character}");
                if (currentHeroInfo.character == null)
                {
                    Debug.LogError("currentHeroInfo.character is null for Archer");
                }
                else if (!(currentHeroInfo.character is Archer))
                {
                    Debug.LogError($"currentHeroInfo.character is not an Archer. Actual type: {currentHeroInfo.character.GetType()}");
                }
                else
                {
                    archerSkillPanel.gameObject.SetActive(true);
                    archerSkillPanel.SetCurrentArcher(currentHeroInfo.character as Archer);
                }
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
        currentHeroInfo = heroInfo;
    }

    public void OnPotionEquipmentClicked()
    {
        MainUIManager.instance.PotionPopUp.GetComponent<Potion>().UpdatePotion(currentHeroInfo.PotionItem.id);
    }

    public void OpPotionEquipmentClosed()
    {
        MainUIManager.instance.PotionPopUp.HideScreen();
    }
}
