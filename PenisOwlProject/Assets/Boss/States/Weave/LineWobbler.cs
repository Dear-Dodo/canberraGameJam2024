using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boss.States.Weave
{
    [Serializable]
    public class LineWobbler
    {
        #region Fields
        
        public float WobbleStrength = 0.2f;
        public float WobbleFrequency = 1.0f;
        public float WobbleSpeed = 1.0f;
        public float WobbleEaseDistance;
        public float WobbleTimeOffset;
        public float LineWidth = 0.2f;

        [SerializeField] private Vector2 _LineStartPoint;
        [SerializeField] private Vector2 _LineEndPoint;
        [SerializeField] private float _DivisionsPerUnit = 5;
        
        protected LineRenderer LineRenderer;
        protected EdgeCollider2D EdgeCollider;
        
        private readonly List<Vector2> _CalculatedPoints = new();

        protected bool IsDirty = false;
        
        protected Vector2 WobbleAxis;
        protected float LineLength;
        protected int DivisionCount;
        protected float NormalizedStepSize;
        
        #endregion
        #region Initialisation

        public void Initialise(LineRenderer lineRenderer, EdgeCollider2D edgeCollider)
        {
            LineRenderer = lineRenderer;
            EdgeCollider = edgeCollider;
            IsDirty = true;
        }
        
        #endregion
        #region Properties
        
        public Vector2 LineStartPoint
        {
            get => _LineStartPoint;
            set
            {
                if (_LineStartPoint == value) 
                    return;
                _LineStartPoint = value;
                RecalculateValues();
            }
        }
    
        public Vector2 LineEndPoint
        {
            get => _LineEndPoint;
            set
            {
                if (_LineEndPoint == value) 
                    return;
                _LineEndPoint = value;
                RecalculateValues();
            }
        }
        
        public float DivisionsPerUnit
        {
            get => _DivisionsPerUnit;
            set
            {
                if (_DivisionsPerUnit.Equals(value))
                    return;
                _DivisionsPerUnit = value;
                RecalculateValues();
            }
        }
        
        #endregion
        #region Public Methods

        public void UpdateLine()
        {
            if (IsDirty)
            {
                RecalculateValues();
                IsDirty = false;
            }
            LineRenderer.widthMultiplier = LineWidth * 2;
            EdgeCollider.edgeRadius = LineWidth;
            
            _CalculatedPoints.Clear();
            float easeDuration = WobbleEaseDistance / LineLength;

            for (var i = 0; i < DivisionCount + 1; i++)
            {
                float t = NormalizedStepSize * i;
                Vector2 baseLinePoint = Vector2.Lerp(LineStartPoint, LineEndPoint, t);
                _CalculatedPoints.Add(baseLinePoint + GetWobbleOffset(t, easeDuration));
            }
            
            LineRenderer.SetPositions(_CalculatedPoints.Select(v => new Vector3(v.x,v.y,0)).ToArray());
            EdgeCollider.SetPoints(_CalculatedPoints);
        }


        public Vector2 GetPoint(float t)
        {
            if (t is < 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(t));
            
            float stepCount = t / NormalizedStepSize;
            int flooredStepCount = Mathf.FloorToInt(stepCount);
            float stepFraction = stepCount - flooredStepCount;

            Vector2 lowerPoint = _CalculatedPoints[flooredStepCount];
            if (stepFraction.Equals(0))
                return lowerPoint;
            
            Vector2 upperPoint = _CalculatedPoints[flooredStepCount + 1];
            return Vector2.Lerp(lowerPoint, upperPoint, stepFraction);
        }
        
        #endregion
        #region Non-Public Methods
        
        private void RecalculateValues()
        {
            Vector2 offset = LineEndPoint - LineStartPoint;
            Vector2 direction = offset.normalized;
            WobbleAxis = Vector2.Perpendicular(direction);
            LineLength = offset.magnitude;
            DivisionCount = Mathf.CeilToInt(_DivisionsPerUnit * LineLength);
            NormalizedStepSize = 1f / DivisionCount;

            LineRenderer.positionCount = DivisionCount + 1;
        }
        
        protected static float GetEaseFactor(float easeDuration, float t)
        {
            if (easeDuration == 0) return 1;
        
            float easeInFactor = easeDuration > 0 ? t / easeDuration : 1;
            float easeOutFactor = (1 - t) / easeDuration;
            return Mathf.Clamp01(Mathf.Min(easeInFactor, easeOutFactor));
        }

        protected Vector2 GetWobbleOffset(float t, float easeDuration)
        {
            float wobbleTime = (Time.time + WobbleTimeOffset) * WobbleSpeed;
            float wobbleFactor = Mathf.Sin(wobbleTime + WobbleFrequency * t) * WobbleStrength;
            return WobbleAxis * (wobbleFactor * GetEaseFactor(easeDuration, t));
        }
        
        #endregion
    }
}