using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemSlot : MonoBehaviour
{
    public IItem item;
    public Sprite icon;
    public Image image;

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    void OnItemAdded()
    {
        DisplayUI();
    }

    private void DisplayUI()
    {
        icon = Resources.Load<Sprite>("ItemImg/" + "1");
        image.sprite = icon;
    }
}
