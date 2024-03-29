using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class LootLockerManager : SingletonMonobehaviour<LootLockerManager>
{
    public TMP_InputField playerNameInputfield;

    void Start()
    {
        StartCoroutine(SetupRoutine());
    }

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
}
