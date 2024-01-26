using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Subject
{
    [Header("Settings")]
    public float Speed;

    [Header("other")]
    [SerializeField] Camera mainCamera;

    //local
    Rigidbody2D _rb;

    Vector2 _inputDirection;

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition), Speed * Time.deltaTime);
    }
}
