using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyThing : MonoBehaviour
{
    public float Damage;
    public float Speed;
    public AnimationCurve Acceleration;
    public float Duration;

    private float _Lifetime = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _Lifetime += Time.fixedDeltaTime;
        transform.Rotate(new Vector3(0, 0, Speed * Acceleration.Evaluate(_Lifetime / Duration) * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player.Player player))
        {
            player.Damage(Damage);
        }
    }
}
