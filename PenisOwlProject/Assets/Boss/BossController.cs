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
using Unity.Cinemachine;

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
        private Material _OwlMat2;
        [SerializeField]
        private Material _OwlDamageMat2;
        [SerializeField]
        private Material _OwlDeathMat;
        [SerializeField]
        private Image _BossBar;
        [SerializeField]
        private CinemachineBasicMultiChannelPerlin _CameraShake;

        public GameObject VictoryScreen;

        private BossState _CurrentState;

        private readonly List<BossState> _States = new();


        private int _StateIndex = 0;

        private int _PhaseIndex = 0;

        public float MaxHealth;

        public float MaxPhases = 2;

        private float _health;

        private bool _dying = false;

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
            if (_health <= 0 && _PhaseIndex < MaxPhases)
            {
                _health = 0;
                if (!_dying)
                {
                    _OwlMat = _OwlMat2;
                    _OwlDamageMat = _OwlDamageMat2;
                    _OwlGeo.material = _OwlMat;
                    _CameraShake.AmplitudeGain = 1;
                    _dying = true;
                }
            }
        }

        public void StartBehaviour()
        {
            _CurrentState = _WeaveAttackState;
            _WeaveAttackState.StartState();
        }

        private void NextAttack()
        {
            if (_health > 0 && _PhaseIndex < MaxPhases)
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
                _PhaseIndex++;
                if (_PhaseIndex < MaxPhases)
                {
                    _health = MaxHealth;
                    _dying = false;
                    _CameraShake.AmplitudeGain = 0;
                    _BossBar.color = new Color(0.75f, 0, 0);
                    _BossBar.fillAmount = 1;

                    _WeaveAttackState.SecondaryWeaveInterval -= 1.25f;
                    _WeaveAttackState.WeaveCount += 2;
                    _SpinnyThingAttackState.Count += 2;
                    _MagpieAttackState.MagpieSpeed += 2;
                    _MagpieAttackState.MagpieCount += 20;
                    _MagpieAttackState.SpawnInterval = 0.1f;

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
            Time.timeScale = 0;
            VictoryScreen.SetActive(true);
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