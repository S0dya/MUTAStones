using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    public float MovementSpeed;
    public SO_Mutation Mutation;

    //local
    Rigidbody2D _rb;

    [HideInInspector] public Vector2 _movementDirection;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        _rb.velocity = _movementDirection * MovementSpeed;
    }

    //trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
