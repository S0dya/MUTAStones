using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonobehaviour<LevelManager>
{
    [Header("settings")]
    public float[] SpawnOffset = new float[2];

    [SerializeField] GameObject[] enemiesPrefabs;
    [SerializeField] Transform enemiesParent;

    //local
    Transform playerTransf;
    Player player;

    protected override void Awake()
    {
        base.Awake();

        playerTransf = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransf.GetComponent<Player>();
    }

    void Start()
    {
        StartCoroutine(WavesCor());
    }

    IEnumerator WavesCor()
    {
        while (true)
        {
            Transform enemyTrasnf = InstantiateEnemy(enemiesPrefabs[Random.Range(0, 1)], GetRandomOffsetPos(SpawnOffset[0], SpawnOffset[1], 1));
            Enemy enemyScript = enemyTrasnf.GetComponent<Enemy>();
            enemyScript.MovemenetDirection = (playerTransf.position - enemyScript.transform.position).normalized;

            yield return new WaitForSeconds(1);
        }
    }


    //other methods
    Transform InstantiateEnemy(GameObject prefab, Vector2 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity, enemiesParent).transform;
    }
    
    public Vector2 GetRandomOffsetPos(float x, float y, float offset)
    {
        return Random.Range(0, 2) == 1 ? GetRandomOffsetPosByX(x, y, offset) : GetRandomOffsetPosByY(x, y, offset);
    }
    public Vector2 GetRandomOffsetPosByX(float x, float y, float offset)
    {
        return Random.Range(0, 2) == 1 ? new Vector2(Random.Range(x, x + offset), Random.Range(-y, y))
            : new Vector2(Random.Range(-x - offset, -x), Random.Range(-y, y));
    }
    public Vector2 GetRandomOffsetPosByY(float x, float y, float offset)
    {
        return Random.Range(0, 2) == 1 ? new Vector2(Random.Range(-x, x), Random.Range(y, y + offset)) 
            : new Vector2(Random.Range(-x, x), Random.Range(-y - offset, -y));
    }

    public void FreezeEnemies() => LoopThroughtEnemiesTransforms(enemiesParent, FreezeEnemy);
    void FreezeEnemy(Enemy enemy) => enemy.Freeze();

    public void ChangeEnemiesDirections() => LoopThroughtEnemiesTransforms(enemiesParent, ChangeEnemyDirection);
    void ChangeEnemyDirection(Enemy enemy) => enemy.ChangeDirection();

    void LoopThroughtEnemiesTransforms(Transform parent, System.Action<Enemy> action)
    {
        foreach (Transform transf in parent) action.Invoke(transf.GetComponent<Enemy>());
    }
}
