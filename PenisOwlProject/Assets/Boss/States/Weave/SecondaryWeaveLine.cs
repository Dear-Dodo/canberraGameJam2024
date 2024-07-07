using System;
using UnityEngine;

namespace Boss.States.Weave
{
    public class SecondaryWeaveLine : WeaveLineBase
    {
        private PrimaryWeaveLine _FirstPrimaryWeave;
        private PrimaryWeaveLine _SecondPrimaryWeave;

        private float _TravelTime;
        
        private float _TravelStep;
        private float _CurrentTravelFraction;

        public event Action<SecondaryWeaveLine> TravelFinished; 
        
        public void InitialiseLine(PrimaryWeaveLine firstPrimaryWeave, PrimaryWeaveLine secondPrimaryWeave, 
            float timeOffset, float travelTime)
        {
            _FirstPrimaryWeave = firstPrimaryWeave;
            _SecondPrimaryWeave = secondPrimaryWeave;
            Wobbler.WobbleTimeOffset = timeOffset;
            _TravelTime = travelTime;
            _TravelStep = 1.0f / travelTime;
            SetLinePoints(0);
        }

        protected override void Update()
        {
            if (_CurrentTravelFraction.Equals(1))
                TravelFinished?.Invoke(this);
            
            _CurrentTravelFraction = Mathf.Clamp01(_CurrentTravelFraction + _TravelStep * Time.deltaTime);
            SetLinePoints(_CurrentTravelFraction);
            base.Update();
        }

        private void SetLinePoints(float t)
        {
            Wobbler.LineStartPoint = transform.InverseTransformPoint(_FirstPrimaryWeave.GetPointWorldSpace(t));
            Wobbler.LineEndPoint = transform.InverseTransformPoint(_SecondPrimaryWeave.GetPointWorldSpace(t));
        }
    }
}