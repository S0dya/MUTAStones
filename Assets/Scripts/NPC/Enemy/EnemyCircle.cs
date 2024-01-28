using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircle : Enemy
{
    protected override void Start()
    {
        base.Start();

        StartCoroutine(FollowPlayerCor());

        Invoke("StopFollowing", 3);
    }

    IEnumerator FollowPlayerCor()
    {
        while (!_isChangingDirection)
        {
            MovemenetDirection = ((Vector2)GameManager.Instance.GetPlayerPos() - (Vector2)transform.position).normalized;

            yield return null;
        }
    }

    void StopFollowing() => _isChangingDirection = true;
}
