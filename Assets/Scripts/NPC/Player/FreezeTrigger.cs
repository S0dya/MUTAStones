using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrigger : MonoBehaviour
{
    Player player;

    Coroutine _checkDistanceCor;

    List<Transform> transforms = new List<Transform>();

    //threshold
    float _curNearestDistance;
    Vector2 _curPlayerPos;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Start()
    {
        _curNearestDistance = float.MaxValue;
    }

    //outside methods
    public void AddTransf(Transform transform)
    {
        transforms.Add(transform);

        if (transforms.Count == 1) _checkDistanceCor = GameManager.Instance.RestartCor(_checkDistanceCor, CheckDistanceCor());
    }
    public void RemoveTransf(Transform transform)
    {
        if (transforms.Contains(transform)) transforms.Remove(transform);
    }

    IEnumerator CheckDistanceCor()
    {
        while (transforms.Count != 0)
        {
            _curPlayerPos = transform.position;
            _curNearestDistance = float.MaxValue;

            foreach (var transf in transforms)
            {
                if (transf == null)
                {
                    transforms.Remove(transf);

                    if (transforms.Count <= 0) break;
                }
                else _curNearestDistance = Mathf.Min(_curNearestDistance, Vector2.Distance(transf.position, _curPlayerPos));
            }

            player.SetFreezeVal(_curNearestDistance);

            yield return null;
        }

        player.ResetFreezeVal();
    }
}
