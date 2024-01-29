using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Subject
{
    [Header("Settings")]
    public float MinSize;
    public float MaxSize;

    public float SpeedIn;
    public float SpeedOut;

    //local
    CinemachineVirtualCamera Vcam;

    float _curSize;


    protected override void Awake()
    {
        base.Awake();

        Vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        _curSize = MaxSize;

        StartCoroutine(RhytmCameraCor());
    }

    IEnumerator RhytmCameraCor()
    {
        while (true)
        {

            while (_curSize > MinSize + 0.1f)
            {
                _curSize = Mathf.Lerp(_curSize, MinSize, SpeedIn * Time.deltaTime);
                Vcam.m_Lens.OrthographicSize = _curSize;
             
                yield return null;
            }

            while (_curSize < MaxSize - 0.1f)
            {
                _curSize = Mathf.Lerp(_curSize, MaxSize, SpeedOut * Time.deltaTime);
                Vcam.m_Lens.OrthographicSize = _curSize;

                yield return null;
            }

            yield return null;
        }
    }
}
