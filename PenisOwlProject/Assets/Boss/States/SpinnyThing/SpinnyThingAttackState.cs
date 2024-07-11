using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityAsync;
using UnityEngine;
namespace Boss.States.Spinny {
    [Serializable]
    public class SpinnyThingAttackState :BossState
    {
        [SerializeField]
        private SpinnyThing SpinnyThingPrefab;


        [Header("Attack Logic")]
        public float Damage;
        public float Speed;
        public AnimationCurve Acceleration;
        public int Count;
        public float Duration;

        private List<SpinnyThing> SpinnyThings = new List<SpinnyThing>();

        protected override async void DoBehaviour(CancellationToken cancellationToken)
        {

            for (int i = 0; i < Count; i++)
            {
                SpinnyThing spinnyThing = GameObject.Instantiate(SpinnyThingPrefab, Boss.transform);
                spinnyThing.Damage = Damage;
                spinnyThing.Speed = Speed * ((i == 0 || i == 3) ? 1 : -1);
                spinnyThing.Acceleration = Acceleration;
                spinnyThing.Duration = Duration;
                spinnyThing.transform.Rotate(Vector3.forward * 360 * ((i + 0.5f) / 2f));
                SpinnyThings.Add(spinnyThing);
            }

            await Await.Seconds(Duration);


            for (int i = SpinnyThings.Count - 1; i >= 0; i--)
            {
                SpinnyThing spinnyThing = SpinnyThings[i];
                GameObject.Destroy(spinnyThing.gameObject);
            }

            CompleteAttack();
        }


    }
}