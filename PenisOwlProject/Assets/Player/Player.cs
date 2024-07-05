using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{

    public class Player : MonoBehaviour
    {
        public float MoveSpeed;

        private InputAction _moveAction;

        // Start is called before the first frame update
        void Start()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 moveValue = _moveAction.ReadValue<Vector2>();
            transform.position += new Vector3(moveValue.x, moveValue.y) * MoveSpeed;
        }
    }
}
