using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionSlot : MonoBehaviour
{
    public Image potionIcon;
    public Image potionBackground;
    public Image potionFrame;
    public TextMeshProUGUI potionCount;

    public HeroPotion heroPotion;

    private void Awake()
    {
        if (heroPotion != null)
            heroPotion = GetComponent<HeroPotion>();
    }

    private void Start()
    {
        Debug.Log("PotionSlot Start method called");
        if (heroPotion != null && heroPotion.changeInformationButton != null)
        {
            Debug.Log("Adding listener to changeInformationButton");
            heroPotion.changeInformationButton.onClick.AddListener(SetButtonInformation);
        }
        else
        {
            Debug.LogError("HeroPotion or changeInformationButton is null");
        }
    }

    public void SetButtonInformation()
    {
        Debug.Log("SetButtonInformation called");
        if (heroPotion != null)
        {
            Debug.Log($"Selected potion index: {heroPotion.selectedPotionIndex}");
            if (heroPotion.selectedPotionIndex >= 0 &&
                heroPotion.selectedPotionIndex < heroPotion._potionButtons.Length)
            {
                Button selectedButton = heroPotion._potionButtons[heroPotion.selectedPotionIndex];
                if (selectedButton != null && selectedButton.image != null)
                {
                    Debug.Log("Updating potionIcon sprite");
                    potionIcon.sprite = selectedButton.image.sprite;
                }
                else
                {
                    Debug.LogWarning("Selected button or its image is null");
                }
            }
            else
            {
                Debug.LogWarning("Invalid selected potion index");
            }
        }
        else
        {
            Debug.LogError("HeroPotion is null");
        }
    }
}
