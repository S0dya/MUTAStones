using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class LootLockerManager : SingletonMonobehaviour<LootLockerManager>
{
    public TMP_InputField playerNameInputfield;
    string leaderboardID = "19971";

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerNameInputfield.text, (response) =>
        {
            /*
            if(response.success) Debug.Log("Succesfully set player name");
            else Debug.Log("Could not set player name");
            */
        });
    }

    public void Setup()
    {
        StartCoroutine(SetupRoutine());
    }


    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
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
}
