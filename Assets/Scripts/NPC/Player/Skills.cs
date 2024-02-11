using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : Subject
{
    [Header("attack")]
    public float AttackDuration;

    public float AttackDistance;
    [SerializeField] LayerMask EnemiesLayer;

    public float TargetInhaleScale;
    public float InhaleSpeed;

    [Header("slow motion")]
    [Range(0.1f, 0.9f)] public float SlowMoValue;
    public float SlowMoDuration;
    public float SlowMoSpeed;

    [Header("dash")]
    public float DashSpeed;

    [Header("shooting")]
    public float ShootingDelay = 0.5f;
    [SerializeField] GameObject BulletPrefab;

    [Header("shield")]
    [SerializeField] GameObject ShieldObj;

    //local
    Player _player;
    LineRenderer _lr;

    float MaxSpeed
    {
        get { return _player.Speed; }
        set { _player.Speed = value; }
    }
    float MovementSpeed
    {
        get { return _player._curMovementSpeed; }
        set { _player._curMovementSpeed = value; }
    }

    float _targetInhaleScaleOffset;

    //bools
    bool _isAttacking;
    bool _isInhaling;

    //treshold
    Vector2 _attackDirection;
    Transform _enemyHitTransf;

    float _enemyTransformScaleX;

    //cors
    Coroutine _attackDurationCor;
    Coroutine _attackCor;
    Coroutine _inhaleCor;

    Coroutine _slowMoCor;
    Coroutine _dashCor;
    Coroutine _shootingCor;

    protected override void Awake()
    {
        base.Awake();

        _player = GetComponent<Player>();
        _lr = GetComponent<LineRenderer>();

        AddAction(EnumsActions.AttackUsed, OnAttack);

        AddAction(EnumsActions.SlowMo, OnSlowMo);
        AddAction(EnumsActions.Dash, OnDash);
        AddAction(EnumsActions.Freeze, OnFreeze);
        AddAction(EnumsActions.AvoidEnemies, OnAvoidEnemies);
        AddAction(EnumsActions.Shield, OnShield);
        AddAction(EnumsActions.ShieldBroke, OnShieldBroke);
        AddAction(EnumsActions.Shooting, OnShooting);

        AddAction(EnumsActions.Escape, OnEscape);
    }

    void Start()
    {
        _targetInhaleScaleOffset = TargetInhaleScale * 0.9f;

        ClearLr();
    }

    //actions
    void OnAttack()
    {
        _attackDurationCor = GameManager.Instance.RestartCor(_attackDurationCor, AttackDurationCor());

        _attackCor = GameManager.Instance.RestartCor(_attackCor, AttackCor());
    }

    //skills actions
    void OnSlowMo() => _slowMoCor = GameManager.Instance.RestartCor(_slowMoCor, SlowMoCor());
    void OnDash() => _dashCor = GameManager.Instance.RestartCor(_dashCor, DashCor());
    void OnFreeze() => LevelManager.Instance.FreezeEnemies();
    void OnAvoidEnemies() => LevelManager.Instance.ChangeEnemiesDirections();
    void OnShield()
    {
        NotObs(EnumsActions.ShieldActivate);

        ToggleShield(true);
    }
    void OnShieldBroke()
    {
        ToggleShield(false);

        NotObs(EnumsActions.ShieldDeactivate);
    }
    void OnShooting() => _shootingCor = GameManager.Instance.RestartCor(_shootingCor, ShootingCor());

    void OnEscape()
    {
        if (_slowMoCor != null) StopCoroutine(_slowMoCor);
    }

    //cors
    IEnumerator AttackDurationCor()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(AttackDuration);
        _isAttacking = false;

        if (!_isInhaling) ClearLr();
    }
    IEnumerator AttackCor()
    {
        SetLr();

        while (_isAttacking)
        {
            _attackDirection = (_player.GetWorldPoint() - (Vector2)transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _attackDirection, AttackDistance, EnemiesLayer);

            if (hit.collider != null)
            {
                _enemyHitTransf = hit.collider.transform;

                _inhaleCor = GameManager.Instance.RestartCor(_inhaleCor, InhaleCor());

                break;
            }

            SetLrPos(transform.position, (Vector2)transform.position + _attackDirection);

            yield return null;
        }
    }
    IEnumerator InhaleCor()
    {
        _isInhaling = true;
        
        Enemy enemy = _enemyHitTransf.GetComponent<Enemy>();
        _enemyTransformScaleX = 1;

        while (_enemyHitTransf != null && _enemyTransformScaleX > TargetInhaleScale)
        {
            _enemyTransformScaleX = enemy.AdditionalScaleSpeed = Mathf.Lerp(_enemyTransformScaleX, _targetInhaleScaleOffset, InhaleSpeed * Time.deltaTime);
            _enemyHitTransf.localScale = new Vector2(_enemyTransformScaleX, _enemyTransformScaleX);

            SetLrPos(transform.position, _enemyHitTransf.position);

            yield return null;
        }

        if (enemy != null) enemy.Killed();
        ClearLr();

        _isInhaling = false;
    }

    //skills cors
    IEnumerator DashCor()
    {
        while (IsValBigger(DashSpeed, MovementSpeed, 0.1f))
        {
            MovementSpeed = Lerp(MovementSpeed, DashSpeed, 1);

            yield return null;
        }

        MovementSpeed = DashSpeed;
        yield return new WaitForSeconds(0.1f);

        while (IsValBigger(MovementSpeed, MaxSpeed, -0.1f))
        {
            MovementSpeed = Lerp(MovementSpeed, MaxSpeed, 1);

            yield return null;
        }
        MovementSpeed = MaxSpeed;
    }

    IEnumerator SlowMoCor()
    {
        while (IsValBigger(Time.timeScale, SlowMoValue, 0.05f))
        {
            Time.timeScale = Lerp(Time.timeScale, SlowMoValue, SlowMoSpeed);

            yield return null;
        }
        Time.timeScale = SlowMoValue;
        yield return new WaitForSeconds(SlowMoDuration);

        while (IsValBigger(1, Time.timeScale, 0.05f))
        {
            Time.timeScale = Lerp(Time.timeScale, 1, SlowMoSpeed);

            yield return null;
        }
        Time.timeScale = 1;
    }

    IEnumerator ShootingCor()
    {
        for (int i = 0; i < 5; i++)
        {
            Shoot(-90); Shoot(90);

            yield return new WaitForSeconds(ShootingDelay);
        }
    }
    void Shoot(float rotation)
    {
        Vector2 directionOfMovement = (_player._curClampedMousePos - (Vector2)transform.position).normalized;
        GameManager.Instance.Shoot(BulletPrefab, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(directionOfMovement.y, directionOfMovement.x) * Mathf.Rad2Deg + rotation));
    }

    //other methods
    void SetLrPos(Vector2 startPos, Vector2 EndPos)
    { //for (int i = 0; i < poss.Length; i++) _lr.SetPosition(i, poss[i]);
        _lr.SetPosition(0, startPos);
        _lr.SetPosition(1, EndPos);
    }
    void ClearLr() => _lr.positionCount = 0;
    void SetLr() => _lr.positionCount = 2;

    bool IsValBigger(float firstVal, float secondVal, float offset)
    {
        return firstVal > secondVal + offset;
    }

    float Lerp(float val, float target, float speed)
    {
         return Mathf.Lerp(val, target, speed * Time.deltaTime);
    }

    void StopCor(Coroutine cor)
    {
        if (cor != null) StopCoroutine(cor);
    }

    void ToggleShield(bool toggle) => ShieldObj.SetActive(toggle);

    Coroutine RestartCor(Coroutine cor, IEnumerator to)
    {
        if (cor != null) StopCoroutine(cor);
        return StartCoroutine(to);
    }
}
