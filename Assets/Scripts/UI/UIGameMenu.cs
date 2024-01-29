using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class UIGameMenu : Subject
{
    [SerializeField] CanvasGroup CgMenu;
    [SerializeField] CanvasGroup CgGameover;

    [SerializeField] CanvasGroup CgMusic;

    [SerializeField] TextMeshProUGUI ScoreText;

    [SerializeField] StudioEventEmitter EventEmmiter;


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

    void Start()
    {
        ToggleMusicCG();
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
        AudioManager.Instance.ToggleMusic(!Settings.isMusicOn);
        ToggleMusicCG();
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
        EventEmmiter.Stop();
        StartCoroutine(SetScoreCor());
        _isGameoverOpened = true;
     
        TogglePanel(CgGameover, true);
    }

    public void OnButtonPressed() => Observer.Instance.NotifyObservers(EnumsActions.UIButtonPressed);

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

    void ToggleMusicCG()
    {
        CgMusic.alpha = Settings.isMusicOn ? 1 : 0.6f;
    }

    IEnumerator SetScoreCor()
    {
        yield return null;

        ScoreText.text = GameManager.Instance.GameData.Score.ToString();
    }
}
