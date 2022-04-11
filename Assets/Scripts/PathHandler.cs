using UnityEngine;
using BansheeGz.BGSpline.Curve;

namespace Lotfy
{
    public enum Direction
    {
        Forward,Backword
    }
    
    [RequireComponent(typeof(BansheeGz.BGSpline.Curve.BGCurve))]
    public class PathHandler : MonoBehaviour
    {
        private BGCurve curve;
        private BGCurveBaseMath math;

        void Start()
        {
            curve = GetComponent<BGCurve>();
            math = new BGCurveBaseMath(curve, new BGCurveBaseMath.Config(BGCurveBaseMath.Fields.PositionAndTangent));
        }

        //getting Gust the direction To look At
        // This is the important method
        public Vector3 CalculateDirFromClosestPoint(Direction Dir, Vector3 outsidePoint, out Vector3 curvePoint)
        {
            Vector3 Dirction = CalculateTangentFromClosestPoint(outsidePoint, out curvePoint);
            Dirction.y = 0;
            switch (Dir)
            {
                case Direction.Forward:
                    return Dirction.normalized;
                case Direction.Backword:
                    return -Dirction.normalized;
                default:
                    return Vector3.zero;
            }
        }

        private Vector3 CalculateTangentFromClosestPoint(Vector3 outsidePoint, out Vector3 curvePoint)
        {
            float curvePointDistance;
            curvePoint = math.CalcPositionByClosestPoint(outsidePoint, out curvePointDistance);
            Vector3 tangent = math.CalcTangentByDistance(curvePointDistance);
            return tangent;

        }
    }
}
