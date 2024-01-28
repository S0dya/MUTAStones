using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Subject
{
    [Header("Settings")]
    public float Speed;

    [Header("effects")]
    [SerializeField] GameObject SlashEffectPrefab;
    [SerializeField] Transform EffectsParent;

    [Header("other")]
    [SerializeField] Color StartingColor;
    [SerializeField] TrailRenderer Trail;

    //local
    Camera MainCamera;
    Inputs _input;
    Rigidbody2D _rb;
    SpriteRenderer _sr;

    Material _trailMat;


    Vector2 _curMousePos;
    Vector2 _curClampedMousePos;

    [HideInInspector] public float _curMovementSpeed;


    //abilities
    HashSet<EnumsActions> _skillsSet = new HashSet<EnumsActions>();

    //bools
    bool _canAttack = true;
    bool _canUseSkill = true;


    protected override void Awake()
    {
        base.Awake();

        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _trailMat = Trail.material;

        AssignActions();
        AssignInput();
    }
    void AssignActions()
    {
        AddAction(EnumsActions.AttackUsed, CantUseAttack);
        AddAction(EnumsActions.SkillUsed, CantUseSkill);

        AddAction(EnumsActions.AttackRestored, CanUseAttack);
        AddAction(EnumsActions.SkillRestored, CanUseSkill);
    }
    void AssignInput()
    {
        _input = new Inputs();
        _input.Main.Enable();

        _input.Main.Attack.performed += ctx => OnLeftMouseButton();
        _input.Main.Skill.performed += ctx => OnRightMouseButton();
        _input.Main.Escape.performed += ctx => OnEscape();
    }

    protected override void Start()
    {
        base.Start();

        _curMovementSpeed = Speed;

        _skillsSet.Add(EnumsActions.SlowMo);
        _skillsSet.Add(EnumsActions.Shield);
    }

    void Update()
    {
        _curMousePos = Mouse.current.position.ReadValue();
        _curClampedMousePos = new Vector2(Mathf.Clamp(_curMousePos.x, 0f, Screen.width),Mathf.Clamp(_curMousePos.y, 0f, Screen.height));
        transform.position = Vector2.Lerp(transform.position, GetWorldPoint(), _curMovementSpeed * Time.deltaTime);
    }

    //input
    void OnLeftMouseButton()
    {
        if (!_canAttack)
        {
            Observer.Instance.NotifyObservers(EnumsActions.AttackUsedFailed);
            return;
        }
        
        Observer.Instance.NotifyObservers(EnumsActions.AttackUsed);

        Vector2 direction = (GetWorldPoint() - (Vector2)transform.position).normalized;
        Instantiate(SlashEffectPrefab, direction * 6 + (Vector2)transform.position, Quaternion.identity, EffectsParent);
    }

    void OnRightMouseButton()
    {
        if (!_canUseSkill)
        {
            Observer.Instance.NotifyObservers(EnumsActions.SkillUsedFailed);
            return;
        }

        Observer.Instance.NotifyObservers(EnumsActions.SkillUsed);

        foreach (var skill in _skillsSet) Observer.Instance.NotifyObservers(skill);
    }
    
    void OnEscape()
    {
        Die();
    }

    //actions
    void CantUseAttack() => _canAttack = false;
    void CantUseSkill() => _canUseSkill = false;
    void CanUseAttack() => _canAttack = true;
    void CanUseSkill() => _canUseSkill = true;

    //other methods
    Vector2 GetWorldPoint()
    {
        return MainCamera.ScreenToWorldPoint(_curClampedMousePos);
    }

    public void Mutate(SO_Mutation mutation)
    {
        if (_skillsSet.Contains(mutation.Skill)) return;

        _skillsSet.Add(mutation.Skill);
        _trailMat.color = _sr.color = Color.Lerp(_sr.color, mutation.Color, 0.5f);
    }

    //trigger
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10) Observer.Instance.NotifyObservers(EnumsActions.KilledByEnemy);
        else Observer.Instance.NotifyObservers(EnumsActions.KilledByBounds);

        Die();
    }

    public void Die()
    {
        Debug.Log("die");
        //Observer.Instance.NotifyObservers(EnumsActions.Gameover);
    }
}
