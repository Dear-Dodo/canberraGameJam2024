using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Boss
{
    public abstract class BossState
    {
        public BossController Boss;
        private CancellationTokenSource _CancellationSource;

        public void StartState()
        {
            _CancellationSource = new CancellationTokenSource();
            DoBehaviour(_CancellationSource.Token);
        }

        public void EndState()
        {
            _CancellationSource.Cancel();
        }

        protected abstract void DoBehaviour(CancellationToken cancellationToken);
    }
}
