using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class Scripts_RingGenerator_Wyatt : MonoBehaviour {
	public enum UvProjection {
		AngularRadial,
		ProjectZ
	}

	[Range(0.01f, 2f)]
	[SerializeField] float radius;
	[Range(0.01f, 2f)]
	[SerializeField] float thickness;
	[Range(3, 64)]
	[SerializeField] int angularSegmentCount = 3;

	[SerializeField] UvProjection uvProjection = UvProjection.AngularRadial;

	float innerRadius => radius - thickness / 2f;
	float outerRadius => radius + thickness / 2f;
	int vertexCount => angularSegmentCount * 2;

	Mesh mesh;

	void Awake() {
		mesh = new Mesh();
		mesh.name = "QuadRing";
		GetComponent<MeshFilter>().sharedMesh = mesh;
	}

	void Update() {
		GenerateMesh();
	}

	void GenerateMesh() {
		mesh.Clear();

		// generate vertices, normals, and UVs
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		for (int i=0; i<angularSegmentCount+1; i++) {
			float t = i / (float)angularSegmentCount;
			float angRad = t * CustomGizmos.TAU;
			Vector2 dir = CustomGizmos.GetUnitVectorByAngle(angRad);

			vertices.Add(dir * outerRadius);
			vertices.Add(dir * innerRadius);

			normals.Add(Vector3.forward);
			normals.Add(Vector3.forward);

			if (uvProjection == UvProjection.AngularRadial) {
				uvs.Add(new Vector2(t, 1));
				uvs.Add(new Vector2(t, 0));
			} else if (uvProjection == UvProjection.ProjectZ) {
				uvs.Add(dir * 0.5f + Vector2.one * 0.5f); // normalize from [-1,1] to [0,1]
				uvs.Add(dir * (radius / innerRadius) * 0.5f + Vector2.one * 0.5f);
			}
		}

		List<int> triangleIndices = new List<int>();
		for (int i = 0; i < angularSegmentCount; i++) { // create two triangles at a time
			int indexRoot = i * 2; // figure out the root index common to each iteration of the loop

			int indexInnerRoot = indexRoot + 1; // define the other vertices with respect to the root
			//int indexOuterNext = (indexRoot + 2) % vertexCount; // mod by the vertex count to wrap around 
			//int indexInnerNext = (indexRoot + 3) % vertexCount; // and finish the mesh
			int indexOuterNext = indexRoot + 2; // we don't meed to wrap anymore because we have an extra vertex to handle the UVs
			int indexInnerNext = indexRoot + 3; 

			// create triangle one clockwise
			triangleIndices.Add(indexRoot);
			triangleIndices.Add(indexOuterNext);
			triangleIndices.Add(indexInnerNext);

			// create triangle two clockwise
			triangleIndices.Add(indexRoot);
			triangleIndices.Add(indexInnerNext);
			triangleIndices.Add(indexInnerRoot);
		}

		mesh.SetVertices(vertices);
		mesh.SetTriangles(triangleIndices, 0);
		mesh.SetNormals(normals);
		mesh.SetUVs(0, uvs);
	}

	void OnDrawGizmosSelected() {
		CustomGizmos.DrawWireCircle(transform.position, transform.rotation, radius, angularSegmentCount);
		CustomGizmos.DrawWireCircle(transform.position, transform.rotation, outerRadius, angularSegmentCount);
	}
}
