using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Subject
{
    [Header("Settings")]
    public float Speed;

    [Header("other")]
    [SerializeField] Camera mainCamera;

    [Header("effects")]
    [SerializeField] GameObject SlashEffectPrefab;
    [SerializeField] Transform EffectsParent;

    //local
    Inputs _input;

    Rigidbody2D _rb;

    Vector2 _inputDirection;

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();

        _input = new Inputs();
        _input.Main.Enable();

        _input.Main.Escape.performed += ctx => OnEscape();
        _input.Main.Attack.performed += ctx => OnLeftMouseButton();
        _input.Main.Skill.performed += ctx => OnRightMouseButton();
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Speed * Time.deltaTime);
    }

    //actions
    void OnEscape()
    {
        Debug.Log("asd");
    }

    void OnLeftMouseButton()
    {
        Vector2 direction = (Mouse.current.position.ReadValue() - (Vector2)transform.position).normalized;

        Instantiate(SlashEffectPrefab, direction * 1.5f + (Vector2)transform.position, Quaternion.identity, EffectsParent);



    }

    void OnRightMouseButton()
    {
        

    }


}
