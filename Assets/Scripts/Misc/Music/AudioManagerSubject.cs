using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerSubject : Subject
{
    protected override void Awake()
    {
        base.Awake();

        AddAction(EnumsActions.SkillUsed, OnSkillUsed);
        AddAction(EnumsActions.AttackUsed, OnAttackUsed);
        AddAction(EnumsActions.Gameover, OnGameover);

        AddAction(EnumsActions.UIButtonPressed, OnUIButtonPressed);
    }

    //actions
    void OnSkillUsed() => AudioManager.Instance.PlayOneShot("OnSkillUsed");
    void OnAttackUsed() => AudioManager.Instance.PlayOneShot("OnAttackUsed");
    void OnGameover() => AudioManager.Instance.PlayOneShot("OnGameover");
    void OnUIButtonPressed() => AudioManager.Instance.PlayOneShot("OnUIButtonPressed");
}
