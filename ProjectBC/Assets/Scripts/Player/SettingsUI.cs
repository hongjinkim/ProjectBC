using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    private HeroManager heroManager;

    private void Start()
    {
        heroManager = FindObjectOfType<HeroManager>();
        resetButton.onClick.AddListener(OnResetButtonClick);
    }

    private void OnResetButtonClick()
    {
        if (heroManager != null)
        {
            heroManager.ResetHeroData();
        }
    }
}