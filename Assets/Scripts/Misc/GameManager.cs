using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : SingletonMonobehaviour<GameManager>
{

    public GameData GameData;

    //local
    Player player;
    Transform EffectsParent;

    protected override void Awake()
    {
        base.Awake();

        GameData = new GameData();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        EffectsParent = GameObject.FindGameObjectWithTag("EffectsParent").transform;
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

    public ParticleSystem Shoot(GameObject bulletPrefab, Vector2 pos, Quaternion rotation)
    {
        return Instantiate(bulletPrefab, pos, rotation, EffectsParent).GetComponent<ParticleSystem>();

    }
}
