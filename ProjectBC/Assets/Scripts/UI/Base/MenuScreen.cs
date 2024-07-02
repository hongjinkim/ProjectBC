using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public abstract class MenuScreen : MonoBehaviour
{
    [SerializeField] protected string _screenName;

    [SerializeField] protected UIManager _UIManager;

    // visual elements
    protected GameObject _screen;

    public event Action ScreenStarted;
    public event Action ScreenEnded;

    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(_screenName))
            _screenName = this.GetType().Name;
    }

    protected virtual void Awake()
    {
        _screen = this.gameObject;
        // set up MainMenuUIManager and UI Document
        if (_UIManager == null)
            _UIManager = GetComponent<UIManager>();
    }


    public bool IsVisible()
    {
        if (_screen == null)
            return false;

        return (_screen.activeSelf);
    }

    // Toggle a UI on and off using the DisplayStyle. 
    public static void SetObjectActivity(GameObject GO, bool state)
    {
        if (GO == null)
            return;

        GO.SetActive(state);
    }

    public virtual void ShowScreen()
    {
        SetObjectActivity(_screen, true);
        ScreenStarted?.Invoke();
    }

    public virtual void HideScreen()
    {
        if (IsVisible())
        {
            SetObjectActivity(_screen, false);
            ScreenEnded?.Invoke();
        }
    }
}
