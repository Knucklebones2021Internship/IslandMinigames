using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class TrackSegment : MonoBehaviour {
	[SerializeField] Mesh2D shape2D;
	[Range(2, 32)] [SerializeField] int edgeRingCount = 8;

	[Range(0f, 1f)] [SerializeField] float t = 0;
	[SerializeField] Transform[] controlPoints = new Transform[4];
	Vector3 GetPos(int i) => controlPoints[i].transform.position;

	Mesh mesh;

	void Awake() {
		mesh = new Mesh();
		mesh.name = "Road Segment";
		GetComponent<MeshFilter>().sharedMesh = mesh;
	}

	void Update() {
		GenerateMesh();
	}

	void GenerateMesh() {
		mesh.Clear();

		// vertex data
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		for (int ring=0; ring<edgeRingCount; ring++) {
			float t = ring / (edgeRingCount - 1f);
			OrientedPoint op = GetBezierOrientedPoint(t);

			for (int i=0; i<shape2D.VertexCount; i++) {
				vertices.Add(op.LocalToWorldPos(shape2D.vertices[i].point * 0.25f));
				normals.Add(op.LocalToWorldVec(shape2D.vertices[i].normal));
			}
		}

		// triangle indices
		List<int> triangles = new List<int>();
		for (int ring = 0; ring < edgeRingCount-1; ring++) {
			int rootIndex = ring * shape2D.VertexCount;
			int rootIndexNext = (ring+1) * shape2D.VertexCount;

			for (int line=0; line<shape2D.LineCount; line+=2) {
				int lineIndexA = shape2D.lineIndices[line];
				int lineIndexB = shape2D.lineIndices[line+1];

				int currentA = rootIndex + lineIndexA;
				int currentB = rootIndex + lineIndexB;
				int nextA = rootIndexNext + lineIndexA;
				int nextB = rootIndexNext + lineIndexB;

				// first triangle
				triangles.Add(currentA);
				triangles.Add(nextA);
				triangles.Add(nextB);
				// second triangle
				triangles.Add(currentA);
				triangles.Add(nextB);
				triangles.Add(currentB);
			}
		}

		mesh.SetVertices(vertices);
		mesh.SetNormals(normals);
		mesh.SetTriangles(triangles, 0);
	}

	public void OnDrawGizmos() {
		return;
		for (int i=0; i<4; i++) {
			Gizmos.DrawSphere(GetPos(i), 0.05f);
		}

		Handles.DrawBezier( GetPos(0), GetPos(3), GetPos(1), GetPos(2), Color.white, EditorGUIUtility.whiteTexture, 1f);

		OrientedPoint op = GetBezierOrientedPoint(t);

		Gizmos.color = Color.red;
		Handles.PositionHandle(op.position, op.orientation);

		void DrawPoint(Vector3 position) => Gizmos.DrawSphere(op.LocalToWorldPos(position), 0.05f);

		Vector3[] scaledVertices = shape2D.vertices.Select(v => (Vector3)v.point * 0.25f).ToArray();
		Vector3[] verts = scaledVertices.Select(v => op.LocalToWorldPos(v)).ToArray();

		for (int i=0; i<shape2D.lineIndices.Length; i+=2) {
			Vector3 p1 = verts[shape2D.lineIndices[i]];
			Vector3 p2 = verts[shape2D.lineIndices[i+1]];

			Gizmos.DrawLine(p1, p2);
			DrawPoint(scaledVertices[i]);
		}

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
