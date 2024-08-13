using UnityEngine;
using UnityEngine.UI;

public class DailyStore : PopUp
{
    [Header("Buttons")]
    public Button backButton;

    protected override void Start()
    {
        base.Start();
        backButton.onClick.AddListener(_UIManager.DailyStorePopUp.HideScreen);
    }
}
