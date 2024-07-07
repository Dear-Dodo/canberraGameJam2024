using System;
using System.Collections.Generic;
using System.Threading;
using UnityAsync;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Boss.States.Weave
{
    [Serializable]
    public class WeaveAttackState : BossState
    {
        [SerializeField] private PrimaryWeaveLine _PrimaryWeavePrefab;
        [SerializeField] private SecondaryWeaveLine _SecondaryWeavePrefab;
        
        [Header("Primary Weave Lines")]
        public int PrimaryWeaveCount = 8;
        public float PrimaryWeaveLength = 10;
        public bool WobbleTimeOffset;
        
        [Header("Secondary Weave Lines")]
        public float SecondaryWeaveInterval = 1.0f;
        public bool SecondaryWobbleTimeOffset;
        public float SecondaryWeaveTravelTime = 3.0f;

        protected override async void DoBehaviour(CancellationToken cancellationToken)
        {
            List<PrimaryWeaveLine> primaryWeaves = CreatePrimaryWeaves();

            while (_ShouldContinue())
            {
                await Await.Seconds(SecondaryWeaveInterval);
                if (!_ShouldContinue())
                    break;
                CreateSecondaryWeaves(primaryWeaves);
            }
            
            return;

            bool _ShouldContinue() => !cancellationToken.IsCancellationRequested && Application.isPlaying;
        }

        private List<PrimaryWeaveLine> CreatePrimaryWeaves()
        {
            var results = new List<PrimaryWeaveLine>(PrimaryWeaveCount);
            float radiansPerWeave = 360.0f / PrimaryWeaveCount;
            
            for (var i = 0; i < PrimaryWeaveCount; i++)
            {
                Quaternion rotation = Quaternion.AngleAxis(radiansPerWeave * i, Vector3.forward);
                PrimaryWeaveLine weave = Object.Instantiate(_PrimaryWeavePrefab, rotation * new Vector3(0, 0.1f), rotation, Boss.transform);
                float wobbleTimeOffset = WobbleTimeOffset ? Random.Range(0.0f, 100.0f) : 0;
                weave.InitialiseLine(Vector2.zero, new Vector2(0, PrimaryWeaveLength), wobbleTimeOffset);
                results.Add(weave);
            }

            return results;
        }

        private List<SecondaryWeaveLine> CreateSecondaryWeaves(List<PrimaryWeaveLine> primaryWeaves)
        {
            var results = new List<SecondaryWeaveLine>(primaryWeaves.Count);

            for (var i = 0; i < primaryWeaves.Count; i++)
            {
                int next = i+1 == primaryWeaves.Count ? 0 : i + 1;
                SecondaryWeaveLine weave = Object.Instantiate(_SecondaryWeavePrefab, Boss.transform);
                float wobbleTimeOffset = SecondaryWobbleTimeOffset ? Random.Range(0.0f, 100.0f) : 0;
                weave.InitialiseLine(primaryWeaves[i], primaryWeaves[next], wobbleTimeOffset, SecondaryWeaveTravelTime);
                weave.TravelFinished += OnSecondaryWeaveTravelFinished;
                results.Add(weave);
            }

            return results;
        }

        private void OnSecondaryWeaveTravelFinished(SecondaryWeaveLine weave)
        {
            Object.Destroy(weave.gameObject);
        }
    }
}