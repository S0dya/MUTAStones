using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Subject
{
    [Header("Settings")]
    public float Speed;
    public float MaxFreezeDistance;

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
    [HideInInspector] public SpriteRenderer _sr;

    Material _trailMat;

    Vector2 _curMousePos;
    [HideInInspector] public Vector2 _curClampedMousePos;

    [HideInInspector] public float _curMovementSpeed;
    [HideInInspector] public float _freezeVal;

    //abilities
    HashSet<EnumsActions> _skillsSet = new HashSet<EnumsActions>();

    //bools
    bool _canAttack = true;
    bool _canUseSkill = true;

    bool _isShieldOn;

    protected override void Awake()
    {
        base.Awake();

        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _trailMat = Trail.material;

        AssignActions();
    }
    void AssignActions()
    {
        AddAction(EnumsActions.AttackUsed, CantUseAttack);
        AddAction(EnumsActions.SkillUsed, CantUseSkill);

        AddAction(EnumsActions.AttackRestored, CanUseAttack);
        AddAction(EnumsActions.SkillRestored, CanUseSkill);

        AddAction(EnumsActions.ShieldActivate, OnShieldActivate);
        AddAction(EnumsActions.ShieldDeactivate, OnShieldDeactivate);

        AddAction(EnumsActions.IncreaseScale, OnIncreaseScale);
        AddAction(EnumsActions.DecreaseScale, OnDecreaseScale);
        AddAction(EnumsActions.ResetMutation, OnResetMutation);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _input = new Inputs();
        _input.Main.Enable();

        _input.Main.Attack.performed += ctx => OnLeftMouseButton();
        _input.Main.Skill.performed += ctx => OnRightMouseButton();
        _input.Main.Escape.performed += ctx => OnEscape();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _input.Main.Attack.performed -= ctx => OnLeftMouseButton();
        _input.Main.Skill.performed -= ctx => OnRightMouseButton();
        _input.Main.Escape.performed -= ctx => OnEscape();

        _input.Main.Disable();
    }

    void Start()
    {
        _curMovementSpeed = Speed;
        _freezeVal = 1;
    }

    void Update()
    {
        _curMousePos = Mouse.current.position.ReadValue();
        _curClampedMousePos = new Vector2(Mathf.Clamp(_curMousePos.x, 0f, Screen.width),Mathf.Clamp(_curMousePos.y, 0f, Screen.height));

        transform.position = Vector2.Lerp(transform.position, GetWorldPoint(), _curMovementSpeed * _freezeVal * Time.deltaTime);
    }

    //input
    void OnLeftMouseButton()
    {
        if (!_canAttack || Time.timeScale == 0)
        {
            NotObs(EnumsActions.AttackUsedFailed);
            return;
        }
        
        NotObs(EnumsActions.AttackUsed);
    }

    void OnRightMouseButton()
    {
        if (!_canUseSkill || Time.timeScale == 0)
        {
            NotObs(EnumsActions.SkillUsedFailed);
            return;
        }

        NotObs(EnumsActions.SkillUsed);

        foreach (var skill in _skillsSet) NotObs(skill);
        NotObs(EnumsActions.ResetMutation);
    }
    
    void OnEscape()
    {
        NotObs(EnumsActions.Escape);
    }

    //actions
    void CantUseAttack() => _canAttack = false;
    void CantUseSkill() => _canUseSkill = false;
    void CanUseAttack() => _canAttack = true;
    void CanUseSkill() => _canUseSkill = true;

    void OnShieldActivate() => _isShieldOn = true;
    void OnShieldDeactivate() => StartCoroutine(ToggleShieldCor());

    void OnIncreaseScale()
    {
        transform.localScale += new Vector3(0.1f, 0.1f, 0);

        GameManager.Instance.GameData.MaxSize = Mathf.Max(GameManager.Instance.GameData.MaxSize, transform.localScale.x);
    }
    void OnDecreaseScale() => transform.localScale = Vector3.one;

    void OnResetMutation()
    {
        NotObs(EnumsActions.DecreaseScale);

        _trailMat.color = _sr.color = StartingColor;
        _skillsSet = new HashSet<EnumsActions>();
    }

    //outside methods
    public void SetFreezeVal(float distance) => _freezeVal = Mathf.Clamp01(distance / MaxFreezeDistance);
    public void ResetFreezeVal() => _freezeVal = 1;


    public void Mutate(SO_Mutation mutation)
    {
        _trailMat.color = _sr.color = Color.Lerp(_sr.color, mutation.Color, 0.5f);
        if (transform.localScale.x < 5) NotObs(EnumsActions.IncreaseScale);

        GameManager.Instance.GameData.OnMutated();
        if (!_skillsSet.Contains(mutation.Skill)) GameManager.Instance.GameData.OnNewSkill();

        _skillsSet.Add(mutation.Skill);
    }
    
    //other methods
    public Vector2 GetWorldPoint()
    {
        return MainCamera.ScreenToWorldPoint(_curClampedMousePos);
    }

    IEnumerator ToggleShieldCor()
    {
        yield return null;

        _isShieldOn = false;
    }

    //trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isShieldOn) return;

        switch (collision.gameObject.layer)
        {
            case 10: NotObs(EnumsActions.KilledByEnemy); break;
            case 7: NotObs(EnumsActions.KilledByBounds); break;
            case 20: NotObs(EnumsActions.KilledByBullet); break;
            default: return;
        }

        Die();
    }

    public void Die()
    {
        NotObs(EnumsActions.Gameover);
    }
}
