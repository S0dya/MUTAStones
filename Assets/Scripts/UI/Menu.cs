using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] CanvasGroup SettingsCg;

    [Header("Settings")]
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;

    [SerializeField] CanvasGroup CgMusic;

    void Start()
    {
        ToggleMusicCG();
    }

    //buttons
    public void OnPlayButton()
    {
        LoadingScene.Instance.OpenScene(2);
    }
    public void OnSettingsButton()
    {
        ToggleCG(SettingsCg, true);
    }
    public void OnQuitButton()
    {
        Application.Quit();
    }

    //settings buttons
    public void OnToggleMusicButton()
    {
        AudioManager.Instance.ToggleMusic(!Settings.isMusicOn);
        ToggleMusicCG();
    }
    public void OnMusicChangeSlider(float val)
    {
        AudioManager.Instance.SetVolume(0, val);
    }
    public void OnSfxChangeSlider(float val)
    {
        AudioManager.Instance.SetVolume(1, val);
    }

    public void OnCloseSettings()
    {
        ToggleCG(SettingsCg, false);
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
}
