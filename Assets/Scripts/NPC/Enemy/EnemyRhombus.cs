using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRhombus : Enemy
{
    [Header("parameters")]
    public float[] SpawnBulletsDelay = new float[2];
    public float SpawnBulletOffset = 1;

    [SerializeField] GameObject BulletPrefab;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(ShootCor());
    }

    IEnumerator ShootCor()
    {
        while (true)
        {
            Shoot(-90); Shoot(90);

            yield return new WaitForSeconds(Random.Range(SpawnBulletsDelay[0], SpawnBulletsDelay[1]));
        }
    }
    void Shoot(float rotation) => GameManager.Instance.Shoot(BulletPrefab, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + rotation));
}
