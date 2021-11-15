using UnityEngine;

public static class CustomGizmos {
	public const float TAU = 6.28318530718f;

	public static Vector2 GetUnitVectorByAngle(float angRad) {
		return new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad));
	}

	public static void DrawWireCircle(Vector3 pos, Quaternion rot, float radius, int detail = 32) {
		Vector3[] points3D = new Vector3[detail];
		for (int i=0; i<detail; i++) {
			float t = i / (float)detail;
			float angRad = t * TAU;

			Vector2 point2D = GetUnitVectorByAngle(angRad) * radius;

			points3D[i] = pos + rot * point2D;
		}

		for (int i=0; i<detail-1; i++) {
			Gizmos.DrawLine(points3D[i], points3D[i+1]);
		} Gizmos.DrawLine(points3D[detail-1], points3D[0]);
	}

}
