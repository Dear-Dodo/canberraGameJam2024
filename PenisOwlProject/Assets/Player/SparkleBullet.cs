using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;

public class SparkleBullet : MonoBehaviour
{
    public float Velocity;
    public float Damage;
    public float Lifetime;
    [HideInInspector]
    public GameObject Player; //used to maintain relative offset during laser mode
    [HideInInspector]
    public float Offset;
    [HideInInspector]
    public bool LaserMode;

    public List<Sprite> Sprites = new List<Sprite>();

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(Vector2.up * Velocity, ForceMode2D.Impulse);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Sprites[Random.Range(0, Sprites.Count)];

        Destroy(gameObject, Lifetime);
    }

    private void Update()
    {
        if (LaserMode)
        {
            transform.position = new Vector3(Player.transform.position.x + Offset, transform.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BossController boss))
        {
            boss.Damage(Damage);
        }
    }
}
