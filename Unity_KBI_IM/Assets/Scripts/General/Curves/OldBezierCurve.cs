using UnityEngine;

//Interpolation between 2 points with a Bezier Curve (cubic spline)
public class OldBezierCurve : MonoBehaviour 
{
    public Transform startPoint, endPoint, controlPointStart, controlPointEnd;
    Vector3 A, B, C, D;

    private void Awake() {
        A = startPoint.position;
        B = controlPointStart.position;
        C = controlPointEnd.position;
        D = endPoint.position;
    }

    void OnDrawGizmos()
    {
        A = startPoint.position;
        B = controlPointStart.position;
        C = controlPointEnd.position;
        D = endPoint.position;

        Gizmos.color = Color.white;
        float resolution = 0.02f;

        Vector3 lastPos = A;

        int loops = Mathf.FloorToInt(1f / resolution);
        for (int i = 1; i <= loops; i++)
        {
            float t = i * resolution;

            Vector3 newPos = DeCasteljausAlgorithm(t);

            Gizmos.DrawLine(lastPos, newPos);

            lastPos = newPos;
        }
		
        Gizmos.color = Color.green;
        Gizmos.DrawLine(A, B);
        Gizmos.DrawLine(C, D);
    }

    Vector3 DeCasteljausAlgorithm(float t)
    {
        float oneMinusT = 1f - t;
        
        //Layer 1
        Vector3 Q = oneMinusT * A + t * B;
        Vector3 R = oneMinusT * B + t * C;
        Vector3 S = oneMinusT * C + t * D;

        //Layer 2
        Vector3 P = oneMinusT * Q + t * R;
        Vector3 T = oneMinusT * R + t * S;

        //Final interpolated position
        Vector3 U = oneMinusT * P + t * T;

        return U;
    }

    public Vector3 Walk(float t) {
        return DeCasteljausAlgorithm(t);
    }

    public static Vector3 Walk(GameObject go, float t) {
        return go.GetComponent<OldBezierCurve>().Walk(t);
    }
}