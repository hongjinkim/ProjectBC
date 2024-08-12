using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonTheme : PopUp
{
    private DungeonManager dungeonManager;

    [Header("Buttons")]
    public Button dungeonEnterButton;

    [Header("Texts")]
    public TextMeshProUGUI themeNameText;

    protected override void Awake()
    {
        base.Awake();
        if (dungeonManager == null)
        {
            dungeonManager = DungeonManager.instance;
        }
    }

    protected override void Start()
    {
        base.Start();
        dungeonEnterButton.onClick.AddListener(EnterDungeon);
    }

    private void EnterDungeon()
    {
        _UIManager.DungeonThemePopUp.HideScreen();

        dungeonManager.EnterDungeon();



    }

    public void InitAdventurePopup(List<Dungeon> themeList)
    {
        //ThemeName
        //adventurePopup.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = themeList[0]._themeName;

        for (int i = 0; i < themeList.Count; i++)
        {
            // StageSlots
            //adventurePopup.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = themeList[i]._stageName;
            //adventurePopup.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(i).GetChild(1).GetComponent<Image>().sprite = themeList[i]._stageImage;
            //adventurePopup.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = "탐색진도 : " + themeList[i]._navigationProgress;
        }
    }

}
