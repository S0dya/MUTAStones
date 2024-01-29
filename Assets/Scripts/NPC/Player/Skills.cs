using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : Subject
{
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
    Player player;

    float MaxSpeed
    {
        get { return player.Speed; }
        set { player.Speed = value; }
    }
    float MovementSpeed
    {
        get { return player._curMovementSpeed; }
        set { player._curMovementSpeed = value; }
    }


    //cors
    Coroutine _slowMoCor;
    Coroutine _dashCor;
    Coroutine _shootingCor;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();

        AddAction(EnumsActions.SlowMo, OnSlowMo);
        AddAction(EnumsActions.Dash, OnDash);
        AddAction(EnumsActions.Freeze, OnFreeze);
        AddAction(EnumsActions.AvoidEnemies, OnAvoidEnemies);
        AddAction(EnumsActions.Shield, OnShield);
        AddAction(EnumsActions.ShieldBroke, OnShieldBroke);
        AddAction(EnumsActions.Shooting, OnShooting);

        AddAction(EnumsActions.Escape, OnEscape);
    }

    //skills actions
    void OnSlowMo() => _slowMoCor = GameManager.Instance.RestartCor(_slowMoCor, SlowMoCor());
    void OnDash() => _dashCor = GameManager.Instance.RestartCor(_dashCor, DashCor());
    void OnFreeze() => LevelManager.Instance.FreezeEnemies();
    void OnAvoidEnemies() => LevelManager.Instance.ChangeEnemiesDirections();
    void OnShield()
    {
        Observer.Instance.NotifyObservers(EnumsActions.ShieldActivate);
        ToggleShield(true);
    }
    void OnShieldBroke()
    {
        ToggleShield(false);
        Observer.Instance.NotifyObservers(EnumsActions.ShieldDeactivate);
    }
    void OnShooting() => _shootingCor = GameManager.Instance.RestartCor(_shootingCor, ShootingCor());

    void OnEscape()
    {
        if (_slowMoCor != null) StopCoroutine(_slowMoCor);
    }

    //cors
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
        Vector2 directionOfMovement = (player._curClampedMousePos - (Vector2)transform.position).normalized;
        GameManager.Instance.Shoot(BulletPrefab, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(directionOfMovement.y, directionOfMovement.x) * Mathf.Rad2Deg + rotation));
    }

    //other methods
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
