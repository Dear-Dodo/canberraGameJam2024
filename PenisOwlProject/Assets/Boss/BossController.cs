using System;
using Boss.States;
using Boss.States.Weave;
using UnityEngine;
using UnityEngine.Serialization;

namespace Boss
{
    public class BossController : MonoBehaviour
    {
        [SerializeField]
        private WeaveAttackState _WeaveAttackState;
        
        private BossState _CurrentState;

        private void Awake()
        {
            _WeaveAttackState.Boss = this;
        }

        private void Start() => StartBehaviour();
        
        public void StartBehaviour()
        {
            _CurrentState = _WeaveAttackState;
            _WeaveAttackState.StartState();
        }
    }
}