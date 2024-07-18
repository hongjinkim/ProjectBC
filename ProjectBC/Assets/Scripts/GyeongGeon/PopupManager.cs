using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{

    [Header("Popup")]
    public UIManager uiManager;
    public GameObject adventurePopup;
    public Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void InitAdventurePopup(List<Dungeon> themeList)
    {
        // ThemeName
        adventurePopup.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = themeList[0]._themeName;

        for (int i = 0; i < themeList.Count; i++)
        {
            // StageSlots
            adventurePopup.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = themeList[i]._stageName;
            adventurePopup.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(i).GetChild(1).GetComponent<Image>().sprite = themeList[i]._stageImage;
            adventurePopup.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = "탐색진도 : "+themeList[i]._navigationProgress;
        }
    }

    public void ChangeCameraPos(Vector3 position)
    {
        mainCamera.transform.position = new Vector3(position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
    }

    public void ExitPopup(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

}
