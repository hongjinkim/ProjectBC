using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPickupText : MonoBehaviour
{
    
    private TextMeshProUGUI textUGUI;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        textUGUI = GetComponent<TextMeshProUGUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.SetParent(GameManager.instance.pickUpGrid);
    }
    public void SetText(string text)
    {
        textUGUI.text = text;
    }
}
