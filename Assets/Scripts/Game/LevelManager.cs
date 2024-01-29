using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonobehaviour<LevelManager>
{
    [Header("settings")]
    public float[] SpawnOffset = new float[2];

    [Header("other")]
    [SerializeField] Transform enemiesParent;

    [Header("enemies prefabs")]
    [SerializeField] EnemyClass[] EnemiesClasses;

    //local
    Transform playerTransf;
    Player player;

    //waves
    int _amountOfDifferentEnemies = 2;
    float _spawnDelay = 0.8f;

    //threshold 
    EnemyClass _curEnemyClass;
    Vector2 _curEnemyPos;
    Transform _curEnemyTrasnf;
    Enemy _curEnemy;

    int _maxDifferentEnemiesAmount;

    protected override void Awake()
    {
        base.Awake();

        playerTransf = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransf.GetComponent<Player>();
    }

    void Start()
    {
        Shuffle(EnemiesClasses);
        _maxDifferentEnemiesAmount = EnemiesClasses.Length;

        StartCoroutine(WavesCor());
    }

    IEnumerator WavesCor()
    {
        while (true)
        {
            _curEnemyClass = EnemiesClasses[GetRandomVal(0, _amountOfDifferentEnemies)];

            switch (_curEnemyClass.SpawnEnum)
            {
                case EnumsEnemySpawn.SpawnsByXAndY: _curEnemyPos = GetRandomOffsetPos(); break;
                case EnumsEnemySpawn.SpawnsByX: _curEnemyPos = GetRandomOffsetPosByX(); break;
                case EnumsEnemySpawn.SpawnsByY: _curEnemyPos = GetRandomOffsetPosByY(); break;
                default: break;
            }

            _curEnemyTrasnf = InstantiateEnemy(_curEnemyClass.EnemyPrefab, _curEnemyPos);
            _curEnemy = _curEnemyTrasnf.GetComponent<Enemy>();
            _curEnemy.MovemenetDirection = (playerTransf.position - _curEnemyTrasnf.position).normalized;

            yield return new WaitForSeconds(_spawnDelay);
        }
    }


    //outside methods
    public void IncreaseEnemiesAmount()
    {
        if (_amountOfDifferentEnemies == _maxDifferentEnemiesAmount) return;

        _amountOfDifferentEnemies++;
    }
    public void DecreaseSpawnDelay() => _spawnDelay *= 0.9f;


    //other methods
    Transform InstantiateEnemy(GameObject prefab, Vector2 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity, enemiesParent).transform;
    }
    
    public Vector2 GetRandomOffsetPos(float x, float y, float offset)
    {
        return GetRandomVal(0, 2) == 1 ? GetRandomOffsetPosByX(x, y, offset) : GetRandomOffsetPosByY(x, y, offset);
    }
    public Vector2 GetRandomOffsetPosByX(float x, float y, float offset)
    {
        return GetRandomVal(0, 2) == 1 ? new Vector2(GetRandomVal(x, x + offset), GetRandomVal(-y, y))
            : new Vector2(GetRandomVal(-x - offset, -x), GetRandomVal(-y, y));
    }
    public Vector2 GetRandomOffsetPosByY(float x, float y, float offset)
    {
        return GetRandomVal(0, 2) == 1 ? new Vector2(GetRandomVal(-x, x), GetRandomVal(y, y + offset)) 
            : new Vector2(GetRandomVal(-x, x), GetRandomVal(-y - offset, -y));
    }

    Vector2 GetRandomOffsetPos()
    {
        return GetRandomOffsetPos(SpawnOffset[0], SpawnOffset[1], 1);
    }
    Vector2 GetRandomOffsetPosByX()
    {
        return GetRandomOffsetPosByX(SpawnOffset[0], SpawnOffset[1], 1);
    }
    Vector2 GetRandomOffsetPosByY()
    {
        return GetRandomOffsetPosByY(SpawnOffset[0], SpawnOffset[1], 1);
    }

    public void FreezeEnemies() => LoopThroughtEnemiesTransforms(enemiesParent, FreezeEnemy);
    void FreezeEnemy(Enemy enemy) => enemy.Freeze();

    public void ChangeEnemiesDirections() => LoopThroughtEnemiesTransforms(enemiesParent, ChangeEnemyDirection);
    void ChangeEnemyDirection(Enemy enemy) => enemy.ChangeDirection();

    void LoopThroughtEnemiesTransforms(Transform parent, System.Action<Enemy> action)
    {
        foreach (Transform transf in parent) action.Invoke(transf.GetComponent<Enemy>());
    }

    float GetRandomVal(float firstVal, float secVal)
    {
        return UnityEngine.Random.Range(firstVal, secVal);
    }
    int GetRandomVal(int firstVal, int secVal)
    {
        return UnityEngine.Random.Range(firstVal, secVal);
    }

    void Shuffle<T>(T[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
}
