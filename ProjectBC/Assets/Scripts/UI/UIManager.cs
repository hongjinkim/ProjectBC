using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] MenuScreen _campScreen;
    [SerializeField] MenuScreen _exploreScreen;
    [SerializeField] MenuScreen _battleScreen;
    [SerializeField] MenuScreen _heroScreen;
    [SerializeField] MenuScreen _inventoryScreen;

    [Header("Toolbars")]
    [SerializeField] MenuScreen _playerInfoBar;
    [SerializeField] MenuScreen _menuBar;

    [Header("Full-screen overlays")]


    List<MenuScreen> _allScreens = new List<MenuScreen>();
    
    Camera mainCamera;

    void OnEnable()
    {
        SetupScreens();
        
    }

    void Start()
    {
        Time.timeScale = 1f;
        mainCamera = Camera.main;
        ShowHomeScreen();
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
    void ShowScreen(MenuScreen screen)
    {
        foreach (MenuScreen m in _allScreens)
        {
            if (m == screen)
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
        ResetCameraPos();
    }


    public void ShowCampScreen()
    {
        ShowScreen(_campScreen);
        ResetCameraPos();
    }

    public void ShowExploreScreen()
    {
        ShowScreen(_exploreScreen);
        ResetCameraPos();
    }

    public void ShowBattleScreen()
    {
        ShowScreen(_battleScreen);
        ChangeCameraPos( GameManager.instance.dungeonManager._selectDungeon.transform.position);
    }

    public void ShowHeroScreen()
    {
        ShowScreen(_heroScreen);
        ResetCameraPos();
    }

    public void ShowInventoryScreen()
    {
        ShowScreen(_inventoryScreen);
        ResetCameraPos();
    }

    public void TogglePlayerInfo(bool onOff)
    {
        if (onOff)
            _playerInfoBar.ShowScreen();
        else
            _playerInfoBar.HideScreen();
    }

    public void ToggleMenuBar(bool onOff)
    {
        if (onOff)
            _menuBar.ShowScreen();
        else
            _menuBar.HideScreen();
    }

    public void ResetCameraPos()
    {
        mainCamera.transform.position = new Vector3(0, 0, -10);
    }

    public void ChangeCameraPos(Vector3 position)
    {
        mainCamera.transform.position = new Vector3(position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
    }

    // overlay screen methods
    //public void ShowSettingsScreen()
    //{

    //}

    //public void ShowInventoryScreen()
    //{

    //}
}
