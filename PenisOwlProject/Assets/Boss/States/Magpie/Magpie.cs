using System;
using UnityEngine;

namespace Boss.States.Magpie
{
    public class Magpie : MonoBehaviour
    {
        private const float TargetReachedDistanceSqr = 0.01f * 0.01f;
            
        public float Speed = 2;
        public float Damage;

        [HideInInspector] public Vector3 Target;

        public event Action<Magpie> ArrivedAtTarget;

        private Transform _Transform;

        private void Awake()
        {
            _Transform = transform;
        }
        
        private void FixedUpdate()
        {
            var maxDelta = Speed * Time.fixedDeltaTime;
            Vector3 currentPosition = _Transform.position;
            transform.LookAt(Target, Vector3.forward);
            Vector3 position = Vector3.MoveTowards(currentPosition, Target, maxDelta);
            _Transform.position = position;
            
            if ((Target - position).sqrMagnitude < TargetReachedDistanceSqr)
                ArrivedAtTarget?.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player.Player player))
            {
                player.Damage(Damage);
            }
        }
    }
}