using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPickupText : MonoBehaviour
{
    private Transform grid;
    private TextMeshProUGUI textUGUI;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        textUGUI = GetComponent<TextMeshProUGUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        grid = GameDataManager.instance.noticeTransform;

        transform.parent = grid;
    }
    public void SetText(string text)
    {
        textUGUI.text = text;
    }
}
