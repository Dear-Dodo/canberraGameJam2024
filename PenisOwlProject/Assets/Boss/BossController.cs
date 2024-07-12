using System;
using System.Collections.Generic;
using Boss.States;
using Boss.States.Magpie;
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
        [SerializeField] 
        private MagpieAttackState _MagpieAttackState;

        private BossState _CurrentState;

        private readonly List<BossState> _States = new();

        private int _StateIndex = 0;

        private void Awake()
        {
            InitState(_WeaveAttackState);
            InitState(_SpinnyThingAttackState);
            InitState(_MagpieAttackState);
            return;

            void InitState(BossState state)
            {
                state.Boss = this;
                state.OnAttackCompletion += NextAttack;
                _States.Add(state);
            }
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