using System;
using System.Collections.Generic;
using Boss.States;
using Boss.States.Magpie;
using Boss.States.Weave;
using Boss.States.Spinny;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using System.Collections;
using UnityEngine.UI;

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
        [SerializeField]
        private MeshRenderer _OwlGeo;
        [SerializeField]
        private Material _OwlMat;
        [SerializeField]
        private Material _OwlDamageMat;
        [SerializeField]
        private Material _OwlDeathMat;
        [SerializeField]
        private Image _BossBar;

        private BossState _CurrentState;

        private readonly List<BossState> _States = new();


        private int _StateIndex = 0;

        public float MaxHealth;

        private float _health;

        private void Awake()
        {
            _health = MaxHealth;

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

        private void Update()
        {
            if (_health <= 0)
            {
                _health = 0;
            }
        }

        public void StartBehaviour()
        {
            _CurrentState = _WeaveAttackState;
            _WeaveAttackState.StartState();
        }

        private void NextAttack()
        {
            if (_health > 0)
            {
                _StateIndex++;
                if (_StateIndex >= _States.Count)
                {
                    _StateIndex = 0;
                }
                _CurrentState = _States[_StateIndex];
                Pulse.SendEvent("Pulse");
                _CurrentState.StartState();
            }
            else
            {
                StartCoroutine(Die()); //die at the end of the next attack
            }
        }

        private IEnumerator Die()
        {
            Pulse.SendEvent("Pulse");
            _OwlGeo.material = _OwlDeathMat;
            float t = 0;
            while (t < 1.1)
            {
                _OwlGeo.material.SetFloat("_Edge", t);
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        public void Damage(float Damage)
        {
            StopCoroutine(DamageFlash());
            StartCoroutine(DamageFlash());
            _health -= Damage;
            _BossBar.fillAmount = _health / MaxHealth;
        }

        private IEnumerator DamageFlash()
        {
            if (_health > 0)
            {
                _OwlGeo.material = _OwlDamageMat;
                yield return new WaitForSeconds(0.1f);
                _OwlGeo.material = _OwlMat;
            }
        }
    }
}