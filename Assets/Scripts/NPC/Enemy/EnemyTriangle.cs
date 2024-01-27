using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriangle : Enemy
{
    

    void Start()
    {
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(_movementDirection.y, _movementDirection.x) * Mathf.Rad2Deg - 90f, Vector3.forward);
    }
}
