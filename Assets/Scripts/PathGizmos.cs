using UnityEngine;
using BansheeGz.BGSpline.Curve;

public class PathGizmos : MonoBehaviour
{
    public BGCurve curve;
    private BGCurveBaseMath math;
    Vector3 Line;
    public GameObject player;

    // This is the important method
    public Vector3 CalculateForwardFromClosestPoint(Vector3 outsidePoint, out Vector3 curvePoint)
    {
        Vector3 forward = CalculateTangentFromClosestPoint(outsidePoint, out curvePoint);
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 CalculateTangentFromClosestPoint(Vector3 outsidePoint, out Vector3 curvePoint)
    {
        float curvePointDistance;
        curvePoint = math.CalcPositionByClosestPoint(outsidePoint, out curvePointDistance);
        Vector3 tangent = math.CalcTangentByDistance(curvePointDistance);
        return tangent;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (curve)
        {
            math = new BGCurveBaseMath(curve, new BGCurveBaseMath.Config(BGCurveBaseMath.Fields.PositionAndTangent));
            CalculateForwardFromClosestPoint(player.transform.position, out Line);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(player.transform.position, Line - player.transform.position);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(Line, CalculateForwardFromClosestPoint(Line, out Line));
            Gizmos.DrawRay(player.transform.position, CalculateForwardFromClosestPoint(Line, out Line));
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(Line, -CalculateForwardFromClosestPoint(Line, out Line));
            Gizmos.DrawRay(player.transform.position, -CalculateForwardFromClosestPoint(Line, out Line));
            Gizmos.DrawSphere(Line, 0.05f);
        }
    }
#endif
}