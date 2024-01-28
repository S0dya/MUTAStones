using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    public float MovementSpeed;
    public SO_Mutation Mutation;

    public float UnFreezeSpeed;
    public float ChangeDirectionSpeed;

    //local
    Rigidbody2D _rb;

    Vector2 _movementDirection;

    [HideInInspector] public bool _isChangingDirection;

    public Vector2 MovemenetDirection
    {
        get { return _movementDirection; }
        set 
        {
            if (_movementDirection != value)
            {
                _movementDirection = value;

                SetMovementDirection();
            }
        }
    }

    float _curMovementSpeed;

    //cors
    Coroutine _freezeCor;
    Coroutine _changeDirectionCor;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        _curMovementSpeed = MovementSpeed;
    }

    protected virtual void Update()
    {
        _rb.velocity = MovemenetDirection * _curMovementSpeed;
    }

    void SetMovementDirection()
    {
        if (this != null) transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(MovemenetDirection.y, MovemenetDirection.x) * Mathf.Rad2Deg - 90f, Vector3.forward);
    }

    //outside methods
    public void Freeze() => _freezeCor = GameManager.Instance.RestartCor(_freezeCor, FreezeCor());
    public void ChangeDirection()
    {
        _isChangingDirection = true;

        _changeDirectionCor = GameManager.Instance.RestartCor(_changeDirectionCor, ChangeDirectionCor());
    }

    //cors
    IEnumerator FreezeCor()
    {
        _curMovementSpeed = 0;
        yield return new WaitForSeconds(3);

        while (_curMovementSpeed < MovementSpeed -0.1f)
        {
            _curMovementSpeed = Mathf.Lerp(_curMovementSpeed, MovementSpeed, UnFreezeSpeed * Time.deltaTime);
         
            yield return null;
        }

        _curMovementSpeed = MovementSpeed;
    }

    IEnumerator ChangeDirectionCor()
    {
        Vector2 newDirection = (GameManager.Instance.GetPlayerPos() - (Vector2)transform.position).normalized * -1;

        while (Vector2.Dot(MovemenetDirection, newDirection) < 0.8f)
        {
            MovemenetDirection = Vector2.Lerp(MovemenetDirection, newDirection, ChangeDirectionSpeed * Time.deltaTime).normalized;

            yield return null;
        }
    }

    //trigger
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 14:
                Observer.Instance.NotifyObservers(EnumsActions.EnemyKilled);

                Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                player.Mutate(Mutation);
                break;
            case 16: Observer.Instance.NotifyObservers(EnumsActions.ShieldBroke); break;
            case 13: return;
            default: break;
        }

        Destroy(gameObject);
    }
}
