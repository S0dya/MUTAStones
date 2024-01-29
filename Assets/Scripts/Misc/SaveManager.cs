using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : SingletonMonobehaviour<SaveManager>
{

    protected override void Awake()
    {
        base.Awake();

        //LoadData();
        Settings.firstTime = false;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveData();
        }
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("PlayerID", Settings.PlayerID);

        for (int i = 0; i < Settings.musicStats.Length; i++) PlayerPrefs.SetFloat($"musicStats {i}", Settings.musicStats[i]);
        
        PlayerPrefs.SetInt("firstTime", Settings.firstTime ? 0 : 1);
        PlayerPrefs.SetInt("isMusicOn", Settings.isMusicOn ? 0 : 1);
    }

    public void LoadData()
    {
        if (PlayerPrefs.GetInt("firstTime") == 1) return;

        Settings.isMusicOn = PlayerPrefs.GetInt("isMusicOn") == 1;

        for (int i = 0; i < Settings.musicStats.Length; i++) Settings.musicStats[i] = PlayerPrefs.GetFloat($"musicStats {i}");

        Settings.PlayerID = PlayerPrefs.GetString("PlayerID");
    }
}
