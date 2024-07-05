using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{

    public class Player : MonoBehaviour
    {
        public float MoveSpeed;
        public float DashDistance;
        public float DashTime;
        public float DashCooldown;

        private float _dashCooldown;

        private InputAction _moveAction;
        private InputAction _dashAction;

        // Start is called before the first frame update
        void Start()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _dashAction = InputSystem.actions.FindAction("Dash");
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 moveValue = _moveAction.ReadValue<Vector2>();
            bool dashValue = _dashAction.WasPressedThisFrame();
            transform.position += new Vector3(moveValue.x, moveValue.y) * MoveSpeed;
            if (dashValue && _dashCooldown == 0)
            {
                StartCoroutine(Dash(moveValue));
            }

            if (_dashCooldown > 0)
            {
                _dashCooldown -= Time.deltaTime;
            }
            if (_dashCooldown < 0)
            {
                _dashCooldown = 0;
            }
        }

        IEnumerator Dash(Vector2 moveValue)
        {
            Vector3 dashStart = transform.position;
            Vector3 dashTarget = transform.position + new Vector3(moveValue.x, moveValue.y) * DashDistance;
            float timer = 0;
            _dashCooldown = DashCooldown;
            while (timer < DashTime)
            {
                transform.position = Vector3.Lerp(dashStart,dashTarget,timer / DashTime);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
