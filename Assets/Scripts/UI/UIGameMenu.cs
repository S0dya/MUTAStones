using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameMenu : Subject
{
    [SerializeField] CanvasGroup CgMenu;
    [SerializeField] CanvasGroup CgGameover;


    //local
    //float _lastTime;

    bool _isMenuOpened;



    protected override void Awake()
    {
        base.Awake();

        AddAction(EnumsActions.Escape, OnEscape);
    }


    //buttons
    public void OnResumerButton()
    {
        CloseMenu();
    }

    public void OnQuitButton()
    {

    }

    public void OnToggleMusicButton()
    {

    }

    //actions
    void OnEscape()
    {
        if (_isMenuOpened) CloseMenu();
        else OpenMenu();
    }

    //main methods
    void OpenMenu()
    {
        _isMenuOpened = true;

        Time.timeScale = 0;

        ToggleCG(CgMenu, true);
    }
    void CloseMenu()
    {
        _isMenuOpened = false;

        Time.timeScale = 1;

        ToggleCG(CgMenu, false);
    }

    //other methods
    void ToggleCG(CanvasGroup cg, bool toggle)
    {
        cg.alpha = toggle ? 1 : 0;
        cg.blocksRaycasts = toggle;
    }

}
