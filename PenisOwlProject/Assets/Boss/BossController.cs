using System;
using Boss.States;
using UnityEngine;

namespace Boss
{
    public class BossController : MonoBehaviour
    {
        [SerializeField]
        private OwlState _OwlState;
        
        private BossState _CurrentState;

        public void StartBehaviour()
        {
            
        }
    }
}