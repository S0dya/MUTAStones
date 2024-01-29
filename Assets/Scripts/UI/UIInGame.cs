using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGame : Subject
{
    [Header("delay images")]
    [SerializeField] Image AttackDelayImage;
    [SerializeField] Image SkillDelayImage;

    [Header("timer")]
    [SerializeField] TextMeshProUGUI TimerText;

    //local

    //time
    float _curMilisecs;
    float _curSecs;
    float _curMins;

    string[] _timeStrings = new string[3];


    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < 3; i++) _timeStrings[i] = "00";

        AddAction(EnumsActions.AttackUsed, OnAttackUsed);
        AddAction(EnumsActions.SkillUsed, OnSkillUsed);

        AddAction(EnumsActions.Gameover, OnGameover);
    }

    void Update()
    {
        UpdateTime();
    }

    //time
    void UpdateTime()
    {
        _curMilisecs += (int)(Time.deltaTime * 1000);
        _timeStrings[2] = GetStringFromTime(_curMilisecs).Substring(0, 2);

        if (_curMilisecs >= 999)
        {
            _curMilisecs -= 999;
            _curSecs++;
            _timeStrings[1] = GetStringFromTime(_curSecs);

            if (_curSecs % 10 == 0)
            {
                LevelManager.Instance.DecreaseSpawnDelay();

                if (_curSecs % 30 == 0) LevelManager.Instance.IncreaseEnemiesAmount();
            }

            if (_curSecs >= 60)
            {
                _curSecs -= 60;
                _curMins++;

                _timeStrings[1] = GetStringFromTime(_curSecs);
                _timeStrings[0] = GetStringFromTime(_curMins);

                Observer.Instance.NotifyObservers(EnumsActions.ResetMutation);
            }
        }

       

        TimerText.text = string.Join(':', _timeStrings);
    }

    //actions
    void OnAttackUsed() => StartCoroutine(DelayCor(AttackDelayImage, GameManager.Instance.GameData.AttackDelayDuration, EnumsActions.AttackRestored));
    void OnSkillUsed() => StartCoroutine(DelayCor(SkillDelayImage, GameManager.Instance.GameData.SkillDelayDuration, EnumsActions.SkillRestored));

    void OnGameover()
    {
        int score = (int)(_curMins * 60 + _curSecs + _curMilisecs / 1000 * GameManager.Instance.GameData.MutationsAmount);

        GameManager.Instance.GameData.Score = score;

        LootLockerManager.Instance.SubmitScore(score);
    }

    //cors
    IEnumerator DelayCor(Image image, float duration, EnumsActions enumAction)
    {
        float elapsedTime = duration;

        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            image.fillAmount = Mathf.Clamp01(elapsedTime / duration);

            yield return null;
        }

        Observer.Instance.NotifyObservers(enumAction);
    }

    //other scripts
    string GetStringFromTime(float val)
    {
        return val >= 10 ? val.ToString() : $"0{val}";
    }
}
