using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
public class TrackSegment : MonoBehaviour {
	[Range(0.015f, 0.98f)] [SerializeField] float t = 0;
	[SerializeField] Transform[] controlPoints = new Transform[4];

	Vector3 GetPos(int i) => controlPoints[i].transform.position;

	public void OnDrawGizmos() {
		for (int i=0; i<4; i++) {
			Gizmos.DrawSphere(GetPos(i), 0.05f);
		}

		Handles.DrawBezier( GetPos(0), GetPos(3), GetPos(1), GetPos(2), Color.white, EditorGUIUtility.whiteTexture, 1f);

		OrientedPoint op = GetBezierOrientedPoint(t);

		Gizmos.color = Color.red;
		Handles.PositionHandle(op.position, op.orientation);

		float radius = 0.03f;
		float thickness = 0.1f;
		Gizmos.DrawSphere(op.LocalToWorld(Vector3.right * thickness*2f), radius);
		Gizmos.DrawSphere(op.LocalToWorld(Vector3.right * thickness), radius);
		Gizmos.DrawSphere(op.LocalToWorld(Vector3.right * -thickness*2f), radius);
		Gizmos.DrawSphere(op.LocalToWorld(Vector3.right * -thickness), radius);
		Gizmos.DrawSphere(op.LocalToWorld(Vector3.up * thickness*2f), radius);
		Gizmos.DrawSphere(op.LocalToWorld(Vector3.up * thickness), radius);

		Gizmos.color = Color.white;
	}

	OrientedPoint GetBezierOrientedPoint(float t) {
		Vector3 p0 = GetPos(0);
		Vector3 p1 = GetPos(1);
		Vector3 p2 = GetPos(2);
		Vector3 p3 = GetPos(3);

		Vector3 a = Vector3.Lerp(p0, p1, t);
		Vector3 b = Vector3.Lerp(p1, p2, t);
		Vector3 c = Vector3.Lerp(p2, p3, t);

		Vector3 d = Vector3.Lerp(a, b, t);
		Vector3 e = Vector3.Lerp(b, c, t);

		Vector3 tangent = (e - d).normalized;

		return new OrientedPoint(
			Vector3.Lerp(d, e, t),
			Quaternion.LookRotation(tangent)
		);
	}
}
