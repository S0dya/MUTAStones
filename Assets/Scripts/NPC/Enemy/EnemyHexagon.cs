using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHexagon : Enemy
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15)
        {
            FreezeTrigger freezeTrigger = collision.gameObject.GetComponent<FreezeTrigger>();
            freezeTrigger.AddTransf(transform);

            return;
        }

        base.OnTriggerEnter2D(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15)
        {
            FreezeTrigger freezeTrigger = collision.gameObject.GetComponent<FreezeTrigger>();
            freezeTrigger.RemoveTransf(transform);
        }
    }
}
