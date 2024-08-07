using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : BaseScreen
{
    public void Start()
    {
        HideScreen();
    }

    public override void ShowScreen()
    {
        transform.SetAsLastSibling();
    }

    public override void HideScreen()
    {
        transform.SetAsFirstSibling();
    }
}
