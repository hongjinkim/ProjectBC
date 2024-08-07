using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreScreen : MainScreen
{
    [Header("Managers")]
    private DungeonManager dungeonManager;

    [Header("Stages")]
    public Button stage1;

    protected override void Awake()
    {
        base.Awake();
        if(dungeonManager == null)
        {
            dungeonManager = DungeonManager.instance;
        }
    }

    private void Start()
    {
        stage1.onClick.AddListener(() => OpenAdventurePopup("A"));
    }

    private void OpenAdventurePopup(string _themeCode)
    {
        dungeonManager._themeList.Clear();

        foreach (var dungeon in dungeonManager._allDungeonList)
        {
            if (dungeon._themeCode.Equals(_themeCode) && !dungeonManager._themeList.Contains(dungeon))
            {
                dungeonManager._themeList.Add(dungeon);
            }
        }

        
        //_UIManager.ExploreThemepopUp.GetComponent<ExploreThemePopup>().InitAdventurePopup(dungeonManager._themeList);
        _UIManager.DungeonThemePopUp.ShowScreen();

        dungeonManager.SelectDungeon(0);
    }
}
