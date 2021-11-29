using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Scripts_QuadGenerator_Wyatt : MonoBehaviour {
	private void Awake() {
		Mesh mesh = new Mesh();
		mesh.name = "Procedural Mesh";

		List<Vector3> points = new List<Vector3>() {
			new Vector3(-0.5f,  0.5f,  0f),
			new Vector3( 0.5f,  0.5f,  0f),
			new Vector3(-0.5f, -0.5f,  0f),
			new Vector3( 0.5f, -0.5f,  0f)
		};

		int[] triIndices = new int[] {
			1, 0, 2,
			3, 1, 2,
		};

		List<Vector2> uvs = new List<Vector2>() {
			new Vector2(1, 1),
			new Vector2(0, 1),
			new Vector2(1, 0),
			new Vector2(0, 0)
		};

		List<Vector3> normals = new List<Vector3>() {
			new Vector3(0, 0, 1),
			new Vector3(0, 0, 1),
			new Vector3(0, 0, 1),
			new Vector3(0, 0, 1)
		};

		mesh.SetVertices(points);
		mesh.triangles = triIndices;
		mesh.SetNormals(normals);
		mesh.SetUVs(0, uvs);

		GetComponent<MeshFilter>().sharedMesh = mesh;
	}
}
