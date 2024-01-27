using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : Subject
{
    [Header("skills settings")]
    [Range(0.1f, 0.9f)] public float SlowMoValue;
    public float SlowMoDuration;
    public float SlowMoSpeed;

    public float DashSpeed;

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

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();

        AddAction(EnumsActions.SlowMo, OnSlowMo);
        AddAction(EnumsActions.Dash, OnDash);
    }

    //skills
    void OnSlowMo()
    {
        if (_slowMoCor != null) StopCoroutine(_slowMoCor);
        _slowMoCor = StartCoroutine(SlowMoCor());
    }

    void OnDash()
    {
        if (_dashCor != null) StopCoroutine(_dashCor);
        _dashCor = StartCoroutine(DashCor());
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

    //other methods
    bool IsValBigger(float firstVal, float secondVal, float offset)
    {
        return firstVal > secondVal + offset;
    }

    float Lerp(float val, float target, float speed)
    {
         return Mathf.Lerp(val, target, speed * Time.deltaTime);
    }
}
