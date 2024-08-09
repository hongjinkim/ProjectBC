using UnityEngine;
using System.Collections.Generic;

public class TraitManager : MonoBehaviour
{
    public GameObject concentrationPanel;
    public GameObject magicPanel;
    public GameObject protectionPanel;
    public ConcentrationTrait concentrationTrait;
    public MagicTrait magicTrait;
    public ProtectionTrait protectionTrait;
    // ... 다른 특성 패널들 ...
    private HeroInfo currentHeroInfo;

    public void ShowTraitPanel(HeroInfo heroInfo)
    {
        HideAllPanels();

        if (heroInfo.traits.Count > 0)
        {
            TraitType traitType = heroInfo.traits[0].Type;
            GameObject panel = GetPanelForTraitType(traitType);
            if (panel != null)
            {
                panel.SetActive(true);
                ITraitPanel traitPanel = panel.GetComponent<ITraitPanel>();
                if (traitPanel != null)
                {
                    traitPanel.Initialize(heroInfo);
                }
            }
        }
    }

    private void HideAllPanels()
    {
        concentrationPanel.SetActive(false);
        magicPanel.SetActive(false);
        protectionPanel.SetActive(false);
        // ... 다른 패널들도 비활성화
    }

    private GameObject GetPanelForTraitType(TraitType traitType)
    {
        switch (traitType)
        {
            case TraitType.Concentration:
                return concentrationPanel;
            case TraitType.Magic:
                return magicPanel;
            case TraitType.Protection:
                return protectionPanel;
            // ... 다른 특성 타입에 대한 case 추가
            default:
                return null;
        }
    }
    public void SetCurrentHero(HeroInfo heroInfo)
    {
        currentHeroInfo = heroInfo;
    }

    public void ApplyConcentrationTrait(int level, bool isLeftTrait)
    {
        concentrationTrait.ChooseTrait(level, isLeftTrait);
        Character currentCharacter = GetCurrentCharacter();
        if (currentCharacter != null)
        {
            concentrationTrait.ApplyEffect(currentCharacter);
        }
        else
        {
            Debug.LogWarning("No current character found to apply trait.");
        }
    }

    public void ApplyMagicTrait(int level, bool isLeftTrait)
    {
        magicTrait.ChooseTrait(level, isLeftTrait);
        Character currentCharacter = GetCurrentCharacter();
        if (currentCharacter != null)
        {
            magicTrait.ApplyEffect(currentCharacter);
        }
        else
        {
            Debug.LogWarning("No current character found to apply trait.");
        }
    }

    public void ApplyProtectionTrait(int level, bool isLeftTrait)
    {
        protectionTrait.ChooseTrait(level, isLeftTrait);
        Character currentCharacter = GetCurrentCharacter();
        if (currentCharacter != null)
        {
            protectionTrait.ApplyEffect(currentCharacter);
        }
        else
        {
            Debug.LogWarning("No current character found to apply trait.");
        }
    }

    private Character GetCurrentCharacter()
    {
        if (currentHeroInfo != null && currentHeroInfo.character != null)
        {
            return currentHeroInfo.character;
        }
        else
        {
            Debug.LogWarning("Current hero or character is null.");
            return null;
        }
    }
    // Concentration Trait
    public void ApplyConcentrationTraitLeft10() { ApplyConcentrationTrait(10, true); }
    public void ApplyConcentrationTraitRight10() { ApplyConcentrationTrait(10, false); }
    public void ApplyConcentrationTraitLeft20() { ApplyConcentrationTrait(20, true); }
    public void ApplyConcentrationTraitRight20() { ApplyConcentrationTrait(20, false); }
    public void ApplyConcentrationTraitLeft30() { ApplyConcentrationTrait(30, true); }
    public void ApplyConcentrationTraitRight30() { ApplyConcentrationTrait(30, false); }
    public void ApplyConcentrationTraitLeft40() { ApplyConcentrationTrait(40, true); }
    public void ApplyConcentrationTraitRight40() { ApplyConcentrationTrait(40, false); }

    // Magic Trait
    public void ApplyMagicTraitLeft10() { ApplyMagicTrait(10, true); }
    public void ApplyMagicTraitRight10() { ApplyMagicTrait(10, false); }
    public void ApplyMagicTraitLeft20() { ApplyMagicTrait(20, true); }
    public void ApplyMagicTraitRight20() { ApplyMagicTrait(20, false); }
    public void ApplyMagicTraitLeft30() { ApplyMagicTrait(30, true); }
    public void ApplyMagicTraitRight30() { ApplyMagicTrait(30, false); }
    public void ApplyMagicTraitLeft40() { ApplyMagicTrait(40, true); }
    public void ApplyMagicTraitRight40() { ApplyMagicTrait(40, false); }

    // Protection Trait
    public void ApplyProtectionTraitLeft10() { ApplyProtectionTrait(10, true); }
    public void ApplyProtectionTraitRight10() { ApplyProtectionTrait(10, false); }
    public void ApplyProtectionTraitLeft20() { ApplyProtectionTrait(20, true); }
    public void ApplyProtectionTraitRight20() { ApplyProtectionTrait(20, false); }
    public void ApplyProtectionTraitLeft30() { ApplyProtectionTrait(30, true); }
    public void ApplyProtectionTraitRight30() { ApplyProtectionTrait(30, false); }
    public void ApplyProtectionTraitLeft40() { ApplyProtectionTrait(40, true); }
    public void ApplyProtectionTraitRight40() { ApplyProtectionTrait(40, false); }
}

// 모든 특성 패널이 구현해야 할 인터페이스
public interface ITraitPanel
{
    void Initialize(HeroInfo heroInfo);
}