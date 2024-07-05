using System;
using System.Threading;
using UnityEngine;

namespace Boss.States
{
    [Serializable]
    public class OwlState : BossState
    {
        public OwlState(BossController boss) : base(boss)
        {
        }
        
        protected override void DoBehaviour(CancellationTokenSource cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}