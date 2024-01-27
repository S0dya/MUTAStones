using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Subject
{
    [Header("Settings")]
    public float Speed;
    public float DashSpeed;

    [Header("other")]
    [SerializeField] Camera mainCamera;

    [Header("effects")]
    [SerializeField] GameObject SlashEffectPrefab;
    [SerializeField] Transform EffectsParent;

    //local
    Inputs _input;
    Rigidbody2D _rb;

    float _curMovementSpeed;

    //cors
    Coroutine _dashCor;

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

    protected override void Start()
    {
        base.Start();

        _curMovementSpeed = Speed;
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), _curMovementSpeed * Time.deltaTime);

        Debug.Log(_curMovementSpeed);
    }

    //actions
    void OnEscape()
    {
        Debug.Log("asd");
    }

    void OnLeftMouseButton()
    {
        Vector2 direction = (mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position).normalized;

        if (_dashCor != null) StopCoroutine(_dashCor);
        _dashCor = StartCoroutine(DashCor());
        Instantiate(SlashEffectPrefab, direction * 6 + (Vector2)transform.position, Quaternion.identity, EffectsParent);



    }

    void OnRightMouseButton()
    {
        

    }

    //cors
    IEnumerator DashCor()
    {
        while (_curMovementSpeed < DashSpeed - 0.1f)
        {
            _curMovementSpeed = Mathf.Lerp(_curMovementSpeed, DashSpeed, Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        while (_curMovementSpeed > Speed + 0.1f)
        {
            _curMovementSpeed = Mathf.Lerp(_curMovementSpeed, Speed, Time.deltaTime);

            yield return null;
        }
    }
}
