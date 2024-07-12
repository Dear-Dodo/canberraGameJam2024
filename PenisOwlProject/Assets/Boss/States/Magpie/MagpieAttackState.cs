using System;
using System.Threading;
using UnityAsync;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Boss.States.Magpie
{
    [Serializable]
    public class MagpieAttackState : BossState
    {
        [SerializeField] private Magpie _MagpiePrefab;
        [SerializeField] private GameObject _Chip;

        
        
        public int MagpieCount = 40;
        public float SpawnInterval = 0.15f;
        public float SpawnRadius = 15;
        public float SpawnHeight = 0f;
        public float MagpieSpeed = 2;

        private int _ActiveMagpieCount = 0;
        
        protected override async void DoBehaviour(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            _Chip.SetActive(true);
            
            for (var magpieSpawnCount = 0; magpieSpawnCount < MagpieCount; magpieSpawnCount++)
            {
                SpawnMagpie();
                await Await.Seconds(SpawnInterval).ConfigureAwait(cancellationToken);

                if (Application.isPlaying && !cancellationToken.IsCancellationRequested) continue;
                
                _Chip.SetActive(false);
                return;
            }

            await Await.Until(() => _ActiveMagpieCount == 0);
            _Chip.SetActive(false);
            CompleteAttack();
        }

        private void SpawnMagpie()
        {
            var bossPosition = Boss.transform.position;
            var radians = Random.value * 2 * Mathf.PI;
            var position = new Vector3(bossPosition.x + SpawnRadius * Mathf.Cos(radians),
                bossPosition.y + SpawnRadius * Mathf.Sin(radians), SpawnHeight);

            var magpie = Object.Instantiate(_MagpiePrefab, position, Quaternion.identity);
            magpie.Target = bossPosition;
            magpie.Speed = MagpieSpeed;
            magpie.ArrivedAtTarget += OnMagpieReachedTarget;
            
            _ActiveMagpieCount++;
        }

        private void OnMagpieReachedTarget(Magpie magpie)
        {
            Object.Destroy(magpie.gameObject);
            _ActiveMagpieCount--;
        }
    }
}