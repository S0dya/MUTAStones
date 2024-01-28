using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed = 1;
    
    [SerializeField] Rigidbody2D Rb;

    void Start() => Rb.AddForce(transform.up * BulletSpeed, ForceMode2D.Force);

    void OnTriggerEnter2D(Collider2D Collision) => Destroy(gameObject);
}
