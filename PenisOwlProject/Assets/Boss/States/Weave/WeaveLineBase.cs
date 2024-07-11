using System;
using UnityEngine;

namespace Boss.States.Weave
{
    [RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
    public class WeaveLineBase : MonoBehaviour
    {
        [SerializeField]
        protected LineWobbler Wobbler;

        public float Damage;

        protected virtual void Awake()
        {
            Wobbler.Initialise(GetComponent<LineRenderer>(), GetComponent<EdgeCollider2D>());
        }

        protected virtual void Update()
        {
            Wobbler.UpdateLine();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Wobbler.Lifetime > Wobbler.FadeInTime && collision.gameObject.TryGetComponent(out Player.Player player))
            {
                player.Damage(Damage);
            }
        }
    }
}