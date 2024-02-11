using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class Subject : MonoBehaviour
{
    [SerializeField] ActionsDictionary[] ActionsDictionaries;

    //local
    Dictionary<EnumsActions, Action> _actionDictionary = new Dictionary<EnumsActions, Action>();

    public void AddAction(EnumsActions enumAction, Action action) => _actionDictionary.Add(enumAction, action);

    protected virtual void Awake()
    {
        foreach (ActionsDictionary kvp in ActionsDictionaries)
        {
            EventToAction wrapper = new EventToAction();
            wrapper.WrapEvent(kvp.Event);

            _actionDictionary.Add(kvp.ObserverEnum, wrapper.InvokeEvent);
        }
    }

    protected virtual void OnEnable() => Observer.Instance.AddObserver(this);
    protected virtual void OnDisable() => Observer.Instance.RemoveObserver(this);

    public void PerformAction(EnumsActions enumAction)
    {
        if (_actionDictionary.ContainsKey(enumAction)) _actionDictionary[enumAction].Invoke();
    }

    public void NotObs(EnumsActions enumAction) => Observer.Instance.NotifyObservers(enumAction);
}
