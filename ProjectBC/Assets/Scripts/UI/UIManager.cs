using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] GameObject _campScreen;
    [SerializeField] GameObject _exploreScreen;
    [SerializeField] GameObject _battleScreen;
    [SerializeField] GameObject _heroScreen;
    [SerializeField] GameObject _inventoryScreen;

    [Header("Toolbars")]
    [SerializeField] GameObject _playerInfoBar;
    [SerializeField] GameObject _menuBar;

    [Header("Full-screen overlays")]


    List<GameObject> _allScreens = new List<GameObject>();

    

    void OnEnable()
    {
        SetupScreens();
        ShowHomeScreen();
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    void SetupScreens()
    {
        if (_campScreen != null)
            _allScreens.Add(_campScreen);

        if (_exploreScreen != null)
            _allScreens.Add(_exploreScreen);

        if (_battleScreen != null)
            _allScreens.Add(_battleScreen);

        if (_heroScreen != null)
            _allScreens.Add(_heroScreen);

        if (_inventoryScreen != null)
            _allScreens.Add(_inventoryScreen);
    }

    // shows one screen at a time
    void ShowScreen(GameObject screen)
    {
        foreach (GameObject go in _allScreens)
        {
            MenuScreen m = go.GetComponent<MenuScreen>();
            if (go == screen)
            {
                m?.ShowScreen();
            }
            else
            {
                m?.HideScreen();
            }
        }
    }

    // methods to toggle screens on/off

    public void ShowHomeScreen()
    {
        ShowScreen(_campScreen);
    }


    public void ShowCampScreen()
    {
        ShowScreen(_campScreen);
    }

    public void ShowExploreScreen()
    {
        ShowScreen(_exploreScreen);
    }

    public void ShowBattleScreen()
    {
        ShowScreen(_battleScreen);
    }

    public void ShowHeroScreen()
    {
        ShowScreen(_heroScreen);
    }

    public void ShowInventoryScreen()
    {
        ShowScreen(_inventoryScreen);
    }

    // overlay screen methods
    //public void ShowSettingsScreen()
    //{

    //}

    //public void ShowInventoryScreen()
    //{

    //}
}
