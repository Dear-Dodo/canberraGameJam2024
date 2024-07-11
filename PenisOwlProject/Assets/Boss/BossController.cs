using System;
using System.Collections.Generic;
using Boss.States;
using Boss.States.Weave;
using Boss.States.Spinny;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace Boss
{
    public class BossController : MonoBehaviour
    {
        [SerializeField]
        private VisualEffect Pulse;


        [SerializeField]
        private WeaveAttackState _WeaveAttackState;
        [SerializeField]
        private SpinnyThingAttackState _SpinnyThingAttackState;

        private BossState _CurrentState;

        private readonly List<BossState> _States = new();

        private int _StateIndex = 0;

        private void Awake()
        {
            _WeaveAttackState.Boss = this;
            _WeaveAttackState.OnAttackCompletion += NextAttack;
            _States.Add(_WeaveAttackState);

            _SpinnyThingAttackState.Boss = this;
            _SpinnyThingAttackState.OnAttackCompletion += NextAttack;
            _States.Add(_SpinnyThingAttackState);
        }

        private void Start() => StartBehaviour();
        
        public void StartBehaviour()
        {
            _CurrentState = _WeaveAttackState;
            _WeaveAttackState.StartState();
        }

        private void NextAttack()
        {
            _StateIndex++;
            if (_StateIndex >= _States.Count)
            {
                _StateIndex = 0;
            }
            _CurrentState = _States[_StateIndex];
            Pulse.SendEvent("Pulse");
            _CurrentState.StartState();
            Debug.Log(_StateIndex);
        }
    }
}