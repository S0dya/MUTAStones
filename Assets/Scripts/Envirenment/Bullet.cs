using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] ParticleSystem Ps;
    public float BulletSpeed = 1;
    
    [SerializeField] Rigidbody2D Rb;

    void Awake()
    {
        var mainModule = Ps.main;
        mainModule.startRotation = transform.eulerAngles.z * Mathf.Deg2Rad;

        //float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        Ps.Play();
    }

    void Start() => Rb.AddForce(transform.up * BulletSpeed, ForceMode2D.Force);

    void OnTriggerEnter2D(Collider2D Collision) => Destroy(gameObject);
}
