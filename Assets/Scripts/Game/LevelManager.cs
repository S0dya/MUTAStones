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
            Enemy enemyScript = Instantiate(enemiesPrefabs[Random.Range(0, 1)], GetRandomOffsetPos(SpawnOffset[0], SpawnOffset[1], 1), Quaternion.identity, enemiesParent).GetComponent<Enemy>();
            enemyScript._movementDirection = playerTransf.position - enemyScript.transform.position;

            yield return new WaitForSeconds(1);
        }
    }

    public Vector2 GetRandomOffsetPos(float x, float y, float offset)
    {
        Vector2 result = new Vector2();

        switch (Random.Range(0, 4))
        {
            case 0: result = new Vector2(Random.Range(-x, x), Random.Range(y, y + offset)); break;
            case 1: result = new Vector2(Random.Range(x, x + offset), Random.Range(-y, y)); break;
            case 2: result = new Vector2(Random.Range(-x, x), Random.Range(-y - offset, -y)); break;
            case 3: result = new Vector2(Random.Range(-x - offset, -x), Random.Range(-y, y)); break;
            default: break;
        }

        return result;
    }
}
