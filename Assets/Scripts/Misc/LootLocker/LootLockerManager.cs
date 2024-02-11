using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class LootLockerManager : SingletonMonobehaviour<LootLockerManager>
{
    [SerializeField] CanvasGroup CgField;
    public TMP_InputField playerNameInputfield;
    string leaderboardID = "19971";

    void Start()
    {
        //StartCoroutine(LoginCor());
    }

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerNameInputfield.text, (response) =>
        {
            if (response.success)
            {
                Settings.PlayerName = playerNameInputfield.text;
                Debug.Log("Succesfully set player name");
            }
            else Debug.Log("Could not set player name");
        });

        OpenGame();
    }

    void OpenGame()
    {
        LoadingScene.Instance.LoadMenu();
        ToggleCgField(false);
    }

    IEnumerator LoginCor()
    {
        yield return LoginRoutine();

        if (Settings.PlayerName != null)
        {
            OpenGame();
        }
        else ToggleCgField(true);
    }

    public void Setup()
    {
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        yield return Leaderboard.Instance.FetchTopHighscoresRoutine();
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if(response.success)
            {
                //Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                //Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }


    public void SubmitScore(int score)
    {
        StartCoroutine(SubmitScoreRoutine(score));
    }

    IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) =>
        {
            if (response.success)
            {
                //Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                //Debug.Log("Failed" + response.errorData.message);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    void ToggleCgField(bool toggle)
    {
        CgField.alpha = toggle ? 1 : 0;
        CgField.blocksRaycasts = toggle;
    }
}
