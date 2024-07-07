using UnityEngine;

namespace Boss.States.Weave
{
    public class PrimaryWeaveLine : WeaveLineBase
    {
        public Vector3 GetPointWorldSpace(float t)
        {
            Vector3 localPoint = Wobbler.GetPoint(t);
            return transform.TransformVector(localPoint);
        }
        
        public void InitialiseLine(Vector2 startPoint, Vector2 endPoint, float timeOffset)
        {
            Wobbler.LineStartPoint = startPoint;
            Wobbler.LineEndPoint = endPoint;
            Wobbler.WobbleTimeOffset = timeOffset;
            Wobbler.UpdateLine();
        }
    }
}
