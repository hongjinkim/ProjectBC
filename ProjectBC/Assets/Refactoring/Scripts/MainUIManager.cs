using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoSingleton<MainUIManager>
{
    [Header("Main Screens")]
    [SerializeField] MainScreen _campScreen;
    [SerializeField] MainScreen _exploreScreen;
    [SerializeField] MainScreen _battleScreen;
    [SerializeField] MainScreen _heroScreen;
    [SerializeField] MainScreen _inventoryScreen;

    [Header("Toolbars")]
    [SerializeField] BaseScreen _playerInfoBar;
    [SerializeField] BaseScreen _menuBar;

    [Header("PopUp")]
    [SerializeField] PopUp _popUp;

    List<MainScreen> _allMainScreens = new List<MainScreen>();

    private Camera mainCamera;

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
            _allMainScreens.Add(_campScreen);

        if (_exploreScreen != null)
            _allMainScreens.Add(_exploreScreen);

        if (_battleScreen != null)
            _allMainScreens.Add(_battleScreen);

        if (_heroScreen != null)
            _allMainScreens.Add(_heroScreen);

        if (_inventoryScreen != null)
            _allMainScreens.Add(_inventoryScreen);
    }

    // shows one main screen at a time
    void ShowMainScreen(MainScreen screen)
    {
        foreach (MainScreen m in _allMainScreens)
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
        ShowMainScreen(_campScreen);
        ResetCameraPos();
    }


    public void ShowCampScreen()
    {
        ShowMainScreen(_campScreen);
        ResetCameraPos();
    }

    public void ShowExploreScreen()
    {
        ShowMainScreen(_exploreScreen);
        ResetCameraPos();
    }

    public void ShowBattleScreen()
    {
        ShowMainScreen(_battleScreen);
        GameManager.instance.dungeonManager.popupManager.ChangeCameraPos(GameManager.instance.dungeonManager._selectDungeon.transform.position);
    }

    public void ShowHeroScreen()
    {
        ShowMainScreen(_heroScreen);
        ResetCameraPos();
    }

    public void ShowInventoryScreen()
    {
        ShowMainScreen(_inventoryScreen);
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
}
