using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Boss
{
    public abstract class BossState
    {
        protected BossController Boss;
        private CancellationTokenSource _CancellationSource;

        protected BossState(BossController boss)
        {
            Boss = boss;
        }

        public void StartState()
        {
            _CancellationSource = new CancellationTokenSource();
        }

        public void EndState()
        {
            _CancellationSource.Cancel();
        }

        protected abstract void DoBehaviour(CancellationTokenSource cancellationToken);
    }
}
