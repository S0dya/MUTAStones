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
            Transform enemyTrasnf = Instantiate(enemiesPrefabs[Random.Range(0, 1)], GetRandomOffsetPos(SpawnOffset[0], SpawnOffset[1], 1), Quaternion.identity, enemiesParent).transform;
            Enemy enemyScript = enemyTrasnf.GetComponent<Enemy>();
            enemyScript._movementDirection = (playerTransf.position - enemyScript.transform.position).normalized;

            yield return new WaitForSeconds(1);
        }
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
}
