using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    public GameData GameData;

    protected override void Awake()
    {
        base.Awake();

        GameData = new GameData();
    }


}
