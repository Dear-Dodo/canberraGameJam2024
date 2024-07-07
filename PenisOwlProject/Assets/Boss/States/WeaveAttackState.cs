using System;
using System.Threading;
using UnityEngine;

namespace Boss.States
{
    [Serializable]
    public class WeaveAttackState : BossState
    {
        public WeaveAttackState(BossController boss) : base(boss)
        {
        }
        
        protected override void DoBehaviour(CancellationTokenSource cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}