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
    bool _isGameoverOpened;


    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1;
        AddAction(EnumsActions.Escape, OnEscape);
        AddAction(EnumsActions.Gameover, OnGameover);
    }


    //buttons
    public void OnResumeButton()
    {
        TogglePanel(CgMenu, false);
    }

    public void OnQuitButton()
    {
        LoadingScene.Instance.OpenMenu(2);
    }

    public void OnToggleMusicButton()
    {

    }

    //buttons gameover
    public void OnReplayButton()
    {
        LoadingScene.Instance.OpenScene(2, 2);
    }

    //actions
    void OnEscape()
    {
        if (_isGameoverOpened) return;

        if (_isMenuOpened) TogglePanel(CgMenu, false);
        else TogglePanel(CgMenu, true);
    }

    void OnGameover()
    {
        _isGameoverOpened = true;
     
        TogglePanel(CgGameover, true);
    }

    //main methods
    void TogglePanel(CanvasGroup cg, bool toggle)
    {
        _isMenuOpened = toggle;
        Time.timeScale = !toggle ? 1 : 0;
        ToggleCG(cg, toggle);
    }


    //other methods
    void ToggleCG(CanvasGroup cg, bool toggle)
    {
        cg.alpha = toggle ? 1 : 0;
        cg.blocksRaycasts = toggle;
    }

}
