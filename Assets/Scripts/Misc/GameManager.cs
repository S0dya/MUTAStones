using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : SingletonMonobehaviour<GameManager>
{

    public GameData GameData;

    //local
    Player player;

    protected override void Awake()
    {
        base.Awake();

        GameData = new GameData();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    public Coroutine RestartCor(Coroutine cor, IEnumerator enumerator)
    {
        if (cor != null) StopCoroutine(cor);
        return StartCoroutine(enumerator);
    }

    public Vector2 GetPlayerPos()
    {
        return player.transform.position;
    }
}
