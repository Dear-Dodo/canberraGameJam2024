using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleBullet : MonoBehaviour
{
    public float Velocity;
    public float Damage;
    public float Lifetime;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(Vector2.up * Velocity, ForceMode2D.Impulse);

        Destroy(gameObject, Lifetime);
    }
}
