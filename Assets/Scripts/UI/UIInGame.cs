using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGame : Subject
{
    [SerializeField] TextMeshProUGUI TimerText;

    //local

    //time
    float _curTime;
    float _curMilisecs;
    float _curSecs;
    float _curMins;

    string[] _timeStrings = new string[3];

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < 3; i++) _timeStrings[i] = "00"; 
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

        if (_curMilisecs >= 1000)
        {
            _curMilisecs -= 1000;
            _curSecs++;
            _timeStrings[1] = GetStringFromTime(_curSecs);
        }

        if (_curSecs >= 60)
        {
            _curSecs -= 60;
            _curMins++;
            _timeStrings[0] = GetStringFromTime(_curMins);
        }

        TimerText.text = string.Join(':', _timeStrings);
    }

    //other scripts
    string GetStringFromTime(float val)
    {
        return val > 10 ? val.ToString() : $"0{val}";
    }
}
